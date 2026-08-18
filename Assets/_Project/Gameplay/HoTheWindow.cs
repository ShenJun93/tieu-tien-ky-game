using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>Pure bounded-window timing for Hộ Thể's active defensive decision. No UnityEngine.Time dependency - currentTime is injected.</summary>
    public readonly struct HoTheWindow
    {
        public readonly float ActivatedAtTime;
        public readonly float DurationSeconds;

        public HoTheWindow(float activatedAtTime, float durationSeconds)
        {
            ActivatedAtTime = activatedAtTime;
            DurationSeconds = Mathf.Max(0f, durationSeconds);
        }

        public bool IsActive(float currentTime) => currentTime >= ActivatedAtTime && currentTime < ActivatedAtTime + DurationSeconds;
    }
}
