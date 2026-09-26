#!/usr/bin/env node
// Stop hook: before an agent declares it is done, the cheap repository checks
// must pass. Unity compile/tests are run explicitly with tools/unity/test.mjs
// (they need several minutes and cannot run while the Editor holds the project).
import { spawnSync } from 'node:child_process';
import { join } from 'node:path';
import { block, readHookInput } from './lib.mjs';

const input = readHookInput();
if (input.stop_hook_active) process.exit(0); // already continuing after a block; avoid loops

const root = process.env.CLAUDE_PROJECT_DIR || input.cwd || process.cwd();
const result = spawnSync(process.execPath, [join(root, 'tools/ci/check-meta.mjs')], { cwd: root, encoding: 'utf8' });
if (result.status !== 0) {
  block(`Repository check failed. Fix it before finishing:\n${result.stderr || result.stdout}`);
}
