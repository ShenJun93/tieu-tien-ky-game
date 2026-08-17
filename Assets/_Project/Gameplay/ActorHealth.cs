namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Pure health/defeat state shared by player and enemies. No
    /// MonoBehaviour/Time dependency so it is unit-testable without a scene.
    /// </summary>
    public sealed class ActorHealth
    {
        public ActorHealth(int maxHealth)
        {
            SetMaxHealthAndRestore(maxHealth);
        }

        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsDefeated => CurrentHealth <= 0;

        /// <summary>Returns true exactly on the hit that brings health to zero.</summary>
        public bool ApplyDamage(int amount)
        {
            if (amount <= 0 || IsDefeated) return false;
            CurrentHealth = System.Math.Max(0, CurrentHealth - amount);
            return CurrentHealth == 0;
        }

        public void SetMaxHealthAndRestore(int maxHealth)
        {
            MaxHealth = System.Math.Max(1, maxHealth);
            CurrentHealth = MaxHealth;
        }

        public void RestoreToFull()
        {
            CurrentHealth = MaxHealth;
        }
    }
}
