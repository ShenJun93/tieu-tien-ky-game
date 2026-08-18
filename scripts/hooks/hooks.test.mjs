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

function authorityJson(overrides = {}) {
  const base = {
    state: 'IMPLEMENT',
    task_id: 'T1',
    branch: 'feat/p0a-local-microfun-spike',
    task_file: 'docs/tasks/TASK.md',
    evidence_file: 'docs/evidence/P0A_EVIDENCE_REPORT.md',
    baseline_ref: 'refs/remotes/origin/main',
    allowed_paths: ['Assets/', 'docs/evidence/'],
    forbidden_paths: ['docs/governance/', 'scripts/hooks/', '.agents/', 'AGENTS.md']
  };
  return JSON.stringify({ ...base, ...overrides }, null, 2);
}

function makeFixture(overrides = {}) {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-hooks-'));
  git(root, ['init', '-q']);
  git(root, ['config', 'user.email', 'hooks@test.local']);
  git(root, ['config', 'user.name', 'Hook Test']);

  for (const name of hookNames) {
    write(root, `scripts/hooks/${name}`, fs.readFileSync(path.join(sourceRoot, 'scripts/hooks', name), 'utf8'));
  }

  write(root, 'docs/governance/NEXT_TASK.md', `# NEXT TASK\n\n\`\`\`json\n${authorityJson(overrides)}\n\`\`\`\n`);
  write(root, 'docs/tasks/TASK.md', '# test task\n');
  write(root, 'docs/evidence/P0A_EVIDENCE_REPORT.md', `# evidence\n\n\`\`\`json\n{\n  "verdict": "UNSET",\n  "android_build": "UNSET",\n  "android_install_run": "UNSET",\n  "automated_tests": "UNSET",\n  "human_playtest": "UNSET"\n}\n\`\`\`\n`);

  commitAll(root, 'baseline');
  const baseline = git(root, ['rev-parse', 'HEAD']);
  git(root, ['update-ref', 'refs/remotes/origin/main', baseline]);
  const branch = overrides.branch ?? 'feat/p0a-local-microfun-spike';
  git(root, ['branch', '-M', branch]);
  return { root, baseline };
}

const NON_MUTATING_STATES = ['PAUSED', 'DISCOVERY', 'REVIEW', 'HUMAN_GATE', 'CLOSED'];
const UNKNOWN_STATES = ['NOT_A_REAL_STATE', undefined];

// --- scope-gate: basic path hygiene (state-independent, preserved) ---

test('scope-gate allows a normal allowed path', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'scope-gate.mjs', ['Assets/_Project/Core/Foo.cs']);
  assert.equal(result.status, 0, result.stderr);
});

test('scope-gate blocks traversal escaping an allowed directory', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'scope-gate.mjs', ['Assets/../docs/governance/NEXT_TASK.md']);
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
});

test('scope-gate blocks Windows absolute paths', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'scope-gate.mjs', ['C:\\temp\\Foo.cs']);
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
});

// --- pre-task: state semantics ---

test('pre-task passes for IMPLEMENT with clean exact branch and current baseline ancestor', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'pre-task.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /baseline:/);
});

test('pre-task blocks a task branch that does not contain current main baseline', () => {
  const { root } = makeFixture();
  git(root, ['checkout', '-q', '--orphan', 'other-main']);
  write(root, 'other.txt', 'x\n');
  commitAll(root, 'other main');
  const other = git(root, ['rev-parse', 'HEAD']);
  git(root, ['checkout', '-q', 'feat/p0a-local-microfun-spike']);
  git(root, ['update-ref', 'refs/remotes/origin/main', other]);
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
});

test('pre-task passes for SPIKE with a valid explicit bounded scope', () => {
  const { root } = makeFixture({ state: 'SPIKE', spike_bounded: true, allowed_paths: ['docs/evidence/spike/'] });
  const result = invoke(root, 'pre-task.mjs');
  assert.equal(result.status, 0, result.stderr);
});

test('pre-task blocks SPIKE that does not declare an explicit bounded scope', () => {
  const { root } = makeFixture({ state: 'SPIKE' });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
});

for (const state of NON_MUTATING_STATES) {
  test(`pre-task blocks state ${state}`, () => {
    const { root } = makeFixture({ state });
    const result = invoke(root, 'pre-task.mjs');
    assert.notEqual(result.status, 0, `unexpected pass for state ${state}: ${result.stdout}`);
  });
}

for (const state of UNKNOWN_STATES) {
  test(`pre-task fails closed for unknown state ${state}`, () => {
    const { root } = makeFixture({ state });
    const result = invoke(root, 'pre-task.mjs');
    assert.notEqual(result.status, 0, `unexpected pass for state ${state}: ${result.stdout}`);
  });
}

// --- scope-gate: state semantics ---

for (const state of NON_MUTATING_STATES) {
  test(`scope-gate blocks mutation when state is ${state}`, () => {
    const { root } = makeFixture({ state });
    const result = invoke(root, 'scope-gate.mjs', ['Assets/_Project/Core/Foo.cs']);
    assert.notEqual(result.status, 0, `unexpected pass for state ${state}: ${result.stdout}`);
  });
}

test('scope-gate allows SPIKE to mutate within its bounded allowed path', () => {
  const { root } = makeFixture({ state: 'SPIKE', spike_bounded: true, allowed_paths: ['docs/evidence/spike/'] });
  const result = invoke(root, 'scope-gate.mjs', ['docs/evidence/spike/notes.md']);
  assert.equal(result.status, 0, result.stderr);
});

test('scope-gate blocks SPIKE mutation outside its bounded allowed path', () => {
  const { root } = makeFixture({ state: 'SPIKE', spike_bounded: true, allowed_paths: ['docs/evidence/spike/'] });
  const result = invoke(root, 'scope-gate.mjs', ['Assets/_Project/Core/Foo.cs']);
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
});

test('scope-gate allows IMPLEMENT to mutate within its allowed path', () => {
  const { root } = makeFixture({ state: 'IMPLEMENT' });
  const result = invoke(root, 'scope-gate.mjs', ['Assets/_Project/Core/Foo.cs']);
  assert.equal(result.status, 0, result.stderr);
});

// --- pre-finish: state semantics ---

test('pre-finish rejects UNSET machine-readable evidence', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
});

test('pre-finish rejects committed files outside task scope', () => {
  const { root } = makeFixture();
  write(root, 'docs/governance/UNAUTHORIZED.md', 'bad\n');
  write(root, 'docs/evidence/P0A_EVIDENCE_REPORT.md', `# evidence\n\n\`\`\`json\n{"verdict":"PASS","android_build":"PASS","android_install_run":"PASS","automated_tests":"PASS","human_playtest":"RECORDED"}\n\`\`\`\n`);
  commitAll(root, 'bad scope');
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, /committed diff violates task scope/);
});

test('pre-finish passes allowed diff with explicit PASS evidence for IMPLEMENT (existing product evidence semantics preserved)', () => {
  const { root } = makeFixture();
  write(root, 'Assets/_Project/Core/Foo.cs', 'class Foo {}\n');
  write(root, 'docs/evidence/P0A_EVIDENCE_REPORT.md', `# evidence\n\n\`\`\`json\n{"verdict":"PASS","android_build":"PASS","android_install_run":"PASS","automated_tests":"PASS","human_playtest":"RECORDED"}\n\`\`\`\n`);
  commitAll(root, 'good scope');
  const result = invoke(root, 'pre-finish.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /PRE-FINISH PASS/);
});

test('pre-finish blocks SPIKE from ever claiming production/implementation completion', () => {
  const { root } = makeFixture({ state: 'SPIKE', spike_bounded: true, allowed_paths: ['docs/evidence/spike/'] });
  write(root, 'docs/evidence/spike/notes.md', 'spike notes\n');
  write(root, 'docs/evidence/P0A_EVIDENCE_REPORT.md', `# evidence\n\n\`\`\`json\n{"verdict":"PASS","android_build":"PASS","android_install_run":"PASS","automated_tests":"PASS","human_playtest":"RECORDED"}\n\`\`\`\n`);
  commitAll(root, 'spike attempt finish');
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
});

for (const state of NON_MUTATING_STATES) {
  test(`pre-finish blocks state ${state} from claiming implementation completion`, () => {
    const { root } = makeFixture({ state });
    const result = invoke(root, 'pre-finish.mjs');
    assert.notEqual(result.status, 0, `unexpected pass for state ${state}: ${result.stdout}`);
  });
}
