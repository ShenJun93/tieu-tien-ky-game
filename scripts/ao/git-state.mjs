import path from 'node:path';
import { spawnSync } from 'node:child_process';

const SHA40 = /^[0-9a-f]{40}$/i;

export function runGit(root, args, options = {}) {
  if (!Array.isArray(args) || args.some(arg => typeof arg !== 'string')) {
    throw new TypeError('git args must be an array of strings');
  }
  const result = spawnSync('git', args, {
    cwd: root,
    encoding: 'utf8',
    timeout: options.timeout_ms ?? options.timeout ?? 300000,
    input: options.input,
    env: options.env ?? process.env
  });
  if (result.error) {
    const error = new Error(`git ${args[0] ?? ''} failed to start: ${result.error.message}`);
    error.code = 'GIT_EXEC_ERROR';
    error.exitCode = null;
    throw error;
  }
  if (result.status !== 0 && !options.allowFailure) {
    const stderr = (result.stderr || '').trim();
    const error = new Error(`git ${args[0] ?? ''} failed (${result.status})${stderr ? `: ${stderr}` : ''}`);
    error.code = 'GIT_COMMAND_FAILED';
    error.exitCode = result.status;
    throw error;
  }
  return {
    status: result.status ?? 1,
    stdout: (result.stdout || '').trim(),
    stderr: (result.stderr || '').trim()
  };
}

export function findRepoRoot(cwd = process.cwd()) {
  return path.resolve(runGit(cwd, ['rev-parse', '--show-toplevel']).stdout);
}

export function getCurrentBranch(root) {
  return runGit(root, ['branch', '--show-current']).stdout;
}

export function getHeadSha(root) {
  return runGit(root, ['rev-parse', 'HEAD']).stdout.toLowerCase();
}

export function getStatusPorcelain(root) {
  return runGit(root, ['status', '--porcelain']).stdout;
}

export function isTreeClean(root) {
  return getStatusPorcelain(root) === '';
}

export function getOriginUrl(root) {
  return runGit(root, ['remote', 'get-url', 'origin']).stdout;
}

export function normalizeRepositoryUrl(value) {
  if (typeof value !== 'string' || !value.trim()) return '';
  let v = value.trim().replaceAll('\\', '/');

  const ssh = v.match(/^git@github\.com:([^/]+)\/(.+?)(?:\.git)?$/i);
  if (ssh) return `${ssh[1]}/${ssh[2].replace(/\.git$/i, '')}`.toLowerCase();

  try {
    const url = new URL(v);
    if (url.hostname.toLowerCase() === 'github.com') {
      return url.pathname.replace(/^\/+/, '').replace(/\.git$/i, '').toLowerCase();
    }
  } catch {
    // local path / non-URL remote
  }

  return v.replace(/\/+$/, '').replace(/\.git$/i, '');
}

export function resolveExactCommit(root, ref) {
  if (typeof ref !== 'string' || !SHA40.test(ref)) {
    throw new Error('ref must be an exact 40-character commit SHA');
  }
  const resolved = runGit(root, ['rev-parse', '--verify', `${ref}^{commit}`]).stdout.toLowerCase();
  if (resolved !== ref.toLowerCase()) {
    throw new Error('exact commit SHA did not resolve to itself');
  }
  return resolved;
}

export function isAncestor(root, ancestor, descendant) {
  resolveExactCommit(root, ancestor);
  resolveExactCommit(root, descendant);
  const result = runGit(root, ['merge-base', '--is-ancestor', ancestor, descendant], { allowFailure: true });
  if (result.status === 0) return true;
  if (result.status === 1) return false;
  throw new Error(`git merge-base failed (${result.status})`);
}

export function changedPaths(root, fromRef, toRef) {
  const output = runGit(root, ['diff', '--name-only', '--no-renames', `${fromRef}..${toRef}`, '--']).stdout;
  return output ? output.split(/\r?\n/).filter(Boolean).sort() : [];
}

export function listWorktrees(root) {
  const output = runGit(root, ['worktree', 'list', '--porcelain']).stdout;
  if (!output) return [];
  const records = [];
  let current = null;
  for (const line of output.split(/\r?\n/)) {
    if (line.startsWith('worktree ')) {
      if (current) records.push(current);
      current = { path: line.slice('worktree '.length) };
    } else if (current && line.startsWith('HEAD ')) {
      current.head = line.slice('HEAD '.length);
    } else if (current && line.startsWith('branch ')) {
      current.branch = line.slice('branch '.length);
    } else if (current && line === 'bare') {
      current.bare = true;
    } else if (current && line === 'detached') {
      current.detached = true;
    }
  }
  if (current) records.push(current);
  return records;
}

export function resolveLiveMain(root) {
  const output = runGit(root, ['ls-remote', '--exit-code', 'origin', 'refs/heads/main']).stdout;
  const lines = output.split(/\r?\n/).filter(Boolean);
  if (lines.length !== 1) throw new Error('origin/main did not resolve to exactly one ref');
  const match = lines[0].match(/^([0-9a-f]{40})\s+refs\/heads\/main$/i);
  if (!match) throw new Error('origin/main did not return an exact commit SHA');
  return match[1].toLowerCase();
}

function resolveGitPath(root, value) {
  if (path.isAbsolute(value)) return path.resolve(value);
  return path.resolve(root, value);
}

export function getGitDir(root) {
  return resolveGitPath(root, runGit(root, ['rev-parse', '--git-dir']).stdout);
}

export function getGitCommonDir(root) {
  return resolveGitPath(root, runGit(root, ['rev-parse', '--git-common-dir']).stdout);
}

export const EXACT_SHA_PATTERN = SHA40;
