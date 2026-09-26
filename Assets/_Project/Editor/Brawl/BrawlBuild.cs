using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

namespace TieuTienKy.EditorTools.Brawl
{
    /// <summary>
    /// Builds the playable brawl scene as its own Android app, which installs beside the old game.
    /// Build it from a clean commit; it does not regenerate the scene.
    ///   Unity -batchmode -projectPath . -executeMethod TieuTienKy.EditorTools.Brawl.BrawlBuild.Build -quit
    /// Output: Builds/Android/TieuTienKy-Brawl-&lt;shortSha&gt;.apk
    /// </summary>
    public static class BrawlBuild
    {
        const string AppId = "com.shenjun93.tieutienky.brawl";
        const string AppName = "TTK Brawl";

        [MenuItem("Tieu Tien Ky/Brawl/Build Brawl APK")]
        public static void Build()
        {
            var android = NamedBuildTarget.Android;
            string previousId = PlayerSettings.GetApplicationIdentifier(android);
            string previousName = PlayerSettings.productName;
            string sha = ShortSha();
            string output = $"Builds/Android/TieuTienKy-Brawl-{sha}.apk";
            Directory.CreateDirectory("Builds/Android");
            try
            {
                PlayerSettings.SetApplicationIdentifier(android, AppId);
                PlayerSettings.productName = AppName;
                BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    scenes = new[] { BrawlArenaBuilder.ScenePath },
                    locationPathName = output,
                    target = BuildTarget.Android,
                    options = BuildOptions.None,
                });
                Debug.Log($"[TTK_BRAWL_BUILD] result={report.summary.result} errors={report.summary.totalErrors} output={output} sourceSha={sha}");
                if (report.summary.result != BuildResult.Succeeded)
                    throw new InvalidOperationException($"[TTK_BRAWL_BUILD] build failed: {report.summary.result}");
            }
            finally
            {
                PlayerSettings.SetApplicationIdentifier(android, previousId);
                PlayerSettings.productName = previousName;
            }
        }

        static string ShortSha()
        {
            var psi = new ProcessStartInfo("git", "rev-parse --short HEAD") { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };
            using (var p = Process.Start(psi))
            {
                string sha = p.StandardOutput.ReadToEnd().Trim();
                p.WaitForExit();
                return string.IsNullOrEmpty(sha) ? "nosha" : sha;
            }
        }
    }
}
