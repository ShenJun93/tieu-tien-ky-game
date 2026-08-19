namespace TieuTienKy.Gameplay
{
    public readonly struct GaleCounterSpec
    {
        public readonly float DashDistanceMultiplier;
        public readonly float PushRadius;
        public readonly float PushImpulse;

        public GaleCounterSpec(float dashDistanceMultiplier, float pushRadius, float pushImpulse)
        {
            DashDistanceMultiplier = dashDistanceMultiplier;
            PushRadius = pushRadius;
            PushImpulse = pushImpulse;
        }
    }

    /// <summary>
    /// Deterministic Product-Proof interpretation of the existing three
    /// run-local Cơ Duyên axes. No generic proc/modifier graph is introduced.
    /// </summary>
    public readonly struct ProductProofRunStyle
    {
        public readonly bool StormControlActive;
        public readonly float StormPulseRadius;
        public readonly float StormPulseImpulse;
        public readonly bool WindWardActive;
        public readonly GaleCounterSpec GaleCounter;

        ProductProofRunStyle(bool stormControlActive, float stormPulseRadius, float stormPulseImpulse, bool windWardActive, GaleCounterSpec galeCounter)
        {
            StormControlActive = stormControlActive;
            StormPulseRadius = stormPulseRadius;
            StormPulseImpulse = stormPulseImpulse;
            WindWardActive = windWardActive;
            GaleCounter = galeCounter;
        }

        public static ProductProofRunStyle FromStacks(int thunderStacks, int windStacks, int wardStacks)
        {
            int thunder = ClampStacks(thunderStacks);
            int wind = ClampStacks(windStacks);
            int ward = ClampStacks(wardStacks);

            bool stormActive = thunder > 0;
            float stormRadius = stormActive ? 2.1f + 0.25f * (thunder - 1) : 0f;
            float stormImpulse = stormActive ? 5.5f + 1.25f * thunder : 0f;

            bool windWardActive = wind > 0 && ward > 0;
            GaleCounterSpec galeCounter = windWardActive
                ? new GaleCounterSpec(1.2f + 0.05f * wind, 1.8f + 0.15f * ward, 5f + 0.75f * (wind + ward))
                : new GaleCounterSpec(1f, 0f, 0f);

            return new ProductProofRunStyle(stormActive, stormRadius, stormImpulse, windWardActive, galeCounter);
        }

        /// <summary>
        /// Runtime bridge for existing scalar setters. Product Proof behavior
        /// turns on at the first investment; exact stack counts remain owned by
        /// RunBlessingState and are still used for its legacy scalar tuning.
        /// </summary>
        public static ProductProofRunStyle FromInvestments(bool thunderInvested, bool windInvested, bool wardInvested)
        {
            return FromStacks(thunderInvested ? 1 : 0, windInvested ? 1 : 0, wardInvested ? 1 : 0);
        }

        static int ClampStacks(int value) => System.Math.Max(0, System.Math.Min(3, value));
    }

    public sealed class WindWardComboState
    {
        ProductProofRunStyle style;
        bool primed;

        public bool IsPrimed => primed;

        public void Configure(ProductProofRunStyle runStyle)
        {
            style = runStyle;
            if (!style.WindWardActive)
            {
                primed = false;
            }
        }

        public bool RecordWardBlock()
        {
            if (!style.WindWardActive)
            {
                return false;
            }

            primed = true;
            return true;
        }

        public bool TryConsume(out GaleCounterSpec spec)
        {
            if (!primed || !style.WindWardActive)
            {
                spec = default;
                return false;
            }

            primed = false;
            spec = style.GaleCounter;
            return true;
        }
    }

    /// <summary>Pure layout values so the mobile thumb cluster is testable without a Canvas.</summary>
    public readonly struct ProductProofHudButtonSpec
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Size;
        public readonly int FontSize;

        public ProductProofHudButtonSpec(float x, float y, float size, int fontSize)
        {
            X = x;
            Y = y;
            Size = size;
            FontSize = fontSize;
        }

        public bool Overlaps(ProductProofHudButtonSpec other)
        {
            float halfSum = (Size + other.Size) * 0.5f;
            return System.Math.Abs(X - other.X) < halfSum && System.Math.Abs(Y - other.Y) < halfSum;
        }
    }

    public static class ProductProofHudLayout
    {
        public static readonly ProductProofHudButtonSpec LoiTram = new ProductProofHudButtonSpec(-130f, 155f, 176f, 30);
        public static readonly ProductProofHudButtonSpec PhongBo = new ProductProofHudButtonSpec(-315f, 135f, 148f, 26);
        public static readonly ProductProofHudButtonSpec HoThe = new ProductProofHudButtonSpec(-235f, 315f, 148f, 26);
    }
}
