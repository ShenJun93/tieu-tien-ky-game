using System.Collections.Generic;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Thin static wrapper around Unity's own AudioSource.PlayClipAtPoint -
    /// not an event bus or audio manager. Clips are loaded once from
    /// Resources/Audio (same convention as the project's existing
    /// Resources.Load material pattern) and cached by name. Presentation-only:
    /// never called from anywhere that decides damage/timing.
    /// </summary>
    public static class CombatAudio
    {
        const string ResourceFolder = "Audio/";
        static readonly Dictionary<string, AudioClip> Cache = new Dictionary<string, AudioClip>();

        /// <summary>
        /// `pitch` lets a feedback-tuning pass make an existing clip read as
        /// heavier/sharper (e.g. a special-moment layer) without authoring a
        /// new audio asset. Left at the default, this is exactly the prior
        /// PlayClipAtPoint behavior; a non-default pitch falls back to a
        /// short-lived manual AudioSource since PlayClipAtPoint itself has no
        /// pitch parameter.
        /// </summary>
        public static void Play(string clipName, Vector3 position, float volume = 1f, float pitch = 1f)
        {
            AudioClip clip = Load(clipName);
            if (clip == null)
            {
                return;
            }

            if (Mathf.Approximately(pitch, 1f))
            {
                AudioSource.PlayClipAtPoint(clip, position, volume);
                return;
            }

            var oneShot = new GameObject("OneShotAudio_" + clipName);
            oneShot.transform.position = position;
            var source = oneShot.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.spatialBlend = 0f;
            source.Play();
            Object.Destroy(oneShot, clip.length / Mathf.Max(0.05f, Mathf.Abs(pitch)));
        }

        static AudioClip Load(string clipName)
        {
            if (Cache.TryGetValue(clipName, out AudioClip cached))
            {
                return cached;
            }

            AudioClip clip = Resources.Load<AudioClip>(ResourceFolder + clipName);
            Cache[clipName] = clip;
            return clip;
        }
    }
}
