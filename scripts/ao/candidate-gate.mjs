import path from 'node:path';
import { spawnSync } from 'node:child_process';
import {
  getCurrentBranch,
  getHeadSha,
  getStatusPorcelain,
  isAncestor,
  resolveExactCommit,
  resolveLiveMain,
  runGit
} from './git-state.mjs';

const SHA40 = /^[0-9a-f]{40}$/i;

function result(status, candidateSha, extra = {}) {
  return {
    gate_status: status,
    candidate_sha: candidateSha ?? null,
    checks: extra.checks ?? [],
    final_head_sha: extra.final_head_sha ?? null,
    final_tree_state: extra.final_tree_state ?? null,
    mutation_detected: extra.mutation_detected ?? false,
    writer_paths: extra.writer_paths ?? [],
    error: extra.error ?? null
  };
}

function normalizePath(value) {
  if (typeof value !== 'string' || !value) throw new Error('path must be non-empty');
  const v = value.replaceAll('\\', '/');
  if (path.posix.isAbsolute(v) || /^[A-Za-z]:\//.test(v)) throw new Error(`absolute path is not allowed: ${value}`);
  const parts = v.split('/');
  if (parts.some(p => p === '..')) throw new Error(`path traversal is not allowed: ${value}`);
  return parts.filter(p => p && p !== '.').join('/');
}

function matchesRule(file, rule) {
  const normalizedRule = normalizePath(rule);
  if (rule.endsWith('/')) return file === normalizedRule || file.startsWith(`${normalizedRule}/`);
  return file === normalizedRule;
}

function isAllowed(file, allowed, forbidden, taskFile) {
  const locked = ['docs/governance/NEXT_TASK.md', taskFile].filter(Boolean);
  if (locked.includes(file)) return false;
  if (forbidden.some(rule => matchesRule(file, rule))) return false;
  return allowed.some(rule => matchesRule(file, rule));
}

function writerPaths(root, transition, candidate) {
  const output = runGit(root, ['diff', '--name-only', '--no-renames', `${transition}...${candidate}`, '--']).stdout;
  return output ? output.split(/\r?\n/).filter(Boolean).map(normalizePath).sort() : [];
}

function defaultRunProcess(root, check) {
  if (!Array.isArray(check.argv) || check.argv.length === 0) {
    return { status: 2, stdout: '', stderr: 'invalid check argv' };
  }
  const [command, ...args] = check.argv;
  const proc = spawnSync(command, args, {
    cwd: root,
    encoding: 'utf8',
    timeout: check.timeout_ms ?? 300000
  });
  return {
    status: proc.status ?? (proc.error ? 1 : 0),
    stdout: proc.stdout ?? '',
    stderr: proc.stderr ?? (proc.error?.message ?? '')
  };
}

export function verifyCandidate({
  root,
  candidateSha,
  authoritySnapshot,
  checks = [],
  runProcess = defaultRunProcess
}) {
  const checkResults = [];
  const finish = (status, error, extra = {}) => result(status, candidateSha, {
    checks: checkResults,
    final_head_sha: (() => { try { return getHeadSha(root); } catch { return null; } })(),
    final_tree_state: (() => { try { return getStatusPorcelain(root) === '' ? 'clean' : 'dirty'; } catch { return null; } })(),
    error,
    ...extra
  });

  if (!authoritySnapshot || authoritySnapshot.gate_status !== 'PASS' || !['IMPLEMENT', 'SPIKE'].includes(authoritySnapshot.mutation_authority)) {
    return finish('BLOCKED_AUTHORITY', 'active mutating authority is required');
  }
  if (authoritySnapshot.workspace_policy === 'REMOTE_GITHUB_BRANCH') {
    return finish('BLOCKED_WORKSPACE', 'REMOTE_GITHUB_BRANCH is not a local candidate-verification surface');
  }
  if (typeof candidateSha !== 'string' || !SHA40.test(candidateSha)) {
    return finish('BLOCKED_CANDIDATE', 'candidate must be an exact 40-character SHA');
  }

  try {
    resolveExactCommit(root, candidateSha);
    const head = getHeadSha(root);
    if (head !== candidateSha.toLowerCase()) return finish('BLOCKED_CANDIDATE', 'current HEAD does not equal candidate');
    if (getCurrentBranch(root) !== authoritySnapshot.branch) return finish('BLOCKED_BRANCH', 'current branch does not equal authority branch');
    if (getStatusPorcelain(root) !== '') return finish('BLOCKED_CANDIDATE', 'candidate tree must be clean');
    if (!isAncestor(root, authoritySnapshot.baseline_ref, candidateSha)) return finish('BLOCKED_CANDIDATE', 'baseline is not an ancestor of candidate');

    const liveMain = resolveLiveMain(root);
    if (liveMain !== authoritySnapshot.baseline_ref.toLowerCase()) return finish('BLOCKED_LIVE_MAIN_DRIFT', 'live main drifted');

    const transition = authoritySnapshot.authority_transition_ref;
    if (!transition || !isAncestor(root, transition, candidateSha)) return finish('BLOCKED_CANDIDATE', 'authority transition is not an ancestor of candidate');

    const paths = writerPaths(root, transition, candidateSha);
    if (paths.length === 0) return finish('BLOCKED_CANDIDATE', 'candidate has no writer delta', { writer_paths: paths });

    const allowed = authoritySnapshot.allowed_paths ?? [];
    const forbidden = authoritySnapshot.forbidden_paths ?? [];
    for (const file of paths) {
      if (!isAllowed(file, allowed, forbidden, authoritySnapshot.task_file)) {
        return finish('BLOCKED_SCOPE', `writer path outside scope: ${file}`, { writer_paths: paths });
      }
    }

    for (const check of checks) {
      const beforeHead = getHeadSha(root);
      const beforeClean = getStatusPorcelain(root) === '';
      if (!beforeClean) return finish('BLOCKED_CANDIDATE', 'tree became dirty before check', { writer_paths: paths });

      const execution = runProcess(root, check);
      const afterHead = getHeadSha(root);
      const afterClean = getStatusPorcelain(root) === '';
      checkResults.push({
        name: check.name,
        status: execution.status === 0 ? 'PASS' : 'FAIL',
        exit_code: execution.status,
        stdout: execution.stdout ?? '',
        stderr: execution.stderr ?? ''
      });

      if (afterHead !== beforeHead || !afterClean) {
        return finish('CHECK_MUTATED_CANDIDATE', `check mutated candidate: ${check.name}`, {
          mutation_detected: true,
          writer_paths: paths
        });
      }
      if (execution.status !== 0) {
        return finish('BLOCKED_CHECK', `candidate check failed: ${check.name}`, { writer_paths: paths });
      }
    }

    return finish('PASS', null, { writer_paths: paths });
  } catch (error) {
    return finish('BLOCKED_CANDIDATE', error.message);
  }
}
