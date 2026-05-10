using UnityEngine;

/// <summary>
/// Pure difficulty math from <see cref="RunDifficultyProfileSO"/>; no Unity time APIs.
/// </summary>
public static class RunDifficultyEvaluator
{
    public static int GetCompletedDifficultyTicks(RunDifficultyProfileSO profile, float elapsedSeconds)
    {
        if (profile == null || profile.difficultyTickIntervalSeconds <= 0f)
            return 0;

        float cappedElapsed = Mathf.Min(elapsedSeconds, profile.sessionDurationSeconds);
        int ticks = Mathf.FloorToInt(cappedElapsed / profile.difficultyTickIntervalSeconds);
        if (profile.maxDifficultyTicks > 0)
            ticks = Mathf.Min(ticks, profile.maxDifficultyTicks);
        return Mathf.Max(0, ticks);
    }

    /// <summary>Compound multiplier: multiplierPerTick ^ tickCount.</summary>
    public static float GetStatMultiplier(RunDifficultyProfileSO profile, float elapsedSeconds)
    {
        if (profile == null)
            return 1f;

        int n = GetCompletedDifficultyTicks(profile, elapsedSeconds);
        return Mathf.Pow(profile.difficultyMultiplierPerTick, n);
    }
}
