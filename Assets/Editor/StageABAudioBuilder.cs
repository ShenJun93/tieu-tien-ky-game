using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TieuTienKy.EditorTools
{
    /// <summary>
    /// Generates the bounded local audio identity Task A4 requires (14 short
    /// clips) since no source audio exists anywhere in the project and the
    /// task forbids sourcing from the web. Pure procedural synthesis (sine/
    /// square/noise + simple envelopes) written directly to 16-bit PCM WAV
    /// files under Assets/_Project/Resources/Audio, matching the project's
    /// existing Resources.Load convention (see P0A_Greybox.mat). Not a
    /// generic audio framework - one fixed, bounded clip list, re-runnable.
    /// </summary>
    public static class StageABAudioBuilder
    {
        const string AudioDir = "Assets/_Project/Resources/Audio";
        const int SampleRate = 22050;

        [MenuItem("Tools/Stage AB/Build Combat Audio (A4)")]
        public static void BuildCombatAudio()
        {
            EnsureFolder(AudioDir);

            WriteClip("BasicSwing", Swing(0.12f, 900f, 500f, 0.5f));
            WriteClip("BasicHit", Thump(0.12f, 130f, 0.7f, 0.02f));
            WriteClip("LoiTramCast", Sweep(0.2f, 400f, 1400f, WaveShape.Sine, 0.25f, 0.5f));
            WriteClip("LoiTramImpact", Zap(0.15f, 1100f, 0.6f));
            WriteClip("PhongBoMove", Swing(0.15f, 1400f, 300f, 0.35f));
            WriteClip("HoTheActivate", Chime(new[] { 500f, 700f }, 0.12f, 0.35f));
            WriteClip("EnemyTelegraph", Beep(0.15f, 300f, WaveShape.Square, 0.35f));
            WriteClip("PlayerHit", Thump(0.15f, 95f, 0.85f, 0.01f));
            WriteClip("EnemyHit", Thump(0.12f, 165f, 0.6f, 0.02f));
            WriteClip("EnemyDeath", Sweep(0.35f, 220f, 55f, WaveShape.Triangle, 0.55f, 0f));
            WriteClip("BossArrival", Boom(0.55f, 60f, 0.8f));
            WriteClip("UIConfirm", Beep(0.08f, 820f, WaveShape.Sine, 0.3f));
            WriteClip("Victory", Chime(new[] { 523f, 659f, 784f }, 0.18f, 0.45f));
            WriteClip("Defeat", Chime(new[] { 392f, 311f }, 0.28f, 0.5f));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[STAGE_AB_A4] Combat audio built at " + AudioDir);
        }

        enum WaveShape { Sine, Square, Triangle }

        static float Wave(WaveShape shape, float phase01)
        {
            float p = phase01 - Mathf.Floor(phase01);
            switch (shape)
            {
                case WaveShape.Square:
                    return p < 0.5f ? 1f : -1f;
                case WaveShape.Triangle:
                    return 4f * Mathf.Abs(p - 0.5f) - 1f;
                default:
                    return Mathf.Sin(p * Mathf.PI * 2f);
            }
        }

        /// <summary>Broadband whoosh: filtered noise with a fast attack/decay envelope and a frequency-modulated tone layered in for a "swing" character.</summary>
        static float[] Swing(float duration, float freqStart, float freqEnd, float amplitude)
        {
            int count = Mathf.CeilToInt(duration * SampleRate);
            var samples = new float[count];
            var rng = new System.Random(12345);
            float phase = 0f;

            for (int i = 0; i < count; i++)
            {
                float t = (float)i / count;
                float freq = Mathf.Lerp(freqStart, freqEnd, t);
                phase += freq / SampleRate;
                float tone = Wave(WaveShape.Sine, phase);
                float noise = (float)(rng.NextDouble() * 2.0 - 1.0);
                float envelope = Mathf.Sin(t * Mathf.PI) * amplitude;
                samples[i] = (tone * 0.4f + noise * 0.6f) * envelope;
            }

            return samples;
        }

        /// <summary>Percussive low-frequency thump: fast attack, exponential decay - the shared shape for every impact/hit/death cue, varied by pitch and decay time.</summary>
        static float[] Thump(float duration, float freq, float amplitude, float attackSeconds)
        {
            int count = Mathf.CeilToInt(duration * SampleRate);
            var samples = new float[count];
            float phase = 0f;
            int attackSamples = Mathf.Max(1, Mathf.RoundToInt(attackSeconds * SampleRate));

            for (int i = 0; i < count; i++)
            {
                phase += freq / SampleRate;
                float tone = Wave(WaveShape.Sine, phase);
                float t = (float)i / count;
                float decay = Mathf.Exp(-t * 6f);
                float attack = i < attackSamples ? (float)i / attackSamples : 1f;
                samples[i] = tone * decay * attack * amplitude;
            }

            return samples;
        }

        /// <summary>Pure frequency sweep with a smooth envelope - used for casts and the descending death tone.</summary>
        static float[] Sweep(float duration, float freqStart, float freqEnd, WaveShape shape, float amplitude, float sustain01)
        {
            int count = Mathf.CeilToInt(duration * SampleRate);
            var samples = new float[count];
            float phase = 0f;

            for (int i = 0; i < count; i++)
            {
                float t = (float)i / count;
                float freq = Mathf.Lerp(freqStart, freqEnd, t);
                phase += freq / SampleRate;
                float envelope = t < sustain01 ? 1f : Mathf.Clamp01(1f - (t - sustain01) / Mathf.Max(0.001f, 1f - sustain01));
                samples[i] = Wave(shape, phase) * envelope * amplitude;
            }

            return samples;
        }

        /// <summary>Sharp high-frequency crack with noise - the electric "zap" impact distinct from the low thumps.</summary>
        static float[] Zap(float duration, float freq, float amplitude)
        {
            int count = Mathf.CeilToInt(duration * SampleRate);
            var samples = new float[count];
            var rng = new System.Random(777);
            float phase = 0f;

            for (int i = 0; i < count; i++)
            {
                phase += freq / SampleRate;
                float tone = Wave(WaveShape.Square, phase);
                float noise = (float)(rng.NextDouble() * 2.0 - 1.0);
                float t = (float)i / count;
                float decay = Mathf.Exp(-t * 10f);
                samples[i] = (tone * 0.5f + noise * 0.5f) * decay * amplitude;
            }

            return samples;
        }

        /// <summary>Two/three-tone chime played in short sequence - Hộ Thể activation, Victory and Defeat all reuse this with different note sets/order.</summary>
        static float[] Chime(float[] frequencies, float noteDuration, float amplitude)
        {
            int noteCount = Mathf.CeilToInt(noteDuration * SampleRate);
            var samples = new float[noteCount * frequencies.Length];

            for (int n = 0; n < frequencies.Length; n++)
            {
                float phase = 0f;
                for (int i = 0; i < noteCount; i++)
                {
                    phase += frequencies[n] / SampleRate;
                    float t = (float)i / noteCount;
                    float envelope = Mathf.Sin(t * Mathf.PI);
                    samples[n * noteCount + i] = Wave(WaveShape.Sine, phase) * envelope * amplitude;
                }
            }

            return samples;
        }

        /// <summary>Short clean beep - UI confirm and enemy telegraph warning share this shape at different pitches/waveforms.</summary>
        static float[] Beep(float duration, float freq, WaveShape shape, float amplitude)
        {
            int count = Mathf.CeilToInt(duration * SampleRate);
            var samples = new float[count];
            float phase = 0f;

            for (int i = 0; i < count; i++)
            {
                phase += freq / SampleRate;
                float t = (float)i / count;
                float envelope = t < 0.7f ? 1f : (1f - (t - 0.7f) / 0.3f);
                samples[i] = Wave(shape, phase) * envelope * amplitude;
            }

            return samples;
        }

        /// <summary>Very low sine with slow decay plus a touch of noise - the boss's one-shot arrival weight cue.</summary>
        static float[] Boom(float duration, float freq, float amplitude)
        {
            int count = Mathf.CeilToInt(duration * SampleRate);
            var samples = new float[count];
            var rng = new System.Random(4242);
            float phase = 0f;

            for (int i = 0; i < count; i++)
            {
                phase += freq / SampleRate;
                float tone = Wave(WaveShape.Sine, phase);
                float noise = (float)(rng.NextDouble() * 2.0 - 1.0);
                float t = (float)i / count;
                float decay = Mathf.Exp(-t * 3f);
                float attack = Mathf.Clamp01(i / (0.01f * SampleRate));
                samples[i] = (tone * 0.85f + noise * 0.15f) * decay * attack * amplitude;
            }

            return samples;
        }

        static void WriteClip(string name, float[] samples)
        {
            string path = AudioDir + "/" + name + ".wav";
            byte[] wav = EncodeWav(samples, SampleRate);
            File.WriteAllBytes(path, wav);
            AssetDatabase.ImportAsset(path);
        }

        static byte[] EncodeWav(float[] samples, int sampleRate)
        {
            const int bitsPerSample = 16;
            const int channels = 1;
            int byteRate = sampleRate * channels * bitsPerSample / 8;
            int dataSize = samples.Length * channels * bitsPerSample / 8;

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(new[] { 'R', 'I', 'F', 'F' });
            writer.Write(36 + dataSize);
            writer.Write(new[] { 'W', 'A', 'V', 'E' });
            writer.Write(new[] { 'f', 'm', 't', ' ' });
            writer.Write(16);
            writer.Write((short)1); // PCM
            writer.Write((short)channels);
            writer.Write(sampleRate);
            writer.Write(byteRate);
            writer.Write((short)(channels * bitsPerSample / 8));
            writer.Write((short)bitsPerSample);
            writer.Write(new[] { 'd', 'a', 't', 'a' });
            writer.Write(dataSize);

            foreach (float sample in samples)
            {
                short s = (short)(Mathf.Clamp(sample, -1f, 1f) * short.MaxValue);
                writer.Write(s);
            }

            return stream.ToArray();
        }

        static void EnsureFolder(string assetPath)
        {
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                return;
            }

            string[] parts = assetPath.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }
    }
}
