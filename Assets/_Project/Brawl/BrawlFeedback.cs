using System.Collections.Generic;
using TieuTienKy.Combat;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

namespace TieuTienKy.Brawl
{
    /// <summary>
    /// Hit feedback for the brawl (CORE-1b-iii): the slash arc on each swing, a spark and a damage
    /// number on each hit, and a camera kick when the local fighter lands or takes a hit. Flash and
    /// hitstop poses live on <see cref="FighterView"/>. Everything is pooled; nothing here touches
    /// the simulation.
    /// </summary>
    public sealed class BrawlFeedback : MonoBehaviour
    {
        [SerializeField] GameObject hitSpark;
        [SerializeField] GameObject slash;
        [SerializeField] TMP_FontAsset numberFont;
        [SerializeField] CinemachineImpulseSource impulse;
        [SerializeField] Color numberColor = Color.white;
        [SerializeField] Color finisherColor = new Color(1f, 0.69f, 0.18f);
        [SerializeField] Color localHurtColor = new Color(0.91f, 0.2f, 0.18f);
        [SerializeField] float lightShake = 0.35f;
        [SerializeField] float finisherShake = 1f;

        readonly List<VfxBurst> sparks = new List<VfxBurst>();
        readonly List<VfxBurst> slashes = new List<VfxBurst>();
        readonly List<DamageNumber> numbers = new List<DamageNumber>();

        /// <summary>Counts damage numbers shown, for tests and debugging.</summary>
        public int NumbersShown { get; private set; }
        public int SlashesShown { get; private set; }

        public void Configure(GameObject spark, GameObject slashArc, TMP_FontAsset font, CinemachineImpulseSource source)
        {
            hitSpark = spark;
            slash = slashArc;
            numberFont = font;
            impulse = source;
        }

        /// <summary>Slash arc at the start of a swing's active ticks, sweeping the way the fighter faces.</summary>
        public void Swing(FighterView view, FighterState state, float height, bool finisher)
        {
            if (slash == null) return;
            var facing = new Vector3(state.Facing.x, 0f, state.Facing.y);
            // Alternate the tilt so consecutive cuts read as different strokes; the finisher is flat and wide.
            float tilt = finisher ? 0f : (state.ComboStep % 2 == 1 ? -22f : 22f);
            var rotation = Quaternion.LookRotation(facing, Vector3.up) * Quaternion.Euler(0f, 0f, tilt);
            var position = view.transform.position + Vector3.up * height * 0.4f + facing * 0.2f;
            Take(slashes, slash).Play(position, rotation, finisher ? 1.5f : 1.1f, finisher ? 1.25f : 1.1f, 0.16f, false);
            SlashesShown++;
        }

        public void Hit(HitEvent hit, float victimHeight, bool localAttacker, bool localVictim)
        {
            var point = new Vector3(hit.Point.x, victimHeight * 0.55f, hit.Point.y);
            if (hitSpark != null)
                Take(sparks, hitSpark).Play(point, Quaternion.identity, hit.Finisher ? 2.2f : 1.4f, 1.6f, 0.14f, true);

            var color = localVictim ? localHurtColor : hit.Finisher ? finisherColor : numberColor;
            var head = new Vector3(hit.Point.x, victimHeight * 1.1f, hit.Point.y);
            TakeNumber().Show(head, hit.Damage, color, hit.Finisher ? 1.5f : 1f, NumbersShown);
            NumbersShown++;

            if (impulse != null && (localAttacker || localVictim))
                impulse.GenerateImpulseWithForce(hit.Finisher || localVictim ? finisherShake : lightShake);
        }

        VfxBurst Take(List<VfxBurst> pool, GameObject prefab)
        {
            foreach (var b in pool)
                if (!b.gameObject.activeSelf) return b;
            var go = Instantiate(prefab, transform);
            var burst = go.AddComponent<VfxBurst>();
            pool.Add(burst);
            return burst;
        }

        DamageNumber TakeNumber()
        {
            foreach (var n in numbers)
                if (!n.gameObject.activeSelf) return n;
            var go = new GameObject("DamageNumber");
            go.transform.SetParent(transform, false);
            var text = go.AddComponent<TextMeshPro>();
            if (numberFont != null) text.font = numberFont;
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 9f;
            text.fontStyle = FontStyles.Bold;
            text.outlineWidth = 0.25f;
            text.outlineColor = new Color32(20, 16, 12, 255);
            text.textWrappingMode = TextWrappingModes.NoWrap;
            var number = go.AddComponent<DamageNumber>();
            numbers.Add(number);
            return number;
        }
    }
}
