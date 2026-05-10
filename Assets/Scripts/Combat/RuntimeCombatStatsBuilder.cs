using UnityEngine;

/// <summary>
/// Builds runtime <see cref="CombatStatsSO"/> instances scaled from a baseline template.
/// </summary>
public static class RuntimeCombatStatsBuilder
{
    /// <param name="combinedMultiplier">Difficulty and optional boss multiplier already combined.</param>
    public static CombatStatsSO CreateScaled(
        CombatStatsSO baseline,
        RunDifficultyProfileSO profile,
        float combinedMultiplier)
    {
        if (baseline == null)
            return null;

        var instance = ScriptableObject.CreateInstance<CombatStatsSO>();
        instance.name = baseline.name + "_Runtime";
        instance.maxHP = baseline.maxHP;
        instance.maxMP = baseline.maxMP;
        instance.attack = baseline.attack;
        instance.defense = baseline.defense;
        instance.speed = baseline.speed;

        if (profile == null || combinedMultiplier <= 0f)
            return instance;

        if (profile.scaleCombatMaxHP)
            instance.maxHP = Mathf.Max(1, Mathf.RoundToInt(baseline.maxHP * combinedMultiplier));
        if (profile.scaleCombatAttack)
            instance.attack = Mathf.Max(1, Mathf.RoundToInt(baseline.attack * combinedMultiplier));

        return instance;
    }
}
