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

function makeFixture() {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'ttk-hooks-'));
  git(root, ['init', '-q']);
  git(root, ['config', 'user.email', 'hooks@test.local']);
  git(root, ['config', 'user.name', 'Hook Test']);

  for (const name of hookNames) {
    write(root, `scripts/hooks/${name}`, fs.readFileSync(path.join(sourceRoot, 'scripts/hooks', name), 'utf8'));
  }

  write(root, 'docs/governance/NEXT_TASK.md', `# NEXT TASK\n\n\`\`\`json\n{\n  "status": "ACTIVE",\n  "task_id": "T1",\n  "branch": "feat/p0a-local-microfun-spike",\n  "task_file": "docs/tasks/TASK.md",\n  "evidence_file": "docs/evidence/P0A_EVIDENCE_REPORT.md",\n  "baseline_ref": "refs/remotes/origin/main",\n  "allowed_paths": ["Assets/", "docs/evidence/"],\n  "forbidden_paths": ["docs/governance/", "scripts/hooks/", ".agents/", "AGENTS.md"]\n}\n\`\`\`\n`);
  write(root, 'docs/tasks/TASK.md', '# test task\n');
  write(root, 'docs/evidence/P0A_EVIDENCE_REPORT.md', `# evidence\n\n\`\`\`json\n{\n  "verdict": "UNSET",\n  "android_build": "UNSET",\n  "android_install_run": "UNSET",\n  "automated_tests": "UNSET",\n  "human_playtest": "UNSET"\n}\n\`\`\`\n`);

  commitAll(root, 'baseline');
  const baseline = git(root, ['rev-parse', 'HEAD']);
  git(root, ['update-ref', 'refs/remotes/origin/main', baseline]);
  git(root, ['branch', '-M', 'feat/p0a-local-microfun-spike']);
  return { root, baseline };
}

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

test('pre-task passes with clean exact branch and current baseline ancestor', () => {
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

test('pre-finish passes allowed diff with explicit PASS evidence', () => {
  const { root } = makeFixture();
  write(root, 'Assets/_Project/Core/Foo.cs', 'class Foo {}\n');
  write(root, 'docs/evidence/P0A_EVIDENCE_REPORT.md', `# evidence\n\n\`\`\`json\n{"verdict":"PASS","android_build":"PASS","android_install_run":"PASS","automated_tests":"PASS","human_playtest":"RECORDED"}\n\`\`\`\n`);
  commitAll(root, 'good scope');
  const result = invoke(root, 'pre-finish.mjs');
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.stdout, /PRE-FINISH PASS/);
});
