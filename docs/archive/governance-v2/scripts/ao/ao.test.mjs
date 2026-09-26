import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { fileURLToPath, pathToFileURL } from 'node:url';
import { execFileSync } from 'node:child_process';

const here = path.dirname(fileURLToPath(import.meta.url));
const sourceRoot = path.resolve(here, '../..');
const git = (root, args, options={}) => execFileSync('git', args, {cwd:root, encoding:'utf8', ...options}).trim();
const write = (root, rel, value) => { const p=path.join(root,rel); fs.mkdirSync(path.dirname(p),{recursive:true}); fs.writeFileSync(p,value); };
const commit = (root, msg) => { git(root,['add','.']); git(root,['commit','-q','-m',msg]); return git(root,['rev-parse','HEAD']); };
const md = a => `# NEXT\n\n\`\`\`json\n${JSON.stringify(a,null,2)}\n\`\`\`\n`;
async function load(name){ try{return await import(`${pathToFileURL(path.join(here,name)).href}?${Math.random()}`);}catch{return {};} }

function repo(){
  const root=fs.mkdtempSync(path.join(os.tmpdir(),'ttk-ao-'));
  git(root,['init','-q']); git(root,['config','user.email','ao@test']); git(root,['config','user.name','AO']);
  write(root,'.gitignore',fs.readFileSync(path.join(sourceRoot,'.gitignore'),'utf8'));
  write(root,'README.md','x\n'); commit(root,'baseline'); return root;
}
function active(o={}){
  const root=repo(), remote=fs.mkdtempSync(path.join(os.tmpdir(),'ttk-ao-remote-'));
  execFileSync('git',['init','--bare','-q',remote]);
  const baseline=git(root,['rev-parse','HEAD']), branch=o.branch??'chore/ao-test';
  git(root,['remote','add','origin',remote]); git(root,['push','-q','origin',`${baseline}:refs/heads/main`]); git(root,['branch','-M',branch]);
  const task='docs/tasks/TASK.md';
  const a={state:o.state??'IMPLEMENT',task_mode:o.taskMode===undefined?'SPEC':o.taskMode,repository:o.repository===undefined?remote.replaceAll('\\','/'):o.repository,task_id:'T',branch,
    baseline_ref:o.badBaseline===undefined?baseline:o.badBaseline,authority_anchor_ref:o.anchor===undefined?baseline:o.anchor,workspace_policy:o.policy===undefined?'EXISTING_AUTHORIZED_WORKTREE':o.policy,
    task_file:task,evidence_file:'docs/evidence/E.md',allowed_paths:o.allowed??['src/'],forbidden_paths:o.forbidden??['blocked/'],
    required_evidence:{ao_tests:'PASS'},stop_condition:'REVIEW'};
  write(root,'docs/governance/NEXT_TASK.md',md(a)); write(root,task,'# task\n');
  if(o.extra) write(root,o.extra,'bad\n');
  const activation=commit(root,'activate');
  if(o.editTask){ write(root,task,'changed\n'); commit(root,'bad contract'); }
  if(o.editNext){ write(root,'docs/governance/NEXT_TASK.md',md({...a,stop_condition:'changed'})); commit(root,'bad next'); }
  let candidate=git(root,['rev-parse','HEAD']);
  if(o.writer!==false){ write(root,o.writerPath??'src/x.txt','ok\n'); candidate=commit(root,'writer'); }
  return {root,remote,baseline,branch,activation,candidate,a};
}
function advance(f){ const tree=git(f.root,['rev-parse',`${f.baseline}^{tree}`]); const sha=git(f.root,['commit-tree',tree,'-p',f.baseline],{input:'advance\n'}); git(f.root,['push','-q','origin',`${sha}:refs/heads/main`]); }

test('evidence stays ignored and sanitizes structured secrets', async()=>{
  const e=await load('evidence.mjs'); assert.equal(typeof e.writeEvidence,'function','evidence module missing');
  const r=repo(), before=git(r,['status','--porcelain']);
  const p=e.writeEvidence(r,{schema_version:1,command:'inspect',token:'x',nested:{authorization:'y',keep:1},ao_mutation:'NONE'},{taskId:'DISCOVERY',now:'2026-08-19T12:00:00Z'});
  assert.match(p.replaceAll('\\','/'),/\.local\/ao\/DISCOVERY\//); assert.equal(git(r,['status','--porcelain']),before);
  const saved=JSON.parse(fs.readFileSync(p,'utf8')); assert.equal(saved.token,'[REDACTED]'); assert.equal(saved.nested.authorization,'[REDACTED]'); assert.equal(saved.nested.keep,1);
});

test('read-only Git primitives enforce exact SHA and repository normalization', async()=>{
  const g=await load('git-state.mjs'); assert.equal(typeof g.findRepoRoot,'function','git module missing');
  const r=repo(); git(r,['branch','-M','feat/x']); const base=git(r,['rev-parse','HEAD']); write(r,'src/a','a\n'); const head=commit(r,'writer');
  assert.equal(g.findRepoRoot(path.join(r,'src')),r); assert.equal(g.getCurrentBranch(r),'feat/x'); assert.equal(g.getHeadSha(r),head); assert.equal(g.isTreeClean(r),true);
  assert.equal(g.resolveExactCommit(r,base),base); assert.throws(()=>g.resolveExactCommit(r,'HEAD'),/40-character/); assert.equal(g.isAncestor(r,base,head),true);
  assert.deepEqual(g.changedPaths(r,base,head),['src/a']); assert.ok(g.listWorktrees(r).some(w=>path.resolve(w.path)===path.resolve(r)));
  assert.equal(g.normalizeRepositoryUrl('git@github.com:ShenJun93/tieu-tien-ky-game.git'),'shenjun93/tieu-tien-ky-game');
});

test('passive authority is observable but malformed/unsupported authority is invalid configuration', async()=>{
  const {inspectAuthority}=await load('authority.mjs'); assert.equal(typeof inspectAuthority,'function','authority module missing');
  const r=repo(); write(r,'docs/governance/NEXT_TASK.md',md({state:'DISCOVERY',task_id:null,branch:null,baseline_ref:null,task_file:null,evidence_file:null,allowed_paths:[],forbidden_paths:[],stop_condition:'HUMAN'})); commit(r,'discovery');
  let x=inspectAuthority(r); assert.equal(x.gate_status,'PASS'); assert.equal(x.mutation_authority,'NONE'); assert.equal(x.next_mechanical,null); assert.equal(x.repository_status,'NOT_APPLICABLE'); assert.equal(x.live_main_status,'NOT_APPLICABLE');
  write(r,'docs/governance/NEXT_TASK.md',md({state:'MAGIC',allowed_paths:[],forbidden_paths:[],stop_condition:'X'})); commit(r,'bad'); assert.equal(inspectAuthority(r).gate_status,'INVALID_INVOCATION');
  write(r,'docs/governance/NEXT_TASK.md','# x\n```json\n{ bad\n```\n'); commit(r,'bad json'); assert.equal(inspectAuthority(r).gate_status,'INVALID_INVOCATION');
});

test('active authority validates repo, exact baseline, activation lock and live-main drift', async()=>{
  const {inspectAuthority}=await load('authority.mjs'); assert.equal(typeof inspectAuthority,'function');
  let f=active(); let x=inspectAuthority(f.root); assert.equal(x.gate_status,'PASS',x.error); assert.equal(x.authority_transition_ref,f.activation); assert.equal(x.live_main_sha,f.baseline);
  f=active({repository:'Other/Repo'}); assert.equal(inspectAuthority(f.root).gate_status,'BLOCKED_REPOSITORY');
  f=active({badBaseline:'HEAD~1',writer:false}); assert.equal(inspectAuthority(f.root).gate_status,'INVALID_INVOCATION');
  f=active({extra:'extra.txt',writer:false}); assert.equal(inspectAuthority(f.root).gate_status,'BLOCKED_ACTIVATION');
  f=active({editTask:true,writer:false}); assert.equal(inspectAuthority(f.root).gate_status,'BLOCKED_ACTIVATION');
  f=active({editNext:true,writer:false}); assert.equal(inspectAuthority(f.root).gate_status,'BLOCKED_ACTIVATION');
  f=active(); advance(f); assert.equal(inspectAuthority(f.root).gate_status,'BLOCKED_LIVE_MAIN_DRIFT');
});

test('REVIEW validates declared repository/baseline/live-main without activation validation', async()=>{
  const {inspectAuthority}=await load('authority.mjs'); const {runCli}=await load('cli.mjs');
  let f=active({state:'REVIEW',writer:false}); let x=inspectAuthority(f.root); assert.equal(x.gate_status,'PASS',x.error); assert.equal(x.repository_status,'PASS'); assert.equal(x.live_main_status,'PASS'); assert.equal(x.live_main_sha,f.baseline); assert.equal(x.authority_transition_ref,null);
  f=active({state:'REVIEW',writer:false,repository:'Other/Repo'}); assert.equal(inspectAuthority(f.root).gate_status,'BLOCKED_REPOSITORY'); assert.equal(runCli(['inspect'],{cwd:f.root,writeLine:()=>{}}),1);
  f=active({state:'REVIEW',writer:false,badBaseline:'HEAD~1'}); assert.equal(inspectAuthority(f.root).gate_status,'INVALID_INVOCATION'); assert.equal(runCli(['inspect'],{cwd:f.root,writeLine:()=>{}}),2);
  f=active({state:'REVIEW',writer:false}); advance(f); assert.equal(inspectAuthority(f.root).gate_status,'BLOCKED_LIVE_MAIN_DRIFT'); assert.equal(runCli(['inspect'],{cwd:f.root,writeLine:()=>{}}),1);
});

test('workspace policy is inspection-only and isolated policy requires a linked worktree', async()=>{
  const {inspectWorkspace}=await load('workspace.mjs'); assert.equal(typeof inspectWorkspace,'function');
  const r=repo(); assert.equal(inspectWorkspace(r,{workspace_policy:'ISOLATED_WORKTREE'}).gate_status,'BLOCKED_WORKSPACE');
  assert.equal(inspectWorkspace(r,{workspace_policy:'EXISTING_AUTHORIZED_WORKTREE'}).gate_status,'PASS');
  assert.equal(inspectWorkspace(r,{workspace_policy:'REMOTE_GITHUB_BRANCH'}).gate_status,'PASS');
  const linked=fs.mkdtempSync(path.join(os.tmpdir(),'ttk-linked-')); fs.rmSync(linked,{recursive:true,force:true}); git(r,['worktree','add','-q','-b','linked',linked]);
  assert.equal(inspectWorkspace(linked,{workspace_policy:'ISOLATED_WORKTREE'}).gate_status,'PASS');
});

test('candidate entry fails closed for wrong identity, dirty/empty delta, scope and remote policy', async()=>{
  const {inspectAuthority}=await load('authority.mjs'), {verifyCandidate}=await load('candidate-gate.mjs'); assert.equal(typeof verifyCandidate,'function');
  let f=active(), a=inspectAuthority(f.root); assert.equal(verifyCandidate({root:f.root,candidateSha:f.baseline,authoritySnapshot:a,checks:[]}).gate_status,'BLOCKED_CANDIDATE');
  f=active(); a=inspectAuthority(f.root); git(f.root,['branch','-M','wrong']); assert.equal(verifyCandidate({root:f.root,candidateSha:f.candidate,authoritySnapshot:a,checks:[]}).gate_status,'BLOCKED_BRANCH');
  f=active(); a=inspectAuthority(f.root); write(f.root,'dirty.txt','x'); assert.equal(verifyCandidate({root:f.root,candidateSha:f.candidate,authoritySnapshot:a,checks:[]}).gate_status,'BLOCKED_CANDIDATE');
  f=active({writer:false}); a=inspectAuthority(f.root); assert.equal(verifyCandidate({root:f.root,candidateSha:f.activation,authoritySnapshot:a,checks:[]}).gate_status,'BLOCKED_CANDIDATE');
  f=active({writerPath:'outside.txt'}); a=inspectAuthority(f.root); assert.equal(verifyCandidate({root:f.root,candidateSha:f.candidate,authoritySnapshot:a,checks:[]}).gate_status,'BLOCKED_SCOPE');
  f=active({allowed:['blocked/'],forbidden:['blocked/'],writerPath:'blocked/x'}); a=inspectAuthority(f.root); assert.equal(verifyCandidate({root:f.root,candidateSha:f.candidate,authoritySnapshot:a,checks:[]}).gate_status,'BLOCKED_SCOPE');
  f=active({policy:'REMOTE_GITHUB_BRANCH'}); a=inspectAuthority(f.root); assert.equal(verifyCandidate({root:f.root,candidateSha:f.candidate,authoritySnapshot:a,checks:[]}).gate_status,'BLOCKED_WORKSPACE');
});

test('candidate checks block nonzero and preserve dirty/HEAD mutations', async()=>{
  const {inspectAuthority}=await load('authority.mjs'), {verifyCandidate}=await load('candidate-gate.mjs'); assert.equal(typeof verifyCandidate,'function');
  let f=active(), a=inspectAuthority(f.root);
  let x=verifyCandidate({root:f.root,candidateSha:f.candidate,authoritySnapshot:a,checks:[{name:'fail',argv:[process.execPath,'-e','process.exit(7)']}]});
  assert.equal(x.gate_status,'BLOCKED_CHECK'); assert.equal(x.checks[0].exit_code,7);
  f=active(); a=inspectAuthority(f.root); const file=path.join(f.root,'src/x.txt');
  x=verifyCandidate({root:f.root,candidateSha:f.candidate,authoritySnapshot:a,checks:[{name:'dirty',argv:[process.execPath,'-e',`require('fs').appendFileSync(${JSON.stringify(file)},'dirty\\n')`]}]});
  assert.equal(x.gate_status,'CHECK_MUTATED_CANDIDATE'); assert.match(fs.readFileSync(file,'utf8'),/dirty/);
  f=active(); a=inspectAuthority(f.root);
  const mutateHead=`const fs=require('fs'),{execFileSync:e}=require('child_process');fs.writeFileSync('h','x');e('git',['add','h']);e('git',['commit','-q','-m','changed']);`;
  x=verifyCandidate({root:f.root,candidateSha:f.candidate,authoritySnapshot:a,checks:[{name:'head',argv:[process.execPath,'-e',mutateHead]}]});
  assert.equal(x.gate_status,'CHECK_MUTATED_CANDIDATE'); assert.equal(x.mutation_detected,true);
});

test('project policy only selects fixed diff/AO/governance checks', async()=>{
  const {candidateChecks}=await load('project-policy.mjs'); assert.equal(typeof candidateChecks,'function');
  const checks=candidateChecks({baselineRef:'a'.repeat(40),candidateSha:'b'.repeat(40),writerPaths:['scripts/ao/cli.mjs','.gitignore']});
  assert.deepEqual(checks.map(c=>c.name),['diff_check','ao_tests']);
  assert.doesNotMatch(JSON.stringify(checks),/Unity|push|reset|rebase|checkout|stash|worktree add/i);
});

test('CLI parses stable surface, uses explicit identity status, and HUMAN_GATE remains passive', async()=>{
  const {parseArgs,runCli}=await load('cli.mjs'); assert.equal(typeof runCli,'function','cli module missing');
  assert.deepEqual(parseArgs(['inspect']),{command:'inspect',json:false,candidate:null}); assert.throws(()=>parseArgs(['verify-candidate']),/candidate/i);
  const r=repo(); write(r,'docs/governance/NEXT_TASK.md',md({state:'HUMAN_GATE',task_id:'T',branch:null,baseline_ref:null,task_file:null,evidence_file:null,allowed_paths:[],forbidden_paths:[],stop_condition:'HUMAN'})); commit(r,'human');
  const out=[]; assert.equal(runCli(['inspect'],{cwd:r,writeLine:s=>out.push(s)}),0); const text=out.join('\n');
  assert.match(text,/AUTHORITY_STATE\s+HUMAN_GATE/); assert.match(text,/REPOSITORY\s+NOT_APPLICABLE/); assert.match(text,/LIVE_MAIN\s+NOT_APPLICABLE/); assert.match(text,/MUTATION_AUTHORITY\s+NONE/); assert.match(text,/AO_MUTATION\s+NONE/); assert.match(text,/NEXT_MECHANICAL\s+NONE/);
  write(r,'docs/governance/NEXT_TASK.md','# bad\n```json\n{ bad\n```\n'); commit(r,'bad config'); assert.equal(runCli(['inspect'],{cwd:r,writeLine:()=>{}}),2);
});

test('CLI verify-candidate passes an exact committed local fixture', async()=>{
  const {runCli}=await load('cli.mjs'); assert.equal(typeof runCli,'function');
  const f=active(), out=[]; const code=runCli(['verify-candidate','--candidate',f.candidate,'--json'],{cwd:f.root,writeLine:s=>out.push(s),candidateChecks:()=>[]});
  assert.equal(code,0,out.join('\n')); const report=JSON.parse(out.at(-1)); assert.equal(report.gate_status,'PASS'); assert.equal(report.candidate_sha,f.candidate); assert.equal(report.ao_mutation,'NONE');
});

test('runtime has no mutation/publication/provider shell capability', ()=>{
  const names=fs.readdirSync(here).filter(n=>n.endsWith('.mjs')&&n!=='ao.test.mjs'); assert.ok(names.length>=7);
  const src=names.map(n=>fs.readFileSync(path.join(here,n),'utf8')).join('\n');
  assert.doesNotMatch(src,/['"]push['"]|['"]reset['"]|['"]rebase['"]|['"]checkout['"]|['"]stash['"]|['"]commit['"]|['"]add['"]/);
  assert.doesNotMatch(src,/worktree\s+add|worktree\s+remove|gh\s+pr|createPullRequest|mergePullRequest|codex|claude|gemini|shell\s*:\s*true/i);
});
