using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>Single-attack hit payload. No combo/progression fields by design.</summary>
    public readonly struct HitInfo
    {
        public readonly int Damage;
        public readonly DamageElement Element;
        public readonly Vector3 KnockbackImpulse;

        public HitInfo(int damage, DamageElement element, Vector3 knockbackImpulse)
        {
            Damage = damage;
            Element = element;
            KnockbackImpulse = knockbackImpulse;
        }
    }
}
