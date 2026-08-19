import fs from 'node:fs';
import path from 'node:path';
import { spawnSync } from 'node:child_process';

const SENSITIVE_KEY = /(token|secret|password|credential|authorization|cookie|api[_-]?key)/i;

export function sanitizeStructured(value) {
  if (Array.isArray(value)) return value.map(sanitizeStructured);
  if (value && typeof value === 'object') {
    const result = {};
    for (const [key, child] of Object.entries(value)) {
      result[key] = SENSITIVE_KEY.test(key) ? '[REDACTED]' : sanitizeStructured(child);
    }
    return result;
  }
  return value;
}

export function evidenceRoot(root) {
  return path.join(root, '.local', 'ao');
}

function safeSegment(value, fallback) {
  const text = String(value ?? fallback).replace(/[^A-Za-z0-9._-]+/g, '-').replace(/^-+|-+$/g, '');
  return text || fallback;
}

function safeTimestamp(now) {
  return String(now).replace(/[:.]/g, '-').replace(/[^\dTZ-]/g, '-');
}

export function writeEvidence(root, report, options = {}) {
  const probe = path.join(root, '.local', 'ao', '.probe');
  const relativeProbe = path.relative(root, probe).replaceAll('\\', '/');
  const ignored = spawnSync('git', ['check-ignore', '--quiet', relativeProbe], {
    cwd: root,
    encoding: 'utf8'
  });
  if (ignored.status !== 0) {
    throw new Error('.local/ao/ must be ignored before AO evidence is written');
  }

  const now = options.now ?? new Date().toISOString();
  const taskId = safeSegment(options.taskId ?? report?.task_id ?? 'DISCOVERY', 'DISCOVERY');
  const dir = path.join(evidenceRoot(root), taskId, safeTimestamp(now));
  fs.mkdirSync(dir, { recursive: true });
  const target = path.join(dir, 'report.json');
  const sanitized = sanitizeStructured(report);
  fs.writeFileSync(target, `${JSON.stringify(sanitized, null, 2)}\n`, 'utf8');
  return target;
}
