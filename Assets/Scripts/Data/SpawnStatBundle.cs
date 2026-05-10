using UnityEngine;

/// <summary>
/// Runtime spawn payload: scaled world HP and combat stats instance owned by the enemy.
/// </summary>
public readonly struct SpawnStatBundle
{
    public SpawnStatBundle(float worldMaxHealth, CombatStatsSO runtimeCombatStats, bool isBoss)
    {
        WorldMaxHealth = worldMaxHealth;
        RuntimeCombatStats = runtimeCombatStats;
        IsBoss = isBoss;
    }

    public float WorldMaxHealth { get; }
    public CombatStatsSO RuntimeCombatStats { get; }
    public bool IsBoss { get; }
}
