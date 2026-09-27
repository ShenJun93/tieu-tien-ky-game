using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

namespace TieuTienKy.EditorTools.Look
{
    /// <summary>
    /// LOOK-1f: builds an Android APK that contains only the arena lab, so the Director can judge the
    /// look on the phone next to the reference games. It installs beside the game under its own
    /// application id, and the id and name overrides are restored after the build. Build it from a
    /// clean commit; it does not regenerate the scene.
    ///   Unity -batchmode -projectPath . -executeMethod TieuTienKy.EditorTools.Look.LookShowcaseBuild.Build -quit
    /// Output: Builds/Android/TieuTienKy-Showcase-&lt;shortSha&gt;.apk
    /// </summary>
    public static class LookShowcaseBuild
    {
        const string Scene = "Assets/_Project/Scenes/Look/ArenaLab.unity";
        const string ShowcaseId = "com.shenjun93.tieutienky.showcase";
        const string ShowcaseName = "TTK Showcase";

        [MenuItem("Tieu Tien Ky/Look/Build Showcase APK")]
        public static void Build()
        {
            var android = NamedBuildTarget.Android;
            string previousId = PlayerSettings.GetApplicationIdentifier(android);
            string previousName = PlayerSettings.productName;
            string sha = ShortSha();
            string output = $"Builds/Android/TieuTienKy-Showcase-{sha}.apk";
            Directory.CreateDirectory("Builds/Android");
            try
            {
                PlayerSettings.SetApplicationIdentifier(android, ShowcaseId);
                PlayerSettings.productName = ShowcaseName;
                BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    scenes = new[] { Scene },
                    locationPathName = output,
                    target = BuildTarget.Android,
                    options = BuildOptions.None,
                });
                Debug.Log($"[TTK_SHOWCASE_BUILD] result={report.summary.result} errors={report.summary.totalErrors} output={output} sourceSha={sha}");
                if (report.summary.result != BuildResult.Succeeded)
                    throw new InvalidOperationException($"[TTK_SHOWCASE_BUILD] build failed: {report.summary.result}");
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
