export function candidateChecks(context) {
  const checks = [
    {
      name: 'diff_check',
      argv: ['git', 'diff', '--check', `${context.baselineRef}..${context.candidateSha}`],
      timeout_ms: 300000
    }
  ];

  const writerPaths = context.writerPaths ?? [];
  if (writerPaths.some(p => p === '.gitignore' || p.startsWith('scripts/ao/'))) {
    checks.push({
      name: 'ao_tests',
      argv: [process.execPath, '--test', 'scripts/ao/ao.test.mjs'],
      timeout_ms: 600000
    });
  }

  if (writerPaths.some(p => p.startsWith('scripts/hooks/') || p.startsWith('docs/governance/') || p.startsWith('.agents/') || p === 'AGENTS.md')) {
    checks.push({
      name: 'governance_hook_tests',
      argv: [process.execPath, '--test', 'scripts/hooks/hooks.test.mjs'],
      timeout_ms: 600000
    });
  }

  return checks;
}
