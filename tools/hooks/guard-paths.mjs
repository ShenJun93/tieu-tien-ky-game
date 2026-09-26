#!/usr/bin/env node
// PreToolUse hook for Edit/Write/MultiEdit/NotebookEdit.
// Blocks text edits that corrupt Unity projects or leak generated/secret files.
import { pathToFileURL } from 'node:url';
import { block, readHookInput, toRepoRelative } from './lib.mjs';

// Unity serializes these as YAML with fileID/GUID cross-references; agents must
// change them through the Editor (MCP/CLI) or an editor script, never as text.
const UNITY_YAML = /\.(unity|prefab|asset|mat|anim|controller|overridecontroller|physicmaterial|physicsmaterial2d|mask|playable|signal|lighting|spriteatlas|spriteatlasv2|terrainlayer|brush|guiskin|fontsettings|rendertexture|cubemap|flare|mixer|preset|shadervariants)$/i;
const GENERATED_DIRS = /^(library|temp|logs|usersettings|builds|obj)\//i;
const SECRETS = /\.(keystore|jks|p12|pem)$/i;

export function checkPath(relPath) {
  const p = relPath.replace(/\\/g, '/');
  const name = p.split('/').pop() || '';
  if (GENERATED_DIRS.test(p)) {
    return `Blocked: ${p} is Unity-generated or build output. Do not write there (AGENTS.md rule 5).`;
  }
  if (SECRETS.test(name)) {
    return `Blocked: ${p} looks like a signing key or certificate. Never write or commit keys.`;
  }
  if (name.toLowerCase().endsWith('.meta')) {
    return `Blocked: ${p} is a Unity .meta file. Create/move assets in the Editor or via AssetDatabase so GUIDs stay valid (AGENTS.md rule 3).`;
  }
  if (UNITY_YAML.test(name)) {
    return `Blocked: ${p} is a Unity YAML asset. Change it through the Unity Editor (MCP/CLI) or an editor script, not as text (AGENTS.md rule 2).`;
  }
  return null;
}

function main() {
  const input = readHookInput();
  const toolInput = input.tool_input || {};
  const filePath = toolInput.file_path || toolInput.notebook_path;
  if (!filePath) return;
  const reason = checkPath(toRepoRelative(filePath, process.env.CLAUDE_PROJECT_DIR || input.cwd));
  if (reason) block(reason);
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) main();
