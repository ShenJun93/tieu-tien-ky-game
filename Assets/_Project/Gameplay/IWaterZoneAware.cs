namespace TieuTienKy.Gameplay
{
    /// <summary>Implemented by anything WaterZone needs to mark wet/dry.</summary>
    public interface IWaterZoneAware
    {
        bool IsInWaterZone { get; }
        void SetInWaterZone(bool inWaterZone);
    }
}
