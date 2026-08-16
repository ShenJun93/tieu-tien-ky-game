using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Trigger volume that marks/unmarks IWaterZoneAware entities. One clear
    /// state/tag per the P0A spec; not a generic zone/status framework.
    /// </summary>
    public sealed class WaterZone : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            var aware = other.GetComponentInParent<IWaterZoneAware>();
            aware?.SetInWaterZone(true);
        }

        void OnTriggerExit(Collider other)
        {
            var aware = other.GetComponentInParent<IWaterZoneAware>();
            aware?.SetInWaterZone(false);
        }
    }
}
