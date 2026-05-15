using UnityEngine;

[CreateAssetMenu(fileName = "RunDifficultyProfile", menuName = "Elemental Survivors/Run/Difficulty Profile")]
public class RunDifficultyProfileSO : ScriptableObject
{
    [Header("Session")]
    [Min(0f)] public float sessionDurationSeconds = 600f;
    public bool stopSpawningWhenSessionEnds = true;

    [Header("Difficulty ticks")]
    [Min(0.01f)] public float difficultyTickIntervalSeconds = 60f;
    [Min(0.01f)] public float difficultyMultiplierPerTick = 1.1f;
    [Tooltip("0 = no extra cap beyond session duration.")]
    public int maxDifficultyTicks;

    [Header("Boss schedule")]
    [Min(0.01f)] public float bossSpawnIntervalSeconds = 198f;
    [Min(0f)] public float firstBossDelaySeconds = 198f;
    [Min(0.01f)] public float bossExtraStatMultiplier = 10f;

    [Header("Scaling targets")]
    public bool scaleCombatMaxHP = true;
    public bool scaleCombatAttack = true;
    public bool scaleWorldHealth = true;
}
