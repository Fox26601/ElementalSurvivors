using UnityEngine;

/// <summary>
/// Single source of run elapsed time for spawning and future session-wide systems.
/// </summary>
public class GameRunSession : MonoBehaviour
{
    [SerializeField] private RunDifficultyProfileSO profile;

    public RunDifficultyProfileSO Profile => profile;

    public float ElapsedSeconds { get; private set; }

    public bool ShouldAllowSpawning
    {
        get
        {
            if (profile == null)
                return true;
            if (!profile.stopSpawningWhenSessionEnds)
                return true;
            return ElapsedSeconds < profile.sessionDurationSeconds;
        }
    }

    public event System.Action SessionEnded;

    private bool sessionEndedFired;

    private void Update()
    {
        ElapsedSeconds += Time.deltaTime;

        if (!sessionEndedFired && profile != null && ElapsedSeconds >= profile.sessionDurationSeconds)
        {
            sessionEndedFired = true;
            SessionEnded?.Invoke();
        }
    }
}
