import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { execFileSync, spawnSync } from 'node:child_process';

const here = path.dirname(fileURLToPath(import.meta.url));
const sourceRoot = path.resolve(here, '../..');
const hookNames = ['pre-task.mjs', 'scope-gate.mjs', 'pre-finish.mjs'];

function git(root, args) {
  return execFileSync('git', args, { cwd: root, encoding: 'utf8' }).trim();
}

function invoke(root, script, args = []) {
  return spawnSync(process.execPath, [`scripts/hooks/${script}`, ...args], {
    cwd: root,
    encoding: 'utf8'
  });
}

function write(root, relative, content) {
  const target = path.join(root, relative);
  fs.mkdirSync(path.dirname(target), { recursive: true });
  fs.writeFileSync(target, content);
}

function commitAll(root, message) {
  git(root, ['add', '.']);
  git(root, ['commit', '-m', message]);
}

function authorityJson(baseline, overrides = {}) {
  const base = {
    state: 'IMPLEMENT',
    task_mode: 'SLICE',
    repository: 'ShenJun93/tieu-tien-ky-game',
    task_id: 'T1',
    branch: 'feat/test-task',
    baseline_ref: baseline,
    workspace_policy: 'EXISTING_AUTHORIZED_WORKTREE',
    task_file: 'docs/tasks/TASK.md',
    evidence_file: 'docs/evidence/TASK_REPORT.md',
    allowed_paths: ['Assets/', 'docs/evidence/', 'docs/governance/NEXT_TASK.md'],
    forbidden_paths: ['scripts/hooks/', '.agents/', 'AGENTS.md'],
    required_evidence: { automated_tests: 'PASS' },
    stop_condition: 'READY_FOR_REVIEW'
  };
  return JSON.stringify({ ...base, ...overrides }, null, 2);
}

function writeAuthority(root, baseline, overrides = {}) {
  write(root, 'docs/governance/NEXT_TASK.md', `# NEXT TASK\n\n\`\`\`json\n${authorityJson(baseline, overrides)}\n\`\`\`\n`);
}

function writeEvidence(root, value) {
  write(root, 'docs/evidence/TASK_REPORT.md', `# evidence\n\n\`\`\`json\n${JSON.stringify(value, null, 2)}\n\`\`\`\n`);
}

function makeFixture(overrides = {}) {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-hooks-'));
  git(root, ['init', '-q']);
  git(root, ['config', 'user.email', 'hooks@test.local']);
  git(root, ['config', 'user.name', 'Hook Test']);
  git(root, ['remote', 'add', 'origin', 'https://github.com/ShenJun93/tieu-tien-ky-game.git']);

  for (const name of hookNames) {
    write(root, `scripts/hooks/${name}`, fs.readFileSync(path.join(sourceRoot, 'scripts/hooks', name), 'utf8'));
  }

  write(root, 'docs/tasks/TASK.md', '# test task\n');
  writeEvidence(root, { verdict: 'UNSET', automated_tests: 'UNSET' });
  write(root, 'docs/governance/NEXT_TASK.md', '# inactive baseline\n');
  commitAll(root, 'baseline');
  const baseline = git(root, ['rev-parse', 'HEAD']);
  git(root, ['update-ref', 'refs/remotes/origin/main', baseline]);

  git(root, ['branch', '-M', overrides.branch ?? 'feat/test-task']);
  writeAuthority(root, baseline, overrides);
  commitAll(root, 'activate task');
  return { root, baseline };
}

const NON_MUTATING_STATES = ['PAUSED', 'DISCOVERY', 'REVIEW', 'HUMAN_GATE', 'CLOSED'];

// --- scope-gate: path hygiene ---

test('scope-gate allows a normal allowed path', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'scope-gate.mjs', ['Assets/_Project/Core/Foo.cs']);
  assert.equal(result.status, 0, result.stderr);
});

test('scope-gate blocks traversal escaping an allowed directory', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'scope-gate.mjs', ['Assets/../docs/secret.md']);
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
});

test('scope-gate blocks Windows absolute paths', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'scope-gate.mjs', ['C:\\temp\\Foo.cs']);
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
});

// --- pre-task: authority / identity semantics ---

test('pre-task passes IMPLEMENT with exact repository, branch, SHA baseline and evidence contract', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'pre-task.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /task_mode: SLICE/);
  assert.match(result.stdout, /workspace_policy: EXISTING_AUTHORIZED_WORKTREE/);
});

for (const state of NON_MUTATING_STATES) {
  test(`pre-task blocks state ${state}`, () => {
    const { root } = makeFixture({ state });
    const result = invoke(root, 'pre-task.mjs');
    assert.notEqual(result.status, 0, `unexpected pass for state ${state}: ${result.stdout}`);
  });
}

test('pre-task fails closed for unknown authority state', () => {
  const { root } = makeFixture({ state: 'NOT_REAL' });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
});

test('pre-task fails closed for unknown task_mode', () => {
  const { root } = makeFixture({ task_mode: 'MAGIC' });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /task_mode/);
});

test('pre-task blocks local execution for REMOTE_GITHUB_BRANCH policy', () => {
  const { root } = makeFixture({ workspace_policy: 'REMOTE_GITHUB_BRANCH' });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /REMOTE_GITHUB_BRANCH/);
});

test('pre-task blocks repository identity mismatch', () => {
  const { root } = makeFixture({ repository: 'Other/Repo' });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /does not match authorized/);
});

test('pre-task requires immutable 40-character SHA baseline', () => {
  const { root } = makeFixture({ baseline_ref: 'refs/remotes/origin/main' });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /40-character commit SHA/);
});

test('pre-task requires non-empty required_evidence', () => {
  const { root } = makeFixture({ required_evidence: {} });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /required_evidence/);
});

test('pre-task passes a bounded SPIKE with task_mode SPIKE', () => {
  const { root } = makeFixture({
    state: 'SPIKE',
    task_mode: 'SPIKE',
    spike_bounded: true,
    allowed_paths: ['docs/evidence/'],
    required_evidence: { spike_result: 'RECORDED' }
  });
  const result = invoke(root, 'pre-task.mjs');
  assert.equal(result.status, 0, result.stderr);
});

test('pre-task blocks SPIKE state with a non-SPIKE task mode', () => {
  const { root } = makeFixture({ state: 'SPIKE', task_mode: 'SLICE', spike_bounded: true });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
});

test('pre-task blocks branch that does not contain authorized baseline', () => {
  const { root, baseline } = makeFixture();
  git(root, ['checkout', '-q', '--orphan', 'unrelated']);
  write(root, 'other.txt', 'x\n');
  commitAll(root, 'unrelated');
  git(root, ['branch', '-M', 'feat/test-task']);
  writeAuthority(root, baseline);
  commitAll(root, 'authority on unrelated history');
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /does not contain baseline|cannot resolve merge-base/);
});

// --- scope-gate: authority state semantics ---

for (const state of NON_MUTATING_STATES) {
  test(`scope-gate blocks mutation when state is ${state}`, () => {
    const { root } = makeFixture({ state });
    const result = invoke(root, 'scope-gate.mjs', ['Assets/_Project/Core/Foo.cs']);
    assert.notEqual(result.status, 0);
  });
}

test('scope-gate allows SPIKE mutation within bounded path', () => {
  const { root } = makeFixture({
    state: 'SPIKE', task_mode: 'SPIKE', spike_bounded: true,
    allowed_paths: ['docs/evidence/'], required_evidence: { spike_result: 'RECORDED' }
  });
  const result = invoke(root, 'scope-gate.mjs', ['docs/evidence/spike.md']);
  assert.equal(result.status, 0, result.stderr);
});

// --- pre-finish: generic task-declared evidence ---

test('pre-finish rejects unsatisfied declared evidence', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /required evidence not satisfied/);
});

test('pre-finish passes governance-only evidence without Android/Human fields', () => {
  const { root, baseline } = makeFixture();
  writeAuthority(root, baseline, {
    task_mode: 'SPEC',
    required_evidence: {
      governance_hook_tests: 'PASS',
      scope_diff: 'PASS'
    }
  });
  writeEvidence(root, {
    verdict: 'PASS',
    governance_hook_tests: 'PASS',
    scope_diff: 'PASS'
  });
  write(root, 'docs/evidence/notes.md', 'verified\n');
  commitAll(root, 'complete governance task');
  const result = invoke(root, 'pre-finish.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.doesNotMatch(result.stdout, /android/i);
});

test('pre-finish blocks when a required evidence key is missing', () => {
  const { root, baseline } = makeFixture();
  writeAuthority(root, baseline, { required_evidence: { automated_tests: 'PASS', scope_diff: 'PASS' } });
  writeEvidence(root, { verdict: 'PASS', automated_tests: 'PASS' });
  commitAll(root, 'missing evidence');
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /scope_diff -> missing/);
});

test('pre-finish blocks when declared evidence has the wrong value', () => {
  const { root } = makeFixture();
  writeEvidence(root, { verdict: 'PASS', automated_tests: 'FAIL' });
  commitAll(root, 'wrong evidence');
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /got "FAIL", expected "PASS"/);
});

test('pre-finish rejects committed files outside task scope', () => {
  const { root } = makeFixture();
  writeEvidence(root, { verdict: 'PASS', automated_tests: 'PASS' });
  write(root, 'UNAUTHORIZED.md', 'bad\n');
  commitAll(root, 'bad scope');
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /committed diff violates task scope/);
});

test('pre-finish rejects evidence verdict FAIL even if required keys match', () => {
  const { root } = makeFixture();
  writeEvidence(root, { verdict: 'FAIL', automated_tests: 'PASS' });
  commitAll(root, 'failed verdict');
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /verdict is FAIL/);
});

test('pre-finish blocks SPIKE from claiming implementation completion', () => {
  const { root } = makeFixture({
    state: 'SPIKE', task_mode: 'SPIKE', spike_bounded: true,
    allowed_paths: ['docs/evidence/'], required_evidence: { spike_result: 'RECORDED' }
  });
  writeEvidence(root, { verdict: 'PASS', spike_result: 'RECORDED' });
  commitAll(root, 'spike evidence');
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /only IMPLEMENT may/);
});
