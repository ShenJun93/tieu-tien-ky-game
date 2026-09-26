#!/usr/bin/env node
// PreToolUse hook for Bash. Only the Director merges, and main only changes
// through reviewed PRs (AGENTS.md rule 1). The branch is evaluated where each
// git command actually runs: after `cd <dir>`, with `git -C <dir>`, and after
// `git switch -c` / `git checkout -b` earlier in the same command.
import { execFileSync } from 'node:child_process';
import { isAbsolute, join } from 'node:path';
import { pathToFileURL } from 'node:url';
import { block, readHookInput } from './lib.mjs';

const PROTECTED = ['main', 'master'];

// Heredoc bodies (commit messages, PR bodies) are data, not commands.
export function stripHeredocs(command) {
  const lines = command.split('\n');
  const out = [];
  let terminator = null;
  for (const line of lines) {
    if (terminator) {
      if (line.trim() === terminator) terminator = null;
      continue;
    }
    out.push(line);
    const m = line.match(/<<-?\s*['"]?([A-Za-z_][A-Za-z0-9_]*)['"]?/);
    if (m) terminator = m[1];
  }
  return out.join('\n');
}

// Split on shell separators so each simple command is checked on its own.
const segments = (command) => stripHeredocs(command).split(/&&|\|\||;|\||\n/).map((s) => s.trim()).filter(Boolean);
const unquote = (s) => s.replace(/^["']|["']$/g, '');
// Git Bash paths (/e/x) -> Windows form (e:/x) so node can use them as cwd.
const toNativeDir = (p) => p.replace(/^\/([a-z])(\/|$)/i, '$1:/');
const resolveDir = (base, target) => {
  const t = toNativeDir(unquote(target));
  return isAbsolute(t) ? t : join(base, t);
};

/**
 * @param {string} command  full Bash command
 * @param {string|((dir: string) => string)} branchAt  branch name, or a lookup by directory
 * @param {string} startDir  directory the command starts in
 */
export function checkCommand(command, branchAt, startDir = '.') {
  const branchOf = typeof branchAt === 'function' ? branchAt : () => branchAt;
  const switched = new Map();
  let dir = startDir;

  for (const seg of segments(command)) {
    if (/\bgh\s+pr\s+merge\b/.test(seg)) {
      return 'Blocked: only the Director merges pull requests (AGENTS.md rule 1).';
    }
    const words = seg.split(/\s+/);
    // Skip leading VAR=value assignments; the command word decides what runs.
    let at = 0;
    while (at < words.length && /^[A-Za-z_][A-Za-z0-9_]*=/.test(words[at])) at++;
    if (words[at] === 'cd' && words[at + 1]) {
      dir = resolveDir(dir, words[at + 1]);
      continue;
    }
    if (words[at] !== 'git') continue;

    let gitDir = dir;
    const rest = [];
    for (let i = at + 1; i < words.length; i++) {
      if (words[i] === '-C' && words[i + 1]) { gitDir = resolveDir(dir, words[++i]); continue; }
      rest.push(words[i]);
    }
    const verb = rest.find((w) => !w.startsWith('-'));
    const args = rest.slice(rest.indexOf(verb) + 1);
    const branch = switched.get(gitDir) ?? branchOf(gitDir);

    if ((verb === 'switch' && (args.includes('-c') || args.includes('-C'))) || (verb === 'checkout' && (args.includes('-b') || args.includes('-B')))) {
      const name = args.find((a, i) => i > 0 && ['-c', '-C', '-b', '-B'].includes(args[i - 1]));
      if (name) switched.set(gitDir, name);
      continue;
    }
    if ((verb === 'switch' || verb === 'checkout') && args[0] && !args[0].startsWith('-')) {
      switched.set(gitDir, args[0]);
      continue;
    }
    if (verb === 'push') {
      const refs = args.filter((a) => !a.startsWith('-'));
      const targetsProtected = refs.slice(1).some((r) => PROTECTED.includes(r.split(':').pop().replace(/^\+/, '').replace(/^refs\/heads\//, '')));
      const implicitPushFromProtected = refs.length <= 1 && PROTECTED.includes(branch);
      if (targetsProtected || implicitPushFromProtected) {
        return 'Blocked: never push to main. Push a branch and open a PR (AGENTS.md rule 1).';
      }
    }
    if (verb === 'commit' && PROTECTED.includes(branch)) {
      return `Blocked: you are on ${branch} (${gitDir}). Create a branch before committing (AGENTS.md rule 1).`;
    }
  }
  return null;
}

function currentBranchOf(cwd) {
  try {
    return execFileSync('git', ['rev-parse', '--abbrev-ref', 'HEAD'], { cwd, encoding: 'utf8', stdio: ['ignore', 'pipe', 'ignore'] }).trim();
  } catch {
    return '';
  }
}

function main() {
  const input = readHookInput();
  const command = (input.tool_input || {}).command;
  if (!command || !/\b(git|gh)\b/.test(command)) return;
  const reason = checkCommand(command, currentBranchOf, toNativeDir(input.cwd || process.cwd()));
  if (reason) block(reason);
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) main();
