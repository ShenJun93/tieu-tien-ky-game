using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>Pure damage-reduction math for Hộ Thể's defensive window. multiplier01: 0 = full block, 1 = no mitigation.</summary>
    public static class DamageMitigation
    {
        public static int Apply(int rawDamage, float multiplier01)
        {
            float clamped = Mathf.Clamp01(multiplier01);
            return Mathf.Max(0, Mathf.RoundToInt(rawDamage * clamped));
        }
    }
}
