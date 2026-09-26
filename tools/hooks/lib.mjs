// Shared helpers for the Claude Code hooks in this folder (ADR 004).
// Hooks read one JSON object on stdin. Exit code 2 blocks the action and
// shows stderr to the agent; exit code 0 allows it.
import { readFileSync } from 'node:fs';

export function readHookInput() {
  try {
    return JSON.parse(readFileSync(0, 'utf8') || '{}');
  } catch {
    return {};
  }
}

export function block(reason) {
  process.stderr.write(`${reason}\n`);
  process.exit(2);
}

export function toRepoRelative(filePath, root) {
  // Accept Windows (E:\x), forward-slash (E:/x) and Git Bash (/e/x) forms.
  const norm = (p) => p.replace(/\\/g, '/').replace(/^\/([a-z])\//i, '$1:/').replace(/\/+$/, '');
  const file = norm(filePath || '');
  const base = norm(root || '');
  if (base && file.toLowerCase().startsWith(`${base.toLowerCase()}/`)) {
    return file.slice(base.length + 1);
  }
  return file.replace(/^\.\//, '');
}
