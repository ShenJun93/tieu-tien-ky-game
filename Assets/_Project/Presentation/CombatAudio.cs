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

        public static void Play(string clipName, Vector3 position, float volume = 1f)
        {
            AudioClip clip = Load(clipName);
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, position, volume);
            }
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
