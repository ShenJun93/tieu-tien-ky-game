using System.Collections;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Primitive impact-enhancing technique: briefly drops Time.timeScale so a
    /// landed hit reads as a real impact instead of a debug trigger. Runs on
    /// realtime so the freeze itself is not skipped by the low timescale it
    /// sets. No generic feedback/juice framework.
    /// </summary>
    public static class HitStop
    {
        public static IEnumerator Routine(float freezeSeconds, float freezeTimeScale)
        {
            float originalTimeScale = Time.timeScale;
            Time.timeScale = freezeTimeScale;

            yield return new WaitForSecondsRealtime(freezeSeconds);

            Time.timeScale = originalTimeScale;
        }
    }
}
