import test from 'node:test';
import assert from 'node:assert/strict';
import { findMetaProblems, splitKnownDebt } from './check-meta.mjs';

test('passes when every file and folder has a .meta', () => {
  assert.deepEqual(findMetaProblems([
    'Assets/_Project.meta',
    'Assets/_Project/Hero.cs',
    'Assets/_Project/Hero.cs.meta',
  ]), []);
});

test('reports a file without .meta', () => {
  assert.deepEqual(findMetaProblems([
    'Assets/_Project.meta',
    'Assets/_Project/Hero.cs',
  ]), ['missing .meta: Assets/_Project/Hero.cs']);
});

test('reports a folder without .meta', () => {
  assert.deepEqual(findMetaProblems([
    'Assets/_Project/Hero.cs',
    'Assets/_Project/Hero.cs.meta',
  ]), ['missing .meta: Assets/_Project']);
});

test('reports an orphaned .meta', () => {
  assert.deepEqual(findMetaProblems([
    'Assets/_Project.meta',
    'Assets/_Project/Gone.cs.meta',
  ]), ['orphaned .meta: Assets/_Project/Gone.cs.meta']);
});

test('ignores paths outside Assets/', () => {
  assert.deepEqual(findMetaProblems(['docs/GAME.md', 'Packages/manifest.json']), []);
});

test('ignores names Unity does not import (hidden or ending in ~)', () => {
  assert.deepEqual(findMetaProblems([
    'Assets/ThirdParty.meta',
    'Assets/ThirdParty/.gitkeep',
    'Assets/Samples~/Demo.cs',
  ]), []);
});

test('known debt warns instead of blocking', () => {
  const problems = ['missing .meta: Assets/A', 'missing .meta: Assets/B'];
  assert.deepEqual(splitKnownDebt(problems, ['missing .meta: Assets/A']), {
    blocking: ['missing .meta: Assets/B'],
    tolerated: ['missing .meta: Assets/A'],
  });
});
