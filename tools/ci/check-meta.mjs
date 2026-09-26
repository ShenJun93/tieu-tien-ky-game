#!/usr/bin/env node
// Fails when a tracked Unity asset (file or folder) under Assets/ has no
// tracked .meta, or a tracked .meta has no matching asset. Missing or orphaned
// .meta files break GUID references in scenes and prefabs.
// Known pre-existing problems listed in check-meta.known-debt.txt only warn;
// fix them and delete the line (R0.2 regenerates the missing folder metas).
import { execFileSync } from 'node:child_process';
import { existsSync, readFileSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath, pathToFileURL } from 'node:url';

// Unity does not import hidden names (".foo") or names ending in "~".
const ignoredByUnity = (segment) => segment.startsWith('.') || segment.endsWith('~');

export function findMetaProblems(trackedPaths) {
  const files = new Set();
  const metas = new Set();
  const dirs = new Set();

  for (const raw of trackedPaths) {
    const p = raw.replace(/\\/g, '/');
    if (!p.startsWith('Assets/')) continue;
    const parts = p.split('/');
    const firstIgnored = parts.findIndex(ignoredByUnity);
    // Deepest visible parent folder; a folder holding only hidden files
    // (e.g. .gitkeep) is still a real folder that needs its own .meta.
    const deepestDir = firstIgnored === -1 ? parts.length - 1 : firstIgnored;
    for (let i = 2; i <= deepestDir; i++) dirs.add(parts.slice(0, i).join('/'));
    if (firstIgnored !== -1) continue;
    if (p.endsWith('.meta')) metas.add(p);
    else files.add(p);
  }

  const problems = [];
  for (const asset of [...files, ...dirs]) {
    if (!metas.has(`${asset}.meta`)) problems.push(`missing .meta: ${asset}`);
  }
  for (const meta of metas) {
    const asset = meta.slice(0, -'.meta'.length);
    if (!files.has(asset) && !dirs.has(asset)) problems.push(`orphaned .meta: ${meta}`);
  }
  return problems.sort();
}

export function splitKnownDebt(problems, knownDebt) {
  const known = new Set(knownDebt);
  return {
    blocking: problems.filter((p) => !known.has(p)),
    tolerated: problems.filter((p) => known.has(p)),
  };
}

function readKnownDebt() {
  const file = join(dirname(fileURLToPath(import.meta.url)), 'check-meta.known-debt.txt');
  if (!existsSync(file)) return [];
  return readFileSync(file, 'utf8')
    .split(/\r?\n/)
    .map((line) => line.trim())
    .filter((line) => line && !line.startsWith('#'));
}

function main() {
  const out = execFileSync('git', ['ls-files', '-z', '--', 'Assets'], { encoding: 'utf8' });
  const tracked = out.split('\0').filter(Boolean);
  const { blocking, tolerated } = splitKnownDebt(findMetaProblems(tracked), readKnownDebt());
  for (const line of tolerated) console.warn(`  WARN known debt: ${line}`);
  if (blocking.length > 0) {
    console.error(`check-meta: FAIL (${blocking.length} new problem(s))`);
    for (const line of blocking) console.error(`  ${line}`);
    process.exit(1);
  }
  console.log(`check-meta: PASS (${tracked.length} tracked paths, ${tolerated.length} known-debt warning(s))`);
}

if (import.meta.url === pathToFileURL(process.argv[1]).href) main();
