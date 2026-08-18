using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Water Shift and Spirit Wind arena chaos, sequenced through
    /// ArenaEventCycle's pure warning/active/cooldown timing. Positioning
    /// and knockback only - arena events never deal direct damage. Only one
    /// Water Zone ever exists; Spirit Wind displaces actors caught in a
    /// telegraphed lane.
    /// </summary>
    public sealed class ArenaEventDirector : MonoBehaviour
    {
        [SerializeField] float warningSeconds = 0.8f;

        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";
        static readonly int TintColorPropertyId = Shader.PropertyToID("_Color");
        static readonly Color WaterShiftWarningColor = new Color(0.3f, 0.7f, 1f, 0.75f);
        static readonly Color SpiritWindWarningColor = new Color(0.85f, 0.85f, 0.95f, 0.6f);

        static Material s_PrimitiveMaterial;

        WaterZone waterZone;
        Transform waterTransform;
        Vector3[] waterPositions;
        int currentWaterPositionIndex;
        GameObject activeMarker;

        public void Initialize(WaterZone zone, Transform zoneTransform, Vector3[] positions)
        {
            waterZone = zone;
            waterTransform = zoneTransform;
            waterPositions = positions;
            currentWaterPositionIndex = 0;

            if (waterPositions == null || waterTransform == null)
            {
                return;
            }

            for (int i = 0; i < waterPositions.Length; i++)
            {
                if (Vector3.Distance(waterPositions[i], waterTransform.position) < 0.01f)
                {
                    currentWaterPositionIndex = i;
                    break;
                }
            }
        }

        public IEnumerator PlayWaterShift()
        {
            if (waterZone == null || waterPositions == null || waterPositions.Length == 0)
            {
                yield break;
            }

            var cycle = new ArenaEventCycle(warningSeconds, activeSeconds: 0.1f, cooldownSeconds: 0.1f);
            cycle.Begin(Time.time);

            int nextIndex = (currentWaterPositionIndex + 1) % waterPositions.Length;
            Vector3 destination = waterPositions[nextIndex];

            SpawnMarker(destination, WaterShiftWarningColor, new Vector3(waterTransform.localScale.x, 0.05f, waterTransform.localScale.z));

            while (true)
            {
                cycle.Tick(Time.time, out bool becameActive, out bool finished);

                if (becameActive)
                {
                    waterTransform.position = destination;
                    currentWaterPositionIndex = nextIndex;
                    HideMarker();
                }

                if (finished)
                {
                    break;
                }

                yield return null;
            }

            HideMarker();
        }

        public IEnumerator PlaySpiritWind(IReadOnlyList<Combatant> actors, Bounds lane, Vector3 impulse)
        {
            var cycle = new ArenaEventCycle(warningSeconds, activeSeconds: 0.15f, cooldownSeconds: 0.1f);
            cycle.Begin(Time.time);

            SpawnMarker(lane.center, SpiritWindWarningColor, new Vector3(lane.size.x, 0.1f, lane.size.z));

            while (true)
            {
                cycle.Tick(Time.time, out bool becameActive, out bool finished);

                if (becameActive)
                {
                    ApplySpiritWindImpulse(actors, lane, impulse);
                    HideMarker();
                }

                if (finished)
                {
                    break;
                }

                yield return null;
            }

            HideMarker();
        }

        /// <summary>Cancels any pending/active event coroutine and clears a lingering warning marker. Safe to call even if this component never owned the driving coroutine (e.g. a caller stopped it via its own StopAllCoroutines).</summary>
        public void ResetEvents()
        {
            StopAllCoroutines();
            HideMarker();
        }

        static void ApplySpiritWindImpulse(IReadOnlyList<Combatant> actors, Bounds lane, Vector3 impulse)
        {
            if (actors == null)
            {
                return;
            }

            for (int i = 0; i < actors.Count; i++)
            {
                Combatant actor = actors[i];
                if (actor == null || actor.IsDefeated)
                {
                    continue;
                }

                if (!lane.Contains(actor.transform.position))
                {
                    continue;
                }

                var receiver = actor.GetComponent<KnockbackReceiver>();
                receiver?.ApplyKnockback(impulse);
            }
        }

        void HideMarker()
        {
            if (activeMarker != null)
            {
                Destroy(activeMarker);
                activeMarker = null;
            }
        }

        void SpawnMarker(Vector3 position, Color color, Vector3 scale)
        {
            HideMarker();

            activeMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            activeMarker.name = "ArenaEventWarning_Primitive";
            Destroy(activeMarker.GetComponent<Collider>());

            activeMarker.transform.position = position + Vector3.up * 0.05f;
            activeMarker.transform.localScale = scale;

            var renderer = activeMarker.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = PrimitiveMaterial();

                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetColor(TintColorPropertyId, color);
                renderer.SetPropertyBlock(block);
            }
        }

        static Material PrimitiveMaterial()
        {
            if (s_PrimitiveMaterial == null)
            {
                s_PrimitiveMaterial = Resources.Load<Material>(PrimitiveMaterialResourcePath);
            }

            return s_PrimitiveMaterial;
        }
    }
}
