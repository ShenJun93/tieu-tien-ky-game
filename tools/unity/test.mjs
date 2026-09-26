#!/usr/bin/env node
// Run Unity EditMode or PlayMode tests headless and print a one-line verdict.
//   node tools/unity/test.mjs [EditMode|PlayMode] [--filter <name>]
// Finds the editor from UNITY_EDITOR, else Unity Hub's install folder for the
// version in ProjectSettings/ProjectVersion.txt. Exit 0 = all tests passed.
import { spawnSync } from 'node:child_process';
import { existsSync, mkdirSync, readFileSync } from 'node:fs';
import { homedir } from 'node:os';
import { join, resolve } from 'node:path';
import { pathToFileURL } from 'node:url';

export function parseResults(xml) {
  const run = xml.match(/<test-run\b[^>]*>/);
  if (!run) return null;
  const attr = (name) => {
    const m = run[0].match(new RegExp(`\\b${name}="([^"]*)"`));
    return m ? m[1] : '';
  };
  const failures = [...xml.matchAll(/<test-case\b[^>]*\bfullname="([^"]*)"[^>]*\bresult="Failed"/g)].map((m) => m[1]);
  return {
    result: attr('result'),
    total: Number(attr('total') || 0),
    passed: Number(attr('passed') || 0),
    failed: Number(attr('failed') || 0),
    skipped: Number(attr('skipped') || 0),
    failures,
  };
}

function editorVersion(root) {
  const text = readFileSync(join(root, 'ProjectSettings/ProjectVersion.txt'), 'utf8');
  return text.match(/m_EditorVersion:\s*(\S+)/)[1];
}

export function findEditor(root, env = process.env) {
  if (env.UNITY_EDITOR) return env.UNITY_EDITOR;
  const version = editorVersion(root);
  const hubDirs = [];
  const hubConfig = join(env.APPDATA || join(homedir(), '.config'), 'UnityHub', 'secondaryInstallPath.json');
  if (existsSync(hubConfig)) {
    try { const dir = JSON.parse(readFileSync(hubConfig, 'utf8')); if (dir) hubDirs.push(dir); } catch { /* ignore */ }
  }
  hubDirs.push('C:/Program Files/Unity/Hub/Editor', '/Applications/Unity/Hub/Editor', join(homedir(), 'Unity/Hub/Editor'));
  for (const dir of hubDirs) {
    for (const exe of ['Editor/Unity.exe', 'Unity.app/Contents/MacOS/Unity', 'Editor/Unity']) {
      const candidate = join(dir, version, exe);
      if (existsSync(candidate)) return candidate;
    }
  }
  return null;
}

function main() {
  const root = resolve(process.cwd());
  const platform = process.argv[2] === 'PlayMode' ? 'PlayMode' : 'EditMode';
  const filterAt = process.argv.indexOf('--filter');
  const editor = findEditor(root);
  if (!editor) {
    console.error(`unity-test ${platform}: BLOCKED (Unity ${editorVersion(root)} not found; set UNITY_EDITOR)`);
    process.exit(3);
  }
  // Not Temp/: Unity wipes the project Temp folder when it exits.
  const outDir = join(root, 'Logs', 'unity-tests');
  mkdirSync(outDir, { recursive: true });
  const resultsPath = join(outDir, `${platform.toLowerCase()}-results.xml`);
  const logPath = join(outDir, `${platform.toLowerCase()}-test.log`);
  // Never add -quit to -runTests: the runner exits by itself and -quit truncates results.
  const args = ['-batchmode', '-nographics', '-projectPath', root, '-runTests', '-testPlatform', platform, '-testResults', resultsPath, '-logFile', logPath];
  if (filterAt > 0 && process.argv[filterAt + 1]) args.push('-testFilter', process.argv[filterAt + 1]);
  const run = spawnSync(editor, args, { stdio: 'ignore' });
  if (!existsSync(resultsPath)) {
    const log = existsSync(logPath) ? readFileSync(logPath, 'utf8') : '';
    const locked = /another Unity instance is running|It looks like another Unity instance/i.test(log);
    console.error(`unity-test ${platform}: ${locked ? 'BLOCKED (the project is open in another Unity Editor)' : `FAIL (no results; exit ${run.status}; see ${logPath})`}`);
    process.exit(locked ? 3 : 1);
  }
  const r = parseResults(readFileSync(resultsPath, 'utf8'));
  const ok = r && r.failed === 0 && r.total > 0 && run.status === 0;
  console.log(`unity-test ${platform}: ${ok ? 'PASS' : 'FAIL'} (${r?.passed ?? 0}/${r?.total ?? 0} passed, ${r?.failed ?? 0} failed, ${r?.skipped ?? 0} skipped)`);
  for (const name of r?.failures ?? []) console.log(`  failed: ${name}`);
  process.exit(ok ? 0 : 1);
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) main();
