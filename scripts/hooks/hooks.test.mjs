import test from 'node:test';
import assert from 'node:assert/strict';
import crypto from 'node:crypto';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { execFileSync, spawnSync } from 'node:child_process';

const here = path.dirname(fileURLToPath(import.meta.url));
const sourceRoot = path.resolve(here, '../..');
const hookNames = ['pre-task.mjs', 'scope-gate.mjs', 'pre-finish.mjs'];
const optionalHookNames = ['human-gate-preflight.mjs'];

function git(root, args, options = {}) {
  const nullGitConfig = process.platform === 'win32' ? 'NUL' : '/dev/null';
  return execFileSync('git', ['-c', 'core.autocrlf=false', ...args], {
    cwd: root,
    encoding: 'utf8',
    ...options,
    env: { ...process.env, GIT_CONFIG_GLOBAL: nullGitConfig, ...(options.env ?? {}) }
  }).trim();
}

function invoke(root, script, args = [], options = {}) {
  const nullGitConfig = process.platform === 'win32' ? 'NUL' : '/dev/null';
  return spawnSync(process.execPath, [`scripts/hooks/${script}`, ...args], {
    cwd: root,
    encoding: 'utf8',
    env: { ...process.env, GIT_CONFIG_GLOBAL: nullGitConfig, ...(options.env ?? {}) }
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

function sha256File(root, relative) {
  return crypto.createHash('sha256').update(fs.readFileSync(path.join(root, relative))).digest('hex');
}

function makeFixture(overrides = {}, fixtureOptions = {}) {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-hooks-work-'));
  const remote = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-hooks-remote-'));
  execFileSync('git', ['init', '--bare', '-q', remote]);

  git(root, ['init', '-q']);
  git(root, ['config', 'user.email', 'hooks@test.local']);
  git(root, ['config', 'user.name', 'Hook Test']);
  const fixtureExcludes = path.join(root, '.git', 'empty-excludes');
  fs.writeFileSync(fixtureExcludes, '');
  git(root, ['config', 'core.excludesfile', fixtureExcludes]);
  git(root, ['config', 'core.autocrlf', 'false']);

  for (const name of hookNames) {
    write(root, `scripts/hooks/${name}`, fs.readFileSync(path.join(sourceRoot, 'scripts/hooks', name), 'utf8'));
  }
  for (const name of optionalHookNames) {
    const source = path.join(sourceRoot, 'scripts/hooks', name);
    if (fs.existsSync(source)) write(root, `scripts/hooks/${name}`, fs.readFileSync(source, 'utf8'));
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
const PRODUCT_GATE = {
  required: true,
  player_promise: 'First 30 seconds feel like authored TTK combat',
  human_question: 'Does this feel materially different from the prototype?',
  artifact_required: true,
  representative_dimensions: ['combat', 'presentation', 'mobile_controls'],
  placeholder_policy: 'NO_UNDECLARED_PLACEHOLDERS',
  target_device_required: true
};

const PRODUCT_GATE_EVIDENCE = {
  automated_tests: 'PASS',
  acceptance_artifact_representative: 'PASS',
  placeholder_inventory: 'RECORDED',
  cross_discipline_coverage: 'PASS',
  target_device_readiness: 'PASS',
  human_gate_question_answerable: 'PASS',
  human_gate_preflight: 'PASS'
};

test('pre-task blocks incomplete required product_gate contract', () => {
  const { root } = makeFixture({
    product_gate: { required: true, human_question: 'Is it fun?' },
    required_evidence: PRODUCT_GATE_EVIDENCE
  });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, /product_gate/);
});

test('pre-task blocks product_gate when mandatory preflight evidence expectations are omitted', () => {
  const { root } = makeFixture({ product_gate: PRODUCT_GATE, required_evidence: { automated_tests: 'PASS' } });
  const result = invoke(root, 'pre-task.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, /acceptance_artifact_representative|product_gate/);
});

test('pre-task accepts a complete product_gate contract with mandatory preflight expectations', () => {
  const { root } = makeFixture({ product_gate: PRODUCT_GATE, required_evidence: PRODUCT_GATE_EVIDENCE });
  const result = invoke(root, 'pre-task.mjs');
  assert.equal(result.status, 0, result.stderr);
});

test('human-gate-preflight fails closed when representative evidence is not PASS', () => {
  const fixture = makeFixture({ product_gate: PRODUCT_GATE, required_evidence: PRODUCT_GATE_EVIDENCE });
  write(fixture.root, 'Builds/Test-future.apk', 'artifact');
  writeEvidence(fixture.root, {
    verdict: 'PASS',
    ...PRODUCT_GATE_EVIDENCE,
    acceptance_artifact_representative: 'FAIL',
    acceptance_artifact_path: 'Builds/Test-future.apk',
    acceptance_artifact_sha256: sha256File(fixture.root, 'Builds/Test-future.apk'),
    acceptance_artifact_source_sha: git(fixture.root, ['rev-parse', 'HEAD'])
  });
  const result = invoke(fixture.root, 'human-gate-preflight.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, /acceptance_artifact_representative/);
});

test('human-gate-preflight blocks an artifact made before later player-runtime mutation', () => {
  const fixture = makeFixture({ product_gate: PRODUCT_GATE, required_evidence: PRODUCT_GATE_EVIDENCE });
  write(fixture.root, 'Assets/GameplayChangedAfterArtifact.cs', 'runtime mutation\\n');
  commitAll(fixture.root, 'runtime changed after artifact source');
  write(fixture.root, 'Builds/Test-future.apk', 'artifact');
  writeEvidence(fixture.root, {
    verdict: 'PASS',
    ...PRODUCT_GATE_EVIDENCE,
    acceptance_artifact_path: 'Builds/Test-future.apk',
    acceptance_artifact_sha256: sha256File(fixture.root, 'Builds/Test-future.apk'),
    acceptance_artifact_source_sha: fixture.activation
  });
  const result = invoke(fixture.root, 'human-gate-preflight.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, /player-runtime mutation|stale artifact/);
});

test('human-gate-preflight blocks dirty player-runtime mutation after artifact source', () => {
  const fixture = makeFixture({ product_gate: PRODUCT_GATE, required_evidence: PRODUCT_GATE_EVIDENCE });
  write(fixture.root, 'Builds/Test-future.apk', 'artifact');
  writeEvidence(fixture.root, {
    verdict: 'PASS',
    ...PRODUCT_GATE_EVIDENCE,
    acceptance_artifact_path: 'Builds/Test-future.apk',
    acceptance_artifact_sha256: sha256File(fixture.root, 'Builds/Test-future.apk'),
    acceptance_artifact_source_sha: git(fixture.root, ['rev-parse', 'HEAD'])
  });
  write(fixture.root, 'Assets/DirtyAfterArtifact.cs', 'dirty runtime mutation\n');
  const result = invoke(fixture.root, 'human-gate-preflight.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, /dirty player-runtime mutation|stale artifact/);
});

test('human-gate-preflight blocks artifact hash mismatch', () => {
  const fixture = makeFixture({ product_gate: PRODUCT_GATE, required_evidence: PRODUCT_GATE_EVIDENCE });
  write(fixture.root, 'Builds/Test-future.apk', 'artifact');
  writeEvidence(fixture.root, {
    verdict: 'PASS',
    ...PRODUCT_GATE_EVIDENCE,
    acceptance_artifact_path: 'Builds/Test-future.apk',
    acceptance_artifact_sha256: '0000000000000000000000000000000000000000000000000000000000000000',
    acceptance_artifact_source_sha: git(fixture.root, ['rev-parse', 'HEAD'])
  });
  const result = invoke(fixture.root, 'human-gate-preflight.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, /SHA-256|hash/);
});


test('human-gate-preflight passes only when contract, evidence and exact artifact binding are ready', () => {
  const fixture = makeFixture({ product_gate: PRODUCT_GATE, required_evidence: PRODUCT_GATE_EVIDENCE });
  write(fixture.root, 'Builds/Test-future.apk', 'artifact');
  writeEvidence(fixture.root, {
    verdict: 'PASS',
    ...PRODUCT_GATE_EVIDENCE,
    acceptance_artifact_path: 'Builds/Test-future.apk',
    acceptance_artifact_sha256: sha256File(fixture.root, 'Builds/Test-future.apk'),
    acceptance_artifact_source_sha: git(fixture.root, ['rev-parse', 'HEAD'])
  });
  const result = invoke(fixture.root, 'human-gate-preflight.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /HUMAN-GATE PREFLIGHT PASS/);
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

function makeMultiParentActivationFixture(overrides = {}) {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-hooks-merge-work-'));
  const remote = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-hooks-merge-remote-'));
  execFileSync('git', ['init', '--bare', '-q', remote]);

  git(root, ['init', '-q']);
  git(root, ['config', 'user.email', 'hooks@test.local']);
  git(root, ['config', 'user.name', 'Hook Test']);
  const fixtureExcludes = path.join(root, '.git', 'empty-excludes');
  fs.writeFileSync(fixtureExcludes, '');
  git(root, ['config', 'core.excludesfile', fixtureExcludes]);
  git(root, ['config', 'core.autocrlf', 'false']);

  for (const name of hookNames) {
    write(root, `scripts/hooks/${name}`, fs.readFileSync(path.join(sourceRoot, 'scripts/hooks', name), 'utf8'));
  }

  writeEvidence(root, { verdict: 'PASS', automated_tests: 'UNSET' });
  write(root, 'docs/governance/NEXT_TASK.md', '# inactive baseline\n');
  commitAll(root, 'baseline');
  const baseline = git(root, ['rev-parse', 'HEAD']);
  const anchor = baseline;
  const branch = overrides.branch ?? 'feat/test-task';

  git(root, ['remote', 'add', 'origin', remote]);
  git(root, ['push', '-q', 'origin', `${baseline}:refs/heads/main`]);
  git(root, ['branch', '-M', branch]);

  git(root, ['checkout', '-q', '-b', 'activation-side-payload']);
  write(root, 'UNAUTHORIZED.md', 'payload inherited from second parent\n');
  commitAll(root, 'side-parent payload');
  const side = git(root, ['rev-parse', 'HEAD']);

  git(root, ['checkout', '-q', branch]);
  git(root, ['merge', '--no-ff', '--no-commit', 'activation-side-payload']);
  write(root, 'docs/tasks/TASK.md', '# test task\n');
  writeAuthority(root, baseline, anchor, remote, overrides);
  commitAll(root, 'merge activation');
  const activation = git(root, ['rev-parse', 'HEAD']);

  return { root, remote, baseline, anchor, side, activation };
}

test('pre-task blocks multi-parent activation whose merge-aware show hides second-parent payload', () => {
  const fixture = makeMultiParentActivationFixture();
  const parentTokens = git(fixture.root, ['rev-list', '--parents', '-n', '1', fixture.activation]).split(/\s+/);
  assert.equal(parentTokens.length, 3, `expected transition + two parents, got ${parentTokens.join(' ')}`);

  const mergeAwareShow = git(fixture.root, ['show', '--pretty=format:', '--name-only', fixture.activation]);
  assert.doesNotMatch(mergeAwareShow, /UNAUTHORIZED\.md/);

  const anchorDiff = git(fixture.root, ['diff', '--name-only', '--no-renames', fixture.anchor, fixture.activation, '--']);
  assert.match(anchorDiff, /UNAUTHORIZED\.md/);

  const result = invoke(fixture.root, 'pre-task.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, /single-parent direct child/);
});

test('pre-finish blocks multi-parent activation whose first parent is the authority anchor', () => {
  const fixture = makeMultiParentActivationFixture();
  const result = invoke(fixture.root, 'pre-finish.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, /single-parent direct child/);
});

const candidateGateSource = path.join(sourceRoot, 'scripts/hooks/candidate-gate.mjs');

function candidateAuthority(baseline, anchor, overrides = {}) {
  const reviewRequired = overrides.independent_review_required ?? true;
  return {
    repository: 'ShenJun93/tieu-tien-ky-game',
    state: 'IMPLEMENT',
    task_mode: 'SPEC',
    task_id: 'T1',
    branch: 'feat/candidate-gate-test',
    baseline_ref: baseline,
    authority_anchor_ref: anchor,
    workspace_policy: 'EXISTING_AUTHORIZED_WORKTREE',
    task_file: 'docs/tasks/T1.md',
    evidence_file: 'docs/evidence/T1_REPORT.md',
    allowed_paths: ['src/', 'docs/evidence/T1_REPORT.md'],
    forbidden_paths: ['docs/governance/NEXT_TASK.md', 'docs/tasks/T1.md'],
    required_evidence: { automated_tests: 'PASS' },
    independent_review_required: reviewRequired,
    review_receipt_file: reviewRequired ? 'docs/reviews/T1.review.json' : null,
    acceptable_review_verdicts: reviewRequired ? ['PASS', 'PASS_WITH_REMEDIATION'] : [],
    stop_condition: 'READY_FOR_REVIEW',
    ...overrides
  };
}

function writeCandidateAuthority(root, authority) {
  write(root, 'docs/governance/NEXT_TASK.md', `# NEXT TASK\n\n\`\`\`json\n${JSON.stringify(authority, null, 2)}\n\`\`\`\n`);
}

function receiptPayload(fixture, overrides = {}) {
  return {
    schema_version: 1,
    task_id: 'T1',
    baseline_sha: fixture.baseline,
    reviewed_candidate_sha: fixture.candidate,
    verdict: 'PASS',
    blocking_findings: [],
    blocking_finding_count: 0,
    reviewer_identifier: 'fixture-independent-reviewer',
    review_completed_at: '2026-08-27T00:00:00Z',
    review_completion_mode: 'INDEPENDENT_READ_ONLY',
    ...overrides
  };
}

function makeCandidateGateFixture(options = {}) {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-candidate-gate-'));
  git(root, ['init', '-q']);
  git(root, ['config', 'user.email', 'candidate-gate@test.local']);
  git(root, ['config', 'user.name', 'Candidate Gate Test']);
  const fixtureExcludes = path.join(root, '.git', 'empty-excludes');
  fs.writeFileSync(fixtureExcludes, '');
  git(root, ['config', 'core.excludesfile', fixtureExcludes]);
  git(root, ['config', 'core.autocrlf', 'false']);

  if (options.gateInBaseline !== false && fs.existsSync(candidateGateSource)) {
    write(root, 'scripts/hooks/candidate-gate.mjs', fs.readFileSync(candidateGateSource, 'utf8'));
  }

  write(root, 'docs/governance/NEXT_TASK.md', '# inactive baseline\n');
  commitAll(root, 'baseline');
  const baseline = git(root, ['rev-parse', 'HEAD']);
  const anchor = baseline;
  git(root, ['branch', '-M', 'feat/candidate-gate-test']);

  const authority = candidateAuthority(baseline, anchor, {
    independent_review_required: options.reviewRequired ?? true,
    ...(options.authorityOverrides ?? {})
  });
  write(root, 'docs/tasks/T1.md', '# TASK T1\n');
  writeCandidateAuthority(root, authority);
  commitAll(root, 'activate task');
  const activation = git(root, ['rev-parse', 'HEAD']);

  write(root, 'src/implementation.txt', 'candidate implementation\n');
  write(root, 'docs/evidence/T1_REPORT.md', '# evidence\n\n```json\n{"verdict":"PASS","automated_tests":"PASS"}\n```\n');
  for (const [relative, content] of Object.entries(options.candidateExtras ?? {})) write(root, relative, content);
  if (options.gateInBaseline === false && fs.existsSync(candidateGateSource)) {
    write(root, 'scripts/hooks/candidate-gate.mjs', fs.readFileSync(candidateGateSource, 'utf8'));
  }
  commitAll(root, 'implementation candidate');
  const candidate = git(root, ['rev-parse', 'HEAD']);
  const fixture = { root, baseline, anchor, activation, candidate, authority };

  if ((options.reviewRequired ?? true) && options.withReceipt !== false) {
    write(root, authority.review_receipt_file, `${JSON.stringify(receiptPayload(fixture, options.receiptOverrides), null, 2)}\n`);
    for (const [relative, content] of Object.entries(options.receiptCommitExtras ?? {})) write(root, relative, content);
    commitAll(root, 'persist independent review receipt');
    fixture.receiptCommit = git(root, ['rev-parse', 'HEAD']);
  }

  return fixture;
}

function terminalAuthority(fixture, overrides = {}) {
  const reviewRequired = fixture.authority.independent_review_required;
  return {
    repository: 'ShenJun93/tieu-tien-ky-game',
    state: 'DISCOVERY',
    task_id: null,
    branch: null,
    baseline_ref: null,
    task_file: null,
    evidence_file: null,
    allowed_paths: [],
    forbidden_paths: [],
    last_terminal_closeout: {
      schema_version: 1,
      task_id: 'T1',
      task_file: 'docs/tasks/T1.md',
      baseline_sha: fixture.baseline,
      authority_anchor_sha: fixture.anchor,
      activation_sha: fixture.activation,
      independent_review_required: reviewRequired,
      review_receipt_file: reviewRequired ? 'docs/reviews/T1.review.json' : null,
      reviewed_candidate_sha: reviewRequired ? fixture.candidate : null,
      ...overrides
    },
    stop_condition: 'HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY'
  };
}

function closeCandidateGateFixture(fixture, overrides = {}) {
  writeCandidateAuthority(fixture.root, terminalAuthority(fixture, overrides));
  commitAll(fixture.root, 'terminal closeout');
  fixture.closeout = git(fixture.root, ['rev-parse', 'HEAD']);
  return fixture;
}

function assertCandidateGateBlocked(fixture, pattern) {
  const result = invoke(fixture.root, 'candidate-gate.mjs');
  assert.notEqual(result.status, 0, `unexpected pass: ${result.stdout}`);
  assert.match(result.stderr, pattern);
}

test('candidate gate passes a valid exact review receipt at the receipt head', () => {
  const fixture = makeCandidateGateFixture();
  const result = invoke(fixture.root, 'candidate-gate.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /CANDIDATE GATE PASS/);
});

test('candidate gate passes deterministic NEXT_TASK-only terminal closeout at final DISCOVERY head', () => {
  const fixture = closeCandidateGateFixture(makeCandidateGateFixture());
  const result = invoke(fixture.root, 'candidate-gate.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /final DISCOVERY/);
});

test('candidate gate preserves active low-risk flow without demanding a receipt', () => {
  const fixture = makeCandidateGateFixture({ reviewRequired: false, withReceipt: false });
  const result = invoke(fixture.root, 'candidate-gate.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /review not required/);
});

test('candidate gate passes final low-risk closeout without a receipt', () => {
  const fixture = closeCandidateGateFixture(
    makeCandidateGateFixture({ reviewRequired: false, withReceipt: false })
  );
  const result = invoke(fixture.root, 'candidate-gate.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /final DISCOVERY low-risk closeout/);
});

test('candidate gate blocks a missing receipt when independent review is required', () => {
  const fixture = makeCandidateGateFixture({ withReceipt: false });
  assertCandidateGateBlocked(fixture, /required review receipt is missing/i);
});

test('candidate gate blocks malformed receipt JSON', () => {
  const fixture = makeCandidateGateFixture();
  write(fixture.root, fixture.authority.review_receipt_file, '{not-json\n');
  git(fixture.root, ['add', fixture.authority.review_receipt_file]);
  git(fixture.root, ['commit', '--amend', '--no-edit']);
  assertCandidateGateBlocked(fixture, /review receipt JSON is invalid/i);
});

test('candidate gate blocks malformed reviewed candidate SHA', () => {
  const fixture = makeCandidateGateFixture({ receiptOverrides: { reviewed_candidate_sha: 'abc123' } });
  assertCandidateGateBlocked(fixture, /reviewed_candidate_sha must be an exact 40-character commit SHA/i);
});

test('candidate gate blocks receipt candidate SHA that differs from the receipt commit parent', () => {
  const fixture = makeCandidateGateFixture({ receiptOverrides: { reviewed_candidate_sha: '0000000000000000000000000000000000000000' } });
  assertCandidateGateBlocked(fixture, /reviewed_candidate_sha does not equal the receipt commit parent/i);
});

test('candidate gate blocks receipt baseline mismatch', () => {
  const fixture = makeCandidateGateFixture({ receiptOverrides: { baseline_sha: '0000000000000000000000000000000000000000' } });
  assertCandidateGateBlocked(fixture, /receipt baseline_sha does not match authorized baseline/i);
});

test('candidate gate blocks receipt task mismatch', () => {
  const fixture = makeCandidateGateFixture({ receiptOverrides: { task_id: 'OTHER_TASK' } });
  assertCandidateGateBlocked(fixture, /receipt task_id does not match authorized task/i);
});

test('candidate gate blocks an unacceptable FAIL verdict', () => {
  const fixture = makeCandidateGateFixture({ receiptOverrides: { verdict: 'FAIL' } });
  assertCandidateGateBlocked(fixture, /receipt verdict is not acceptable/i);
});

test('candidate gate blocks remaining blocking findings', () => {
  const fixture = makeCandidateGateFixture({
    receiptOverrides: { blocking_findings: ['P1 unresolved'], blocking_finding_count: 1 }
  });
  assertCandidateGateBlocked(fixture, /blocking findings remain/i);
});

for (const [label, relative] of [
  ['implementation', 'src/post-review.txt'],
  ['evidence', 'docs/evidence/T1_REPORT.md'],
  ['active task contract', 'docs/tasks/T1.md'],
  ['unauthorized path', 'UNAUTHORIZED.md']
]) {
  test(`candidate gate blocks post-review ${label} mutation`, () => {
    const fixture = makeCandidateGateFixture();
    write(fixture.root, relative, `post-review ${label}\n`);
    commitAll(fixture.root, `post-review ${label}`);
    assertCandidateGateBlocked(fixture, /post-review commit sequence is unauthorized/i);
  });
}

test('candidate gate blocks a stale receipt reused after a later implementation commit', () => {
  const fixture = makeCandidateGateFixture();
  write(fixture.root, 'src/implementation.txt', 'new implementation candidate after receipt\n');
  commitAll(fixture.root, 'later implementation candidate');
  assertCandidateGateBlocked(fixture, /post-review commit sequence is unauthorized/i);
});

test('candidate gate blocks a receipt commit that also mutates evidence', () => {
  const fixture = makeCandidateGateFixture({
    receiptCommitExtras: { 'docs/evidence/T1_REPORT.md': '# evidence changed during receipt persistence\n' }
  });
  assertCandidateGateBlocked(fixture, /receipt-only commit must change exactly/i);
});

test('candidate gate blocks non-canonical receipt path metadata instead of discovering a receipt', () => {
  const fixture = makeCandidateGateFixture({
    authorityOverrides: { review_receipt_file: 'docs/reviews/something-else.json' }
  });
  assertCandidateGateBlocked(fixture, /review_receipt_file must equal canonical path/i);
});

test('candidate gate blocks a reviewed candidate outside the activated writer scope', () => {
  const fixture = makeCandidateGateFixture({
    candidateExtras: { 'UNAUTHORIZED-CANDIDATE.md': 'outside writer scope\n' }
  });
  assertCandidateGateBlocked(fixture, /reviewed candidate violates authorized writer scope/i);
});

test('candidate gate blocks terminal closeout claim that differs from the receipt', () => {
  const fixture = closeCandidateGateFixture(
    makeCandidateGateFixture(),
    { reviewed_candidate_sha: '0000000000000000000000000000000000000000' }
  );
  assertCandidateGateBlocked(fixture, /terminal reviewed_candidate_sha does not match receipt/i);
});

test('candidate gate blocks terminal closeout commit containing an unauthorized path', () => {
  const fixture = closeCandidateGateFixture(makeCandidateGateFixture());
  write(fixture.root, 'UNAUTHORIZED.md', 'folded into closeout\n');
  git(fixture.root, ['add', '.']);
  git(fixture.root, ['commit', '--amend', '--no-edit']);
  assertCandidateGateBlocked(fixture, /terminal closeout must change exactly/i);
});

test('candidate gate blocks final DISCOVERY metadata that fails to clear live authority fields', () => {
  const fixture = makeCandidateGateFixture();
  const invalid = terminalAuthority(fixture);
  invalid.branch = 'feat/still-authorized';
  writeCandidateAuthority(fixture.root, invalid);
  commitAll(fixture.root, 'invalid terminal closeout');
  assertCandidateGateBlocked(fixture, /terminal DISCOVERY metadata must clear branch/i);
});

test('candidate gate blocks final DISCOVERY without binding metadata once Candidate Gate exists in baseline', () => {
  const fixture = makeCandidateGateFixture({ reviewRequired: false, withReceipt: false });
  writeCandidateAuthority(fixture.root, {
    repository: 'ShenJun93/tieu-tien-ky-game',
    state: 'DISCOVERY',
    task_id: null,
    branch: null,
    baseline_ref: null,
    task_file: null,
    evidence_file: null,
    allowed_paths: [],
    forbidden_paths: [],
    stop_condition: 'HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY'
  });
  commitAll(fixture.root, 'terminal closeout missing binding');
  assertCandidateGateBlocked(fixture, /missing last_terminal_closeout review binding metadata/i);
});

test('candidate gate blocks low-risk metadata that names a review receipt', () => {
  const fixture = makeCandidateGateFixture({
    reviewRequired: false,
    withReceipt: false,
    authorityOverrides: {
      review_receipt_file: 'docs/reviews/T1.review.json',
      acceptable_review_verdicts: ['PASS']
    }
  });
  assertCandidateGateBlocked(fixture, /low-risk task must set review_receipt_file to null/i);
});

test('candidate gate blocks missing explicit independent review policy', () => {
  const fixture = makeCandidateGateFixture({ reviewRequired: false, withReceipt: false });
  const invalid = { ...fixture.authority };
  delete invalid.independent_review_required;
  writeCandidateAuthority(fixture.root, invalid);
  git(fixture.root, ['add', 'docs/governance/NEXT_TASK.md']);
  git(fixture.root, ['commit', '--amend', '--no-edit']);
  assertCandidateGateBlocked(fixture, /independent_review_required must be an explicit boolean/i);
});

test('candidate gate permits A2 bootstrap closeout only when the gate was absent from the PR baseline', () => {
  const fixture = makeCandidateGateFixture({ gateInBaseline: false, withReceipt: false });
  writeCandidateAuthority(fixture.root, {
    repository: 'ShenJun93/tieu-tien-ky-game',
    state: 'DISCOVERY',
    task_id: null,
    branch: null,
    baseline_ref: null,
    task_file: null,
    evidence_file: null,
    allowed_paths: [],
    forbidden_paths: [],
    stop_condition: 'HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY'
  });
  commitAll(fixture.root, 'current-canon A2 bootstrap closeout');
  const result = invoke(fixture.root, 'candidate-gate.mjs', [], {
    env: { CANDIDATE_GATE_BASE_SHA: fixture.baseline }
  });
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /A2 bootstrap/i);
});

test('candidate gate permits the active A2 candidate under pre-A2 canon only when absent from the PR baseline', () => {
  const fixture = makeCandidateGateFixture({
    gateInBaseline: false,
    reviewRequired: false,
    withReceipt: false,
    authorityOverrides: {
      task_id: 'TASK-TIEU-TIEN-KY-EXACT-REVIEW-BINDING-A2-001',
      task_file: 'docs/tasks/T1.md'
    }
  });
  const bootstrapAuthority = { ...fixture.authority };
  delete bootstrapAuthority.independent_review_required;
  delete bootstrapAuthority.review_receipt_file;
  delete bootstrapAuthority.acceptable_review_verdicts;
  writeCandidateAuthority(fixture.root, bootstrapAuthority);
  git(fixture.root, ['add', 'docs/governance/NEXT_TASK.md']);
  git(fixture.root, ['commit', '--amend', '--no-edit']);
  const result = invoke(fixture.root, 'candidate-gate.mjs', [], {
    env: { CANDIDATE_GATE_BASE_SHA: fixture.baseline }
  });
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /A2 bootstrap/i);
});

function readSource(relative) {
  return fs.readFileSync(path.join(sourceRoot, relative), 'utf8');
}

test('skill-pressure: vertical slice gate blocks known-confounded Human handoff', () => {
  const text = readSource('.agents/skills/ttk-vertical-slice-production-gate/SKILL.md');
  assert.match(text, /NO_UNDECLARED_PLACEHOLDERS/);
  assert.match(text, /learning build/i);
  assert.match(text, /acceptance artifact/i);
  assert.match(text, /human-gate-preflight\.mjs/);
  assert.match(text, /technical|compile|tests/i);
});

test('skill-pressure: player experience integration requires cross-discipline outcome chain', () => {
  const text = readSource('.agents/skills/ttk-player-experience-integration/SKILL.md');
  for (const term of ['GAMEPLAY', 'ANIMATION', 'REACTION', 'CAMERA', 'VFX', 'AUDIO', 'HAPTIC', 'UI', 'WORLD']) {
    assert.match(text, new RegExp(term, 'i'));
  }
  assert.match(text, /component exists/i);
});

test('skill-pressure: authored content skill rejects prototype constructors as production default', () => {
  const text = readSource('.agents/skills/ttk-unity-authored-content-pipeline/SKILL.md');
  assert.match(text, /Scenes\/Prefabs\/Animator\/Materials\/UI/i);
  assert.match(text, /GameObject\.CreatePrimitive/);
  assert.match(text, /procedural HUD/i);
  assert.match(text, /default production presentation strategy/i);
});

test('skill-pressure: art benchmarking extracts principles without copying IP expression', () => {
  const text = readSource('.agents/skills/ttk-art-target-reference-benchmarking/SKILL.md');
  assert.match(text, /quality bar/i);
  assert.match(text, /Extract principles, not assets/i);
  assert.match(text, /protected expression/i);
  assert.match(text, /reference similarity.*never the acceptance metric/is);
});

test('skill-pressure: enemy direction requires role telegraph counterplay and group pressure', () => {
  const text = readSource('.agents/skills/ttk-enemy-ai-encounter-direction/SKILL.md');
  for (const term of ['pressure role', 'telegraph', 'counterplay', 'group composition']) assert.match(text, new RegExp(term, 'i'));
  assert.match(text, /HP\/damage\/speed\/color/i);
});

test('skill-pressure: VFX direction protects attention hierarchy at representative density', () => {
  const text = readSource('.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md');
  assert.match(text, /visual priority|attention budget/i);
  assert.match(text, /representative encounter|combat density/i);
  assert.match(text, /target mobile device/i);
  assert.match(text, /more particles/i);
});

test('skill-pressure: mobile performance requires physical target-device frame and thermal evidence', () => {
  const text = readSource('.agents/skills/ttk-mobile-performance-budget/SKILL.md');
  assert.match(text, /frame-time|frame pacing/i);
  assert.match(text, /thermal/i);
  assert.match(text, /physical target devices/i);
  assert.match(text, /android_build=PASS.*performance PASS/i);
});

test('skill-pressure: playtest research rejects vague liking question as sole evidence', () => {
  const text = readSource('.agents/skills/ttk-playtest-user-research/SKILL.md');
  assert.match(text, /task-based observation/i);
  assert.match(text, /neutral follow-ups/i);
  assert.match(text, /Do you like it\?/i);
  assert.match(text, /unanswered playtest is not a PASS/i);
});

test('skill-pressure: onboarding accessibility covers new-player touch and sensory readability', () => {
  const text = readSource('.agents/skills/ttk-onboarding-accessibility/SKILL.md');
  assert.match(text, /first 30–60 seconds|first-session/i);
  assert.match(text, /safe areas/i);
  assert.match(text, /contrast/i);
  assert.match(text, /motion\/flash/i);
  assert.match(text, /operator.*already know/is);
});

test('skill-pressure: execution and Human gate cannot bypass representativeness preflight', () => {
  const execute = readSource('.agents/skills/execute-task/SKILL.md');
  const human = readSource('.agents/skills/ttk-human-product-gate/SKILL.md');
  assert.match(execute, /ttk-vertical-slice-production-gate/);
  assert.match(execute, /ttk-player-experience-integration/);
  assert.match(execute, /human-gate-preflight\.mjs/);
  assert.match(human, /before.*install\/launch\/handoff/is);
  assert.match(human, /known to be structurally confounded/i);
});

test('skill-pressure: reviewer treats non-representative green artifact as blocking', () => {
  const text = readSource('.agents/skills/review-task/SKILL.md');
  assert.match(text, /technically green.*structurally non-representative artifact.*blocking/is);
  assert.match(text, /TECHNICAL GATE/);
  assert.match(text, /REPRESENTATIVE PREFLIGHT/);
  assert.match(text, /HUMAN PRODUCT GATE/);
});