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

function git(root, args, options = {}) {
  return execFileSync('git', args, { cwd: root, encoding: 'utf8', ...options }).trim();
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

function authorityJson(baseline, anchor, repository, overrides = {}) {
  const base = {
    state: 'IMPLEMENT',
    task_mode: 'SLICE',
    repository,
    task_id: 'T1',
    branch: 'feat/test-task',
    baseline_ref: baseline,
    authority_anchor_ref: anchor,
    workspace_policy: 'EXISTING_AUTHORIZED_WORKTREE',
    task_file: 'docs/tasks/TASK.md',
    evidence_file: 'docs/evidence/TASK_REPORT.md',
    allowed_paths: ['Assets/', 'docs/evidence/'],
    forbidden_paths: ['docs/governance/NEXT_TASK.md', 'docs/tasks/TASK.md', 'scripts/hooks/', '.agents/', 'AGENTS.md'],
    required_evidence: { automated_tests: 'PASS' },
    stop_condition: 'READY_FOR_REVIEW'
  };
  return JSON.stringify({ ...base, ...overrides }, null, 2);
}

function writeAuthority(root, baseline, anchor, repository, overrides = {}) {
  write(root, 'docs/governance/NEXT_TASK.md', `# NEXT TASK\n\n\`\`\`json\n${authorityJson(baseline, anchor, repository, overrides)}\n\`\`\`\n`);
}

function writeEvidence(root, value) {
  write(root, 'docs/evidence/TASK_REPORT.md', `# evidence\n\n\`\`\`json\n${JSON.stringify(value, null, 2)}\n\`\`\`\n`);
}

function makeFixture(overrides = {}, fixtureOptions = {}) {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-hooks-work-'));
  const remote = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-hooks-remote-'));
  execFileSync('git', ['init', '--bare', '-q', remote]);

  git(root, ['init', '-q']);
  git(root, ['config', 'user.email', 'hooks@test.local']);
  git(root, ['config', 'user.name', 'Hook Test']);

  for (const name of hookNames) {
    write(root, `scripts/hooks/${name}`, fs.readFileSync(path.join(sourceRoot, 'scripts/hooks', name), 'utf8'));
  }

  writeEvidence(root, { verdict: 'PASS', automated_tests: 'UNSET' });
  write(root, 'docs/governance/NEXT_TASK.md', '# inactive baseline\n');
  commitAll(root, 'baseline');
  const baseline = git(root, ['rev-parse', 'HEAD']);
  const anchor = baseline;

  git(root, ['remote', 'add', 'origin', remote]);
  git(root, ['push', '-q', 'origin', `${baseline}:refs/heads/main`]);
  git(root, ['branch', '-M', overrides.branch ?? 'feat/test-task']);

  write(root, 'docs/tasks/TASK.md', '# test task\n');
  writeAuthority(root, baseline, anchor, remote, overrides);
  if (fixtureOptions.activationExtraPath) {
    write(root, fixtureOptions.activationExtraPath, fixtureOptions.activationExtraContent ?? 'folded writer payload\n');
  }
  commitAll(root, 'activate task');
  const activation = git(root, ['rev-parse', 'HEAD']);

  return { root, remote, baseline, anchor, activation };
}

function advanceRemoteMain(fixture) {
  const tree = git(fixture.root, ['rev-parse', `${fixture.baseline}^{tree}`]);
  const advanced = git(fixture.root, ['commit-tree', tree, '-p', fixture.baseline], { input: 'advance main\n' });
  git(fixture.root, ['push', '-q', 'origin', `${advanced}:refs/heads/main`]);
  return advanced;
}

const NON_MUTATING_STATES = ['PAUSED', 'DISCOVERY', 'REVIEW', 'HUMAN_GATE', 'CLOSED'];

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

test('scope-gate hard-blocks NEXT_TASK even if accidentally allowed', () => {
  const { root } = makeFixture({ allowed_paths: ['Assets/', 'docs/evidence/', 'docs/governance/NEXT_TASK.md'] });
  const result = invoke(root, 'scope-gate.mjs', ['docs/governance/NEXT_TASK.md']);
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /writer-locked control-plane path/);
});

test('scope-gate hard-blocks active task contract even if accidentally allowed', () => {
  const { root } = makeFixture({ allowed_paths: ['Assets/', 'docs/evidence/', 'docs/tasks/TASK.md'] });
  const result = invoke(root, 'scope-gate.mjs', ['docs/tasks/TASK.md']);
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /writer-locked control-plane path/);
});

test('pre-task passes IMPLEMENT with locked authority, exact baseline and live main', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'pre-task.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /authority_transition:/);
  assert.match(result.stdout, /live_main:/);
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

test('pre-task requires immutable 40-character authority anchor', () => {
  const { root } = makeFixture({ authority_anchor_ref: 'HEAD~1' });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /authority_anchor_ref/);
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
  const { root } = makeFixture();
  git(root, ['checkout', '-q', '--orphan', 'unrelated']);
  write(root, 'other.txt', 'x\n');
  commitAll(root, 'unrelated');
  git(root, ['branch', '-M', 'feat/test-task']);
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /does not contain baseline|cannot resolve/);
});

test('pre-task blocks a second NEXT_TASK mutation after activation', () => {
  const fixture = makeFixture();
  write(fixture.root, 'docs/governance/NEXT_TASK.md', fs.readFileSync(path.join(fixture.root, 'docs/governance/NEXT_TASK.md'), 'utf8') + '\n<!-- writer edit -->\n');
  commitAll(fixture.root, 'attempt authority self-edit');
  const result = invoke(fixture.root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /exactly one NEXT_TASK transition/);
});

test('pre-task blocks active task contract mutation after activation', () => {
  const fixture = makeFixture();
  write(fixture.root, 'docs/tasks/TASK.md', '# changed by writer\n');
  commitAll(fixture.root, 'attempt contract self-edit');
  const result = invoke(fixture.root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /task contract/);
});

test('pre-task blocks when live origin/main advances after authorization', () => {
  const fixture = makeFixture();
  advanceRemoteMain(fixture);
  const result = invoke(fixture.root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /live origin\/main drifted/);
});

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

test('pre-finish rejects unsatisfied declared evidence', () => {
  const { root } = makeFixture();
  const result = invoke(root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /required evidence not satisfied/);
});

test('pre-finish passes governance-only evidence and ignores activation control-plane diff', () => {
  const fixture = makeFixture({
    task_mode: 'SPEC',
    required_evidence: {
      governance_hook_tests: 'PASS',
      scope_diff: 'PASS'
    }
  });
  writeEvidence(fixture.root, {
    verdict: 'PASS',
    governance_hook_tests: 'PASS',
    scope_diff: 'PASS'
  });
  write(fixture.root, 'docs/evidence/notes.md', 'verified\n');
  commitAll(fixture.root, 'complete governance task');
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /writer changed files checked:/);
  assert.doesNotMatch(result.stdout, /android/i);
});

test('pre-finish blocks when a required evidence key is missing', () => {
  const fixture = makeFixture({ required_evidence: { automated_tests: 'PASS', scope_diff: 'PASS' } });
  writeEvidence(fixture.root, { verdict: 'PASS', automated_tests: 'PASS' });
  commitAll(fixture.root, 'missing evidence');
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /scope_diff -> missing/);
});

test('pre-finish blocks when declared evidence has the wrong value', () => {
  const fixture = makeFixture();
  writeEvidence(fixture.root, { verdict: 'PASS', automated_tests: 'FAIL' });
  commitAll(fixture.root, 'wrong evidence');
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /got "FAIL", expected "PASS"/);
});

test('pre-finish rejects committed writer file outside task scope', () => {
  const fixture = makeFixture();
  writeEvidence(fixture.root, { verdict: 'PASS', automated_tests: 'PASS' });
  write(fixture.root, 'UNAUTHORIZED.md', 'bad\n');
  commitAll(fixture.root, 'bad scope');
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /committed diff violates task scope/);
});

test('pre-finish blocks a writer change to NEXT_TASK after activation', () => {
  const fixture = makeFixture();
  write(fixture.root, 'docs/governance/NEXT_TASK.md', fs.readFileSync(path.join(fixture.root, 'docs/governance/NEXT_TASK.md'), 'utf8') + '\n<!-- writer edit -->\n');
  commitAll(fixture.root, 'authority self-edit');
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /exactly one NEXT_TASK transition/);
});

test('pre-finish blocks a writer change to active task contract after activation', () => {
  const fixture = makeFixture();
  write(fixture.root, 'docs/tasks/TASK.md', '# writer changed contract\n');
  commitAll(fixture.root, 'contract self-edit');
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /task contract changed/);
});

test('pre-finish blocks when live origin/main advances before completion', () => {
  const fixture = makeFixture();
  writeEvidence(fixture.root, { verdict: 'PASS', automated_tests: 'PASS' });
  commitAll(fixture.root, 'complete task');
  advanceRemoteMain(fixture);
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /live origin\/main drifted/);
});

test('pre-finish rejects evidence verdict FAIL even if required keys match', () => {
  const fixture = makeFixture();
  writeEvidence(fixture.root, { verdict: 'FAIL', automated_tests: 'PASS' });
  commitAll(fixture.root, 'failed verdict');
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /verdict is FAIL/);
});

test('pre-finish blocks SPIKE from claiming implementation completion', () => {
  const fixture = makeFixture({
    state: 'SPIKE', task_mode: 'SPIKE', spike_bounded: true,
    allowed_paths: ['docs/evidence/'], required_evidence: { spike_result: 'RECORDED' }
  });
  writeEvidence(fixture.root, { verdict: 'PASS', spike_result: 'RECORDED' });
  commitAll(fixture.root, 'spike evidence');
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /only IMPLEMENT may/);
});

test('pre-task blocks an activation commit containing an extra forbidden file', () => {
  const fixture = makeFixture({}, {
    activationExtraPath: 'UNAUTHORIZED.md',
    activationExtraContent: 'must not be folded into activation\n'
  });
  const result = invoke(fixture.root, 'pre-task.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /authority-transition commit must change exactly/);
  assert.match(result.stderr, /UNAUTHORIZED\.md/);
});

test('pre-finish blocks rewritten or squashed activation containing writer payload', () => {
  const fixture = makeFixture({}, {
    activationExtraPath: 'Assets/FoldedWriterPayload.cs',
    activationExtraContent: '// payload folded into replacement activation\n'
  });
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /authority-transition commit must change exactly/);
  assert.match(result.stderr, /Assets\/FoldedWriterPayload\.cs/);
});

test('pre-finish accepts one aggregate evidence file containing every required key', () => {
  const required_evidence = {
    activation_exact_content_tests: 'PASS',
    evidence_contract_aggregate_tests: 'PASS',
    scope_diff: 'PASS'
  };
  const fixture = makeFixture({ required_evidence });
  writeEvidence(fixture.root, { verdict: 'PASS', ...required_evidence });
  commitAll(fixture.root, 'aggregate evidence complete');
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /activation_exact_content_tests, evidence_contract_aggregate_tests, scope_diff/);
});

test('review skill defers to active review contract verdict enum with a fallback only when undeclared', () => {
  const skill = fs.readFileSync(path.join(sourceRoot, '.agents/skills/review-task/SKILL.md'), 'utf8');
  assert.match(skill, /verdict enum declared by the \*\*active review contract\*\*/i);
  assert.match(skill, /If the active review contract does not declare a verdict enum, use the fallback default/i);
  assert.match(skill, /Do not invent a second competing taxonomy/i);
});
