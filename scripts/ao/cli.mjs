#!/usr/bin/env node
import { pathToFileURL } from 'node:url';
import { inspectAuthority } from './authority.mjs';
import { verifyCandidate } from './candidate-gate.mjs';
import { writeEvidence } from './evidence.mjs';
import { candidateChecks as defaultCandidateChecks } from './project-policy.mjs';
import { inspectWorkspace } from './workspace.mjs';
import { findRepoRoot } from './git-state.mjs';

export function parseArgs(argv) {
  if (!Array.isArray(argv) || argv.length === 0) throw new Error('unknown or missing command');
  const command = argv[0];
  if (!['inspect', 'verify-candidate'].includes(command)) throw new Error(`unknown command: ${command}`);

  let json = false;
  let candidate = null;
  for (let i = 1; i < argv.length; i += 1) {
    const arg = argv[i];
    if (arg === '--json') {
      if (json) throw new Error('duplicate --json');
      json = true;
      continue;
    }
    if (arg === '--candidate') {
      if (command !== 'verify-candidate') throw new Error('--candidate is only valid for verify-candidate');
      if (candidate !== null) throw new Error('duplicate candidate');
      candidate = argv[++i];
      if (!candidate) throw new Error('candidate value is required');
      continue;
    }
    throw new Error(`unexpected argument: ${arg}`);
  }
  if (command === 'verify-candidate' && !candidate) throw new Error('candidate is required');
  return { command, json, candidate };
}

function humanInspect(report) {
  const val = v => v == null ? 'NONE' : String(v);
  return [
    `AUTHORITY_STATE     ${val(report.authority_state)}`,
    `REPOSITORY          ${val(report.repository_status ?? 'NOT_APPLICABLE')}`,
    `LIVE_MAIN           ${val(report.live_main_status ?? 'NOT_APPLICABLE')}`,
    `MUTATION_AUTHORITY  ${val(report.mutation_authority)}`,
    `AO_MUTATION         NONE`,
    `NEXT_MECHANICAL     ${val(report.next_mechanical)}`
  ];
}

function humanCandidate(report) {
  return [
    `GATE_STATUS         ${report.gate_status}`,
    `CANDIDATE           ${report.candidate_sha ?? 'NONE'}`,
    `AO_MUTATION         NONE`
  ];
}

export function runCli(argv, dependencies = {}) {
  const writeLine = dependencies.writeLine ?? (line => console.log(line));
  const cwd = dependencies.cwd ?? process.cwd();
  const inspect = dependencies.inspectAuthority ?? inspectAuthority;
  const inspectWs = dependencies.inspectWorkspace ?? inspectWorkspace;
  const chooseChecks = dependencies.candidateChecks ?? defaultCandidateChecks;
  const verify = dependencies.verifyCandidate ?? verifyCandidate;
  const persistEvidence = dependencies.writeEvidence ?? writeEvidence;

  let parsed;
  try {
    parsed = parseArgs(argv);
  } catch (error) {
    writeLine(`INVALID_INVOCATION ${error.message}`);
    return 2;
  }

  let root;
  try {
    root = findRepoRoot(cwd);
  } catch (error) {
    writeLine(`BLOCKED_REPOSITORY ${error.message}`);
    return 1;
  }

  const authority = inspect(root);
  let report;

  if (parsed.command === 'inspect') {
    const workspace = authority.workspace_policy ? inspectWs(root, authority.authority ?? { workspace_policy: authority.workspace_policy }) : null;
    report = {
      schema_version: 1,
      command: 'inspect',
      repository: authority.repository,
      repository_status: authority.repository_status ?? 'NOT_APPLICABLE',
      authority_state: authority.authority_state,
      task_id: authority.task_id,
      baseline_ref: authority.baseline_ref,
      authority_anchor_ref: authority.authority_anchor_ref,
      branch: authority.branch,
      head_sha: authority.head_sha,
      tree_state: authority.tree_state,
      live_main_sha: authority.live_main_sha,
      live_main_status: authority.live_main_status ?? 'NOT_APPLICABLE',
      gate_status: authority.gate_status === 'PASS' && workspace?.gate_status === 'BLOCKED_WORKSPACE' ? 'BLOCKED_WORKSPACE' : authority.gate_status,
      candidate_sha: null,
      checks: [],
      mutation_authority: authority.mutation_authority,
      next_mechanical: authority.next_mechanical,
      workspace,
      ao_mutation: 'NONE',
      error: authority.error ?? workspace?.reason ?? null,
      recorded_at: new Date().toISOString()
    };
  } else {
    if (authority.gate_status !== 'PASS') {
      report = {
        schema_version: 1,
        command: 'verify-candidate',
        repository: authority.repository,
        repository_status: authority.repository_status ?? 'NOT_APPLICABLE',
        authority_state: authority.authority_state,
        task_id: authority.task_id,
        baseline_ref: authority.baseline_ref,
        authority_anchor_ref: authority.authority_anchor_ref,
        branch: authority.branch,
        head_sha: authority.head_sha,
        tree_state: authority.tree_state,
        live_main_sha: authority.live_main_sha,
        live_main_status: authority.live_main_status ?? 'NOT_APPLICABLE',
        gate_status: authority.gate_status,
        candidate_sha: parsed.candidate,
        checks: [],
        ao_mutation: 'NONE',
        error: authority.error,
        recorded_at: new Date().toISOString()
      };
    } else {
      let checks = [];
      if (authority.workspace_policy !== 'REMOTE_GITHUB_BRANCH') {
        const entry = verify({ root, candidateSha: parsed.candidate, authoritySnapshot: authority, checks: [] });
        if (entry.gate_status !== 'PASS') {
          report = {
            schema_version: 1,
            command: 'verify-candidate',
            repository: authority.repository,
            repository_status: authority.repository_status ?? 'NOT_APPLICABLE',
            authority_state: authority.authority_state,
            task_id: authority.task_id,
            baseline_ref: authority.baseline_ref,
            authority_anchor_ref: authority.authority_anchor_ref,
            branch: authority.branch,
            head_sha: authority.head_sha,
            tree_state: authority.tree_state,
            live_main_sha: authority.live_main_sha,
            live_main_status: authority.live_main_status ?? 'NOT_APPLICABLE',
            ...entry,
            ao_mutation: 'NONE',
            recorded_at: new Date().toISOString()
          };
        } else {
          checks = chooseChecks({
            baselineRef: authority.baseline_ref,
            candidateSha: parsed.candidate,
            writerPaths: entry.writer_paths
          });
          const verified = verify({ root, candidateSha: parsed.candidate, authoritySnapshot: authority, checks });
          report = {
            schema_version: 1,
            command: 'verify-candidate',
            repository: authority.repository,
            repository_status: authority.repository_status ?? 'NOT_APPLICABLE',
            authority_state: authority.authority_state,
            task_id: authority.task_id,
            baseline_ref: authority.baseline_ref,
            authority_anchor_ref: authority.authority_anchor_ref,
            branch: authority.branch,
            head_sha: authority.head_sha,
            tree_state: authority.tree_state,
            live_main_sha: authority.live_main_sha,
            live_main_status: authority.live_main_status ?? 'NOT_APPLICABLE',
            ...verified,
            ao_mutation: 'NONE',
            recorded_at: new Date().toISOString()
          };
        }
      } else {
        const verified = verify({ root, candidateSha: parsed.candidate, authoritySnapshot: authority, checks: [] });
        report = {
          schema_version: 1,
          command: 'verify-candidate',
          repository: authority.repository,
          repository_status: authority.repository_status ?? 'NOT_APPLICABLE',
          authority_state: authority.authority_state,
          task_id: authority.task_id,
          baseline_ref: authority.baseline_ref,
          authority_anchor_ref: authority.authority_anchor_ref,
          branch: authority.branch,
          head_sha: authority.head_sha,
          tree_state: authority.tree_state,
          live_main_sha: authority.live_main_sha,
          live_main_status: authority.live_main_status ?? 'NOT_APPLICABLE',
          ...verified,
          ao_mutation: 'NONE',
          recorded_at: new Date().toISOString()
        };
      }
    }
  }

  try {
    persistEvidence(root, report, { taskId: report.task_id ?? 'DISCOVERY' });
  } catch (error) {
    report = { ...report, gate_status: report.gate_status === 'PASS' ? 'BLOCKED_CHECK' : report.gate_status, error: report.error ?? `evidence: ${error.message}` };
  }

  if (parsed.json) {
    writeLine(JSON.stringify(report));
  } else {
    const lines = parsed.command === 'inspect' ? humanInspect(report) : humanCandidate(report);
    for (const line of lines) writeLine(line);
  }

  if (report.gate_status === 'PASS' || report.gate_status === 'NOT_APPLICABLE') return 0;
  if (report.gate_status === 'INVALID_INVOCATION') return 2;
  return 1;
}

const invoked = process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href;
if (invoked) {
  process.exitCode = runCli(process.argv.slice(2));
}
