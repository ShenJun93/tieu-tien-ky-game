using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

namespace TieuTienKy.EditorTools.Build
{
    /// <summary>
    /// Durable Android build entry point, callable deterministically via
    /// `-executeMethod TieuTienKy.EditorTools.Build.AndroidBuildEntryPoint.Build`
    /// (paired with `-quit`; see ttk-runtime-verify Skill for the invocation rule).
    /// Replaces this project's prior pattern of temporary, never-committed
    /// per-task BuildPipeline.BuildPlayer scripts.
    /// </summary>
    public static class AndroidBuildEntryPoint
    {
        const string OutputDirectory = "Builds/Android";
        const string DefaultLabel = "Dev";
        const string LabelEnvironmentVariable = "TTK_BUILD_LABEL";

        [MenuItem("Tools/TTK/Build Android (Runtime Verify Entry Point)")]
        public static void Build()
        {
            string shortSha = ResolveShortSha();
            string label = ResolveLabel();
            string fileName = $"TieuTienKy-{label}-{shortSha}.apk";

            Directory.CreateDirectory(OutputDirectory);
            string outputPath = Path.Combine(OutputDirectory, fileName);

            string[] scenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                throw new InvalidOperationException(
                    "[TTK_ANDROID_BUILD] No enabled scenes found in EditorBuildSettings; refusing to build an empty Android artifact.");
            }

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = outputPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;

            Debug.Log(
                $"[TTK_ANDROID_BUILD] result={summary.result} totalErrors={summary.totalErrors} " +
                $"totalWarnings={summary.totalWarnings} outputPath={summary.outputPath} sourceSha={shortSha}");

            if (summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"[TTK_ANDROID_BUILD] Android build did not succeed: result={summary.result} " +
                    $"totalErrors={summary.totalErrors}. See the build log for detail.");
            }
        }

        static string ResolveLabel()
        {
            string label = Environment.GetEnvironmentVariable(LabelEnvironmentVariable);
            return string.IsNullOrWhiteSpace(label) ? DefaultLabel : label;
        }

        static string ResolveShortSha()
        {
            var startInfo = new ProcessStartInfo("git", "rev-parse --short HEAD")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd().Trim();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0 || string.IsNullOrEmpty(output))
                {
                    throw new InvalidOperationException(
                        $"[TTK_ANDROID_BUILD] Could not resolve source git SHA for artifact naming: exitCode={process.ExitCode} stderr={error}");
                }

                return output;
            }
        }
    }
}
