import test from 'node:test';
import assert from 'node:assert/strict';
import { checkPath } from './guard-paths.mjs';
import { checkCommand } from './guard-bash.mjs';
import { toRepoRelative } from './lib.mjs';

test('allows code and docs', () => {
  for (const p of ['Assets/_Project/Gameplay/Hero.cs', 'docs/GAME.md', 'tools/ci/check-meta.mjs', 'Assets/_Project/Shaders/Toon.shader']) {
    assert.equal(checkPath(p), null, p);
  }
});

test('blocks Unity YAML assets, meta files, generated dirs and keys', () => {
  for (const p of [
    'Assets/_Project/Scenes/Arena.unity',
    'Assets/_Project/Prefabs/Hero.prefab',
    'Assets/_Project/Data/Move.asset',
    'ProjectSettings/ProjectSettings.asset',
    'Assets/_Project/Materials/Water.mat',
    'Assets/_Project/Gameplay/Hero.cs.meta',
    'Library/ArtifactDB',
    'Temp/x.txt',
    'Builds/Android/TieuTienKy-Dev-abc1234.apk',
    'user.keystore',
  ]) {
    assert.notEqual(checkPath(p), null, p);
  }
});

test('toRepoRelative strips the project root on Windows and POSIX paths', () => {
  assert.equal(toRepoRelative('E:\\Game\\TTK\\Assets\\A.prefab', 'E:\\Game\\TTK'), 'Assets/A.prefab');
  assert.equal(toRepoRelative('/home/u/ttk/docs/GAME.md', '/home/u/ttk/'), 'docs/GAME.md');
  assert.equal(toRepoRelative('./Temp/x', ''), 'Temp/x');
  assert.equal(toRepoRelative('/e/Game/TTK/Library/x', 'E:/Game/TTK'), 'Library/x');
  assert.equal(toRepoRelative('E:\\Game\\TTK\\Library\\x', '/e/Game/TTK'), 'Library/x');
});

test('blocks pushes to main and merges', () => {
  assert.ok(checkCommand('git push origin main', 'feat/x'));
  assert.ok(checkCommand('git push origin HEAD:main', 'feat/x'));
  assert.ok(checkCommand('git push -f origin +main', 'feat/x'));
  assert.ok(checkCommand('git -C repo push origin refs/heads/main', 'feat/x'));
  assert.ok(checkCommand('git push', 'main'));
  assert.ok(checkCommand('cd repo && gh pr merge 71 --squash', 'feat/x'));
});

test('blocks commits on main but allows them on branches', () => {
  assert.ok(checkCommand('git add -A && git commit -m x', 'main'));
  assert.equal(checkCommand('git add -A && git commit -m x', 'chore/r0-2'), null);
});

test('allows normal branch work', () => {
  assert.equal(checkCommand('git push -u origin chore/r0-2-unity-safety', 'chore/r0-2-unity-safety'), null);
  assert.equal(checkCommand('git push', 'chore/r0-2-unity-safety'), null);
  assert.equal(checkCommand('git pull && git log --oneline -3', 'main'), null);
  assert.equal(checkCommand('gh pr create --fill', 'feat/x'), null);
  assert.equal(checkCommand('git switch main && git pull', 'feat/x'), null);
});

// Regression: the session starts in the main checkout but the command works
// in a worktree on a feature branch (false positive seen in slice R0.3).
const branches = { 'e:/repo': 'main', 'e:/wt/r03': 'feat/r0-3-urp' };
const branchAt = (dir) => branches[dir.replace(/\\/g, '/').toLowerCase()] ?? 'unknown';

test('uses the branch of the directory after cd or git -C', () => {
  assert.equal(checkCommand('cd /e/wt/r03 && git add -A && git commit -m x', branchAt, 'e:/repo'), null);
  assert.equal(checkCommand('git -C e:/wt/r03 commit -m x', branchAt, 'e:/repo'), null);
  assert.ok(checkCommand('git commit -m x', branchAt, 'e:/repo'));
  assert.ok(checkCommand('cd /e/wt/r03 && cd /e/repo && git commit -m x', branchAt, 'e:/repo'));
});

test('follows a branch switch earlier in the same command', () => {
  assert.equal(checkCommand('git switch -c chore/fix && git commit -m x', 'main'), null);
  assert.equal(checkCommand('git checkout -b chore/fix && git commit -m x', 'main'), null);
  assert.ok(checkCommand('git switch main && git commit -m x', 'feat/x'));
});

test('ignores heredoc bodies such as commit messages', () => {
  const cmd = "git commit -F - <<'EOF'\nfix: never git push origin main\ngh pr merge is Director-only\nEOF";
  assert.equal(checkCommand(cmd, 'feat/x'), null);
});
