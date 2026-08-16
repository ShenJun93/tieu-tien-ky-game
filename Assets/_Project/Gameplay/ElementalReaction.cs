namespace TieuTienKy.Gameplay
{
    public enum DamageElement
    {
        Physical,
        Lightning
    }

    /// <summary>
    /// Hardcoded, deliberately non-generic rule for the single P0A
    /// elemental micro-reaction: Water Zone + Lightning Hit -> Conductive Burst.
    /// Do not extend this into a generic reaction graph/matrix in P0A.
    /// </summary>
    public static class ElementalReaction
    {
        public static bool TryTriggerConductiveBurst(bool targetInWaterZone, DamageElement incomingElement)
        {
            return targetInWaterZone && incomingElement == DamageElement.Lightning;
        }
    }
}
