import path from 'node:path';
import { getGitCommonDir, getGitDir, listWorktrees } from './git-state.mjs';

export function inspectWorkspace(root, authority) {
  const policy = authority?.workspace_policy ?? null;
  if (!policy) {
    return { gate_status: 'NOT_APPLICABLE', policy: null, linked_worktree: false, registered: false, reason: null };
  }

  try {
    const gitDir = path.resolve(getGitDir(root));
    const commonDir = path.resolve(getGitCommonDir(root));
    const linkedWorktree = gitDir !== commonDir;
    const registered = listWorktrees(root).some(w => path.resolve(w.path) === path.resolve(root));

    if (policy === 'ISOLATED_WORKTREE' && !linkedWorktree) {
      return {
        gate_status: 'BLOCKED_WORKSPACE',
        policy,
        linked_worktree: false,
        registered,
        reason: 'ISOLATED_WORKTREE requires a linked Git worktree'
      };
    }

    if (policy === 'ISOLATED_WORKTREE' && !registered) {
      return {
        gate_status: 'BLOCKED_WORKSPACE',
        policy,
        linked_worktree: true,
        registered: false,
        reason: 'current worktree is not registered'
      };
    }

    if (policy === 'EXISTING_AUTHORIZED_WORKTREE' || policy === 'REMOTE_GITHUB_BRANCH' || policy === 'ISOLATED_WORKTREE') {
      return {
        gate_status: 'PASS',
        policy,
        linked_worktree: linkedWorktree,
        registered,
        reason: null
      };
    }

    return { gate_status: 'BLOCKED_WORKSPACE', policy, linked_worktree: linkedWorktree, registered, reason: 'unknown workspace policy' };
  } catch (error) {
    return { gate_status: 'BLOCKED_WORKSPACE', policy, linked_worktree: false, registered: false, reason: error.message };
  }
}
