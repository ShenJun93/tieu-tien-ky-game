import test from 'node:test';
import assert from 'node:assert/strict';
import { parseResults } from './test.mjs';

test('parses a passing NUnit3 test-run header', () => {
  const xml = '<?xml version="1.0"?><test-run id="2" testcasecount="3" result="Passed" total="3" passed="3" failed="0" inconclusive="0" skipped="0"></test-run>';
  assert.deepEqual(parseResults(xml), { result: 'Passed', total: 3, passed: 3, failed: 0, skipped: 0, failures: [] });
});

test('lists failed test cases', () => {
  const xml = '<test-run result="Failed(Child)" total="2" passed="1" failed="1" skipped="0">'
    + '<test-case id="1" name="A" fullname="TTK.Tests.A" result="Passed" />'
    + '<test-case id="2" name="B" fullname="TTK.Tests.B" result="Failed" />'
    + '</test-run>';
  const r = parseResults(xml);
  assert.equal(r.failed, 1);
  assert.deepEqual(r.failures, ['TTK.Tests.B']);
});

test('returns null for non-results input', () => {
  assert.equal(parseResults('<html></html>'), null);
});
