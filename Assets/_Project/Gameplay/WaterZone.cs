using System.Collections.Generic;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Trigger volume that marks/unmarks IWaterZoneAware entities. One clear
    /// state/tag per the P0A spec; not a generic zone/status framework.
    ///
    /// Membership is established by explicit overlap polling every
    /// FixedUpdate, not by OnTriggerEnter/OnTriggerExit. Two reasons: (1)
    /// OnTriggerEnter is edge-triggered and never fires for a collider
    /// spawned already overlapping (DummyTarget spawns inside this zone),
    /// and (2) a CharacterController-vs-plain-trigger pair with no
    /// Rigidbody/ArticulationBody on either side is not guaranteed to raise
    /// reliable trigger callbacks at all. Polling sidesteps both problems and
    /// is symmetric: it clears membership exactly the same way it sets it,
    /// so a Dummy knocked out of the zone cannot stay stuck "in water".
    /// </summary>
    public sealed class WaterZone : MonoBehaviour
    {
        readonly HashSet<IWaterZoneAware> insideLastCheck = new HashSet<IWaterZoneAware>();
        readonly HashSet<IWaterZoneAware> insideThisCheck = new HashSet<IWaterZoneAware>();

        void FixedUpdate()
        {
            Bounds bounds = GetComponent<Collider>().bounds;
            ApplyMembership(Physics.OverlapBox(bounds.center, bounds.extents, transform.rotation));
        }

        /// <summary>
        /// Given the colliders currently overlapping this zone, marks newly
        /// entered IWaterZoneAware entities true and newly departed ones
        /// false, diffed against the previous call. Separated from the
        /// Physics.OverlapBox call so the actual membership logic - entry
        /// AND exit - is testable without a live physics scene.
        /// </summary>
        public void ApplyMembership(IEnumerable<Collider> overlapping)
        {
            insideThisCheck.Clear();
            foreach (Collider other in overlapping)
            {
                var aware = other.GetComponentInParent<IWaterZoneAware>();
                if (aware != null)
                {
                    insideThisCheck.Add(aware);
                }
            }

            foreach (IWaterZoneAware aware in insideThisCheck)
            {
                if (insideLastCheck.Contains(aware))
                {
                    continue;
                }

                aware.SetInWaterZone(true);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[P0A_WATER] ENTER {aware}");
#endif
            }

            foreach (IWaterZoneAware aware in insideLastCheck)
            {
                if (insideThisCheck.Contains(aware))
                {
                    continue;
                }

                aware.SetInWaterZone(false);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[P0A_WATER] EXIT {aware}");
#endif
            }

            insideLastCheck.Clear();
            foreach (IWaterZoneAware aware in insideThisCheck)
            {
                insideLastCheck.Add(aware);
            }
        }
    }
}
