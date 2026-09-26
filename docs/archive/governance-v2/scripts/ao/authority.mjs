import fs from 'node:fs';
import path from 'node:path';
import {
  changedPaths,
  findRepoRoot,
  getCurrentBranch,
  getHeadSha,
  getOriginUrl,
  getStatusPorcelain,
  isAncestor,
  normalizeRepositoryUrl,
  resolveExactCommit,
  resolveLiveMain,
  runGit
} from './git-state.mjs';

export const KNOWN_STATES = new Set(['PAUSED', 'DISCOVERY', 'SPIKE', 'IMPLEMENT', 'REVIEW', 'HUMAN_GATE', 'CLOSED']);
export const MUTATING_STATES = new Set(['SPIKE', 'IMPLEMENT']);
export const TASK_MODES = new Set(['MICRO', 'SLICE', 'SPEC', 'BATCH', 'SPIKE', 'PARALLEL']);
export const WORKSPACE_POLICIES = new Set(['ISOLATED_WORKTREE', 'EXISTING_AUTHORIZED_WORKTREE', 'REMOTE_GITHUB_BRANCH']);

class GateError extends Error {
  constructor(gateStatus, message) {
    super(message);
    this.gateStatus = gateStatus;
  }
}

function gate(status, message) {
  throw new GateError(status, message);
}

function config(message) {
  gate('INVALID_INVOCATION', message);
}

export function readAuthority(root) {
  const file = path.join(root, 'docs/governance/NEXT_TASK.md');
  if (!fs.existsSync(file)) config('NEXT_TASK.md is missing');
  const text = fs.readFileSync(file, 'utf8');
  const match = text.match(/```json\s*([\s\S]*?)```/i);
  if (!match) config('NEXT_TASK.md has no JSON authority block');
  try {
    return JSON.parse(match[1]);
  } catch (error) {
    config(`NEXT_TASK.md JSON is malformed: ${error.message}`);
  }
}

function requireString(value, name) {
  if (typeof value !== 'string' || !value.trim()) config(`${name} must be a non-empty string`);
}

function requireStringArray(value, name) {
  if (!Array.isArray(value) || value.some(v => typeof v !== 'string')) {
    config(`${name} must be a string array`);
  }
}

function requireExactShaWhenDeclared(value, name) {
  if (value != null && !/^[0-9a-f]{40}$/i.test(value)) {
    config(`${name} must be an exact 40-character commit SHA`);
  }
}

export function validateAuthorityShape(authority) {
  if (!authority || typeof authority !== 'object' || Array.isArray(authority)) {
    config('authority must be an object');
  }
  if (!KNOWN_STATES.has(authority.state)) config(`unknown authority state: ${authority.state}`);

  requireStringArray(authority.allowed_paths ?? [], 'allowed_paths');
  requireStringArray(authority.forbidden_paths ?? [], 'forbidden_paths');
  requireString(authority.stop_condition, 'stop_condition');

  if (authority.task_mode != null) {
    requireString(authority.task_mode, 'task_mode');
    if (!TASK_MODES.has(authority.task_mode)) config(`unknown task_mode: ${authority.task_mode}`);
  }

  for (const key of ['repository', 'task_id', 'branch', 'task_file', 'evidence_file']) {
    if (authority[key] != null) requireString(authority[key], key);
  }

  if (authority.workspace_policy != null) {
    requireString(authority.workspace_policy, 'workspace_policy');
    if (!WORKSPACE_POLICIES.has(authority.workspace_policy)) {
      config(`unknown workspace_policy: ${authority.workspace_policy}`);
    }
  }

  if (authority.required_evidence != null &&
      (typeof authority.required_evidence !== 'object' || Array.isArray(authority.required_evidence))) {
    config('required_evidence must be an object');
  }

  requireExactShaWhenDeclared(authority.baseline_ref, 'baseline_ref');
  requireExactShaWhenDeclared(authority.authority_anchor_ref, 'authority_anchor_ref');

  if (!MUTATING_STATES.has(authority.state)) return authority;

  for (const key of [
    'task_mode', 'repository', 'task_id', 'branch', 'task_file', 'evidence_file',
    'workspace_policy', 'baseline_ref', 'authority_anchor_ref'
  ]) {
    requireString(authority[key], key);
  }

  if (!authority.required_evidence || Object.keys(authority.required_evidence).length === 0) {
    config('required_evidence must be a non-empty object');
  }
  return authority;
}

function commitsTouching(root, range, relativePath) {
  const output = runGit(root, ['rev-list', range, '--', relativePath]).stdout;
  return output ? output.split(/\r?\n/).filter(Boolean) : [];
}

export function validateActivation(root, authority) {
  if (!MUTATING_STATES.has(authority.state)) return null;

  try {
    resolveExactCommit(root, authority.baseline_ref);
    resolveExactCommit(root, authority.authority_anchor_ref);
  } catch (error) {
    gate('BLOCKED_AUTHORITY', error.message);
  }

  const head = getHeadSha(root);
  if (!isAncestor(root, authority.baseline_ref, authority.authority_anchor_ref)) {
    gate('BLOCKED_ACTIVATION', 'baseline is not an ancestor of authority anchor');
  }
  if (!isAncestor(root, authority.authority_anchor_ref, head)) {
    gate('BLOCKED_ACTIVATION', 'authority anchor is not an ancestor of HEAD');
  }

  const range = `${authority.authority_anchor_ref}..HEAD`;
  const nextTaskCommits = commitsTouching(root, range, 'docs/governance/NEXT_TASK.md');
  if (nextTaskCommits.length !== 1) {
    gate('BLOCKED_ACTIVATION', `expected exactly one NEXT_TASK transition, got ${nextTaskCommits.length}`);
  }
  const taskCommits = commitsTouching(root, range, authority.task_file);
  if (taskCommits.length !== 1 || taskCommits[0] !== nextTaskCommits[0]) {
    gate('BLOCKED_ACTIVATION', 'active task contract must change exactly once at the authority transition');
  }

  const transition = nextTaskCommits[0];
  const parentLine = runGit(root, ['rev-list', '--parents', '-n', '1', transition]).stdout.split(/\s+/);
  if (parentLine.length !== 2 || parentLine[1].toLowerCase() !== authority.authority_anchor_ref.toLowerCase()) {
    gate('BLOCKED_ACTIVATION', 'authority transition must be one direct single-parent child of authority anchor');
  }

  const paths = changedPaths(root, authority.authority_anchor_ref, transition);
  const expected = ['docs/governance/NEXT_TASK.md', authority.task_file].sort();
  if (paths.length !== 2 || paths[0] !== expected[0] || paths[1] !== expected[1]) {
    gate('BLOCKED_ACTIVATION', `authority transition changed unexpected paths: ${paths.join(', ')}`);
  }
  return transition.toLowerCase();
}

export function inspectAuthority(cwd = process.cwd()) {
  let root;
  let authority = null;
  let head = null;
  let currentBranch = null;
  let treeState = null;
  let liveMain = null;
  let transition = null;
  let repositoryStatus = 'NOT_APPLICABLE';
  let liveMainStatus = 'NOT_APPLICABLE';

  try {
    root = findRepoRoot(cwd);
    authority = readAuthority(root);
    validateAuthorityShape(authority);

    head = getHeadSha(root);
    currentBranch = getCurrentBranch(root);
    treeState = getStatusPorcelain(root) === '' ? 'clean' : 'dirty';

    if (authority.repository != null) {
      const actualRepo = normalizeRepositoryUrl(getOriginUrl(root));
      const expectedRepo = normalizeRepositoryUrl(authority.repository);
      if (actualRepo.toLowerCase() !== expectedRepo.toLowerCase()) {
        repositoryStatus = 'BLOCKED';
        gate('BLOCKED_REPOSITORY', `origin ${actualRepo} does not match authorized ${expectedRepo}`);
      }
      repositoryStatus = 'PASS';
    }

    if (MUTATING_STATES.has(authority.state)) {
      if (currentBranch !== authority.branch) {
        gate('BLOCKED_BRANCH', `current branch ${currentBranch} does not match authorized ${authority.branch}`);
      }
      transition = validateActivation(root, authority);
    }

    if (authority.baseline_ref != null) {
      try {
        resolveExactCommit(root, authority.baseline_ref);
      } catch (error) {
        gate('BLOCKED_AUTHORITY', error.message);
      }
      liveMain = resolveLiveMain(root);
      if (liveMain !== authority.baseline_ref.toLowerCase()) {
        liveMainStatus = 'BLOCKED';
        gate('BLOCKED_LIVE_MAIN_DRIFT', `live origin/main drifted: ${liveMain} != ${authority.baseline_ref}`);
      }
      liveMainStatus = 'PASS';
    }

    return {
      gate_status: 'PASS',
      repository_status: repositoryStatus,
      live_main_status: liveMainStatus,
      authority_state: authority.state,
      mutation_authority: authority.state === 'IMPLEMENT' ? 'IMPLEMENT' : authority.state === 'SPIKE' ? 'SPIKE' : 'NONE',
      repository: authority.repository ?? null,
      task_id: authority.task_id ?? null,
      baseline_ref: authority.baseline_ref ?? null,
      authority_anchor_ref: authority.authority_anchor_ref ?? null,
      authority_transition_ref: transition,
      branch: authority.branch ?? null,
      current_branch: currentBranch,
      head_sha: head,
      tree_state: treeState,
      live_main_sha: liveMain,
      workspace_policy: authority.workspace_policy ?? null,
      next_mechanical: MUTATING_STATES.has(authority.state) ? 'verify-candidate' : null,
      allowed_paths: authority.allowed_paths ?? [],
      forbidden_paths: authority.forbidden_paths ?? [],
      task_file: authority.task_file ?? null,
      evidence_file: authority.evidence_file ?? null,
      authority,
      error: null
    };
  } catch (error) {
    return {
      gate_status: error.gateStatus ?? 'BLOCKED_AUTHORITY',
      repository_status: repositoryStatus,
      live_main_status: liveMainStatus,
      authority_state: authority?.state ?? null,
      mutation_authority: 'NONE',
      repository: authority?.repository ?? null,
      task_id: authority?.task_id ?? null,
      baseline_ref: authority?.baseline_ref ?? null,
      authority_anchor_ref: authority?.authority_anchor_ref ?? null,
      authority_transition_ref: null,
      branch: authority?.branch ?? null,
      current_branch: currentBranch,
      head_sha: head,
      tree_state: treeState,
      live_main_sha: liveMain,
      workspace_policy: authority?.workspace_policy ?? null,
      next_mechanical: null,
      allowed_paths: authority?.allowed_paths ?? [],
      forbidden_paths: authority?.forbidden_paths ?? [],
      task_file: authority?.task_file ?? null,
      evidence_file: authority?.evidence_file ?? null,
      authority,
      error: error.message
    };
  }
}
