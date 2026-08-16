using UnityEngine;

namespace TieuTienKy.Core
{
    /// <summary>
    /// Plain rate-limit timer. No UnityEngine.Time dependency so it is
    /// directly unit-testable with an injected clock value.
    /// </summary>
    public sealed class Cooldown
    {
        readonly float duration;
        float readyAtTime;

        public Cooldown(float duration)
        {
            this.duration = Mathf.Max(0f, duration);
            readyAtTime = float.NegativeInfinity;
        }

        public float Duration => duration;

        public bool IsReady(float currentTime) => currentTime >= readyAtTime;

        /// <summary>
        /// Attempts to consume the cooldown at currentTime. Returns true and
        /// arms the next window if ready; returns false and leaves state
        /// untouched if still on cooldown.
        /// </summary>
        public bool TryUse(float currentTime)
        {
            if (!IsReady(currentTime))
            {
                return false;
            }

            readyAtTime = currentTime + duration;
            return true;
        }
    }
}
