namespace TieuTienKy.Gameplay
{
    public enum EnemyArchetype
    {
        Pursuer,
        Lancer
    }

    /// <summary>Narrow concrete Pursuer/Lancer tuning data - not a generic AI definition system.</summary>
    public readonly struct EnemyCombatProfile
    {
        public readonly EnemyArchetype Archetype;
        public readonly float ChaseSpeed;
        public readonly float StoppingDistance;
        public readonly float AttackRange;
        public readonly float TelegraphSeconds;
        public readonly float RecoverySeconds;
        public readonly int Damage;
        public readonly float KnockbackMagnitude;
        public readonly float LungeDistance;

        public EnemyCombatProfile(
            EnemyArchetype archetype,
            float chaseSpeed,
            float stoppingDistance,
            float attackRange,
            float telegraphSeconds,
            float recoverySeconds,
            int damage,
            float knockbackMagnitude,
            float lungeDistance)
        {
            Archetype = archetype;
            ChaseSpeed = chaseSpeed;
            StoppingDistance = stoppingDistance;
            AttackRange = attackRange;
            TelegraphSeconds = telegraphSeconds;
            RecoverySeconds = recoverySeconds;
            Damage = damage;
            KnockbackMagnitude = knockbackMagnitude;
            LungeDistance = lungeDistance;
        }

        public static EnemyCombatProfile Pursuer(float chaseSpeedMultiplier = 1f) => new EnemyCombatProfile(
            EnemyArchetype.Pursuer,
            chaseSpeed: 2.8f * chaseSpeedMultiplier,
            stoppingDistance: 1.35f,
            attackRange: 1.55f,
            telegraphSeconds: 0.35f,
            recoverySeconds: 0.65f,
            damage: 1,
            knockbackMagnitude: 3.5f,
            lungeDistance: 0f);

        public static EnemyCombatProfile Lancer(float chaseSpeedMultiplier = 1f) => new EnemyCombatProfile(
            EnemyArchetype.Lancer,
            chaseSpeed: 2.25f * chaseSpeedMultiplier,
            stoppingDistance: 2.6f,
            attackRange: 3.2f,
            telegraphSeconds: 0.65f,
            recoverySeconds: 1.0f,
            damage: 1,
            knockbackMagnitude: 5f,
            lungeDistance: 2.3f);
    }
}
