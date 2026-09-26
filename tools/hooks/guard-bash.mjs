#!/usr/bin/env node
// PreToolUse hook for Bash. Only the Director merges, and main only changes
// through reviewed PRs (AGENTS.md rule 1).
import { execFileSync } from 'node:child_process';
import { pathToFileURL } from 'node:url';
import { block, readHookInput } from './lib.mjs';

const PROTECTED = ['main', 'master'];

// Split on shell separators so each simple command is checked on its own.
const segments = (command) => command.split(/&&|\|\||;|\||\n/).map((s) => s.trim()).filter(Boolean);

export function checkCommand(command, currentBranch) {
  for (const seg of segments(command)) {
    const words = seg.split(/\s+/);
    const gitAt = words.indexOf('git');
    if (/^gh\s+pr\s+merge\b/.test(seg) || /\bgh\s+pr\s+merge\b/.test(seg)) {
      return 'Blocked: only the Director merges pull requests (AGENTS.md rule 1).';
    }
    if (gitAt === -1) continue;
    const sub = words.slice(gitAt + 1).filter((w, i, all) => !(all[i - 1] === '-C' || w === '-C'));
    const verb = sub.find((w) => !w.startsWith('-'));
    const args = sub.slice(sub.indexOf(verb) + 1);
    if (verb === 'push') {
      const refs = args.filter((a) => !a.startsWith('-'));
      const targetsProtected = refs.slice(1).some((r) => PROTECTED.includes(r.split(':').pop().replace(/^\+/, '').replace(/^refs\/heads\//, '')));
      const implicitPushFromProtected = refs.length <= 1 && PROTECTED.includes(currentBranch);
      if (targetsProtected || implicitPushFromProtected) {
        return 'Blocked: never push to main. Push a branch and open a PR (AGENTS.md rule 1).';
      }
    }
    if (verb === 'commit' && PROTECTED.includes(currentBranch)) {
      return `Blocked: you are on ${currentBranch}. Create a branch before committing (AGENTS.md rule 1).`;
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
  const reason = checkCommand(command, currentBranchOf(input.cwd || process.cwd()));
  if (reason) block(reason);
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) main();
