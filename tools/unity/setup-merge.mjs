#!/usr/bin/env node
// Register Unity's Smart Merge (UnityYAMLMerge) as the git merge driver named
// in .gitattributes. Writes only this clone's local git config.
import { execFileSync } from 'node:child_process';
import { existsSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { findEditor } from './test.mjs';

const editor = findEditor(process.cwd());
if (!editor) {
  console.error('setup-merge: Unity editor not found; set UNITY_EDITOR and retry.');
  process.exit(3);
}
const toolsDir = editor.endsWith('.app/Contents/MacOS/Unity')
  ? join(dirname(editor), '../Tools')
  : join(dirname(editor), 'Data/Tools');
const merge = ['UnityYAMLMerge.exe', 'UnityYAMLMerge'].map((n) => join(toolsDir, n)).find(existsSync);
if (!merge) {
  console.error(`setup-merge: UnityYAMLMerge not found under ${toolsDir}`);
  process.exit(1);
}
const driver = `"${merge.replace(/\\/g, '/')}" merge -p %O %B %A %A`;
execFileSync('git', ['config', 'merge.unityyamlmerge.name', 'Unity SmartMerge (UnityYAMLMerge)']);
execFileSync('git', ['config', 'merge.unityyamlmerge.driver', driver]);
execFileSync('git', ['config', 'merge.unityyamlmerge.recursive', 'binary']);
console.log('setup-merge: registered unityyamlmerge driver for this clone.');
