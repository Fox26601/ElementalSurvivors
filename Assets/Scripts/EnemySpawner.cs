using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RunDifficultyProfileSO runProfile;
    [SerializeField] private EnemyArchetypeSO gruntArchetype;
    [SerializeField] private EnemyArchetypeSO bossArchetype;
    [SerializeField] private GameRunSession session;

    [Header("Spawn cadence")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnRadius = 8f;

    private float gruntTimer;
    private float nextBossSpawnAtElapsed;
    private Transform player;

    private void Start()
    {
        gruntTimer = spawnInterval;
        player = PlayerController.Instance;

        if (runProfile != null)
            nextBossSpawnAtElapsed = runProfile.firstBossDelaySeconds;
    }

    private void Update()
    {
        if (gruntArchetype == null || gruntArchetype.prefab == null)
            return;

        if (session != null && !session.ShouldAllowSpawning)
            return;

        float elapsed = session != null ? session.ElapsedSeconds : 0f;

        gruntTimer -= Time.deltaTime;
        if (gruntTimer <= 0f)
        {
            SpawnGrunt(elapsed);
            gruntTimer = spawnInterval;
        }

        if (runProfile != null && session != null && TryConsumeBossSpawn(elapsed))
            SpawnBoss(elapsed);
    }

    private bool TryConsumeBossSpawn(float elapsed)
    {
        if (elapsed < nextBossSpawnAtElapsed)
            return false;

        nextBossSpawnAtElapsed += runProfile.bossSpawnIntervalSeconds;
        return true;
    }

    private void SpawnGrunt(float elapsed)
    {
        SpawnFromArchetype(gruntArchetype, elapsed, isBoss: false);
    }

    private void SpawnBoss(float elapsed)
    {
        EnemyArchetypeSO arch = bossArchetype != null ? bossArchetype : gruntArchetype;
        if (arch == null || arch.prefab == null)
            return;

        SpawnFromArchetype(arch, elapsed, isBoss: true);
    }

    private void SpawnFromArchetype(EnemyArchetypeSO archetype, float elapsed, bool isBoss)
    {
        if (player == null)
            player = PlayerController.Instance;
        if (player == null || archetype == null || archetype.combatBaseline == null)
            return;

        float difficultyMul = runProfile != null
            ? RunDifficultyEvaluator.GetStatMultiplier(runProfile, elapsed)
            : 1f;
        float bossMul = isBoss && runProfile != null ? runProfile.bossExtraStatMultiplier : 1f;
        float combined = difficultyMul * bossMul;

        float worldHp = archetype.worldHealthBaseline;
        if (runProfile == null || runProfile.scaleWorldHealth)
            worldHp *= combined;
        else if (isBoss)
            worldHp *= bossMul;

        CombatStatsSO runtimeStats = RuntimeCombatStatsBuilder.CreateScaled(
            archetype.combatBaseline,
            runProfile,
            combined);

        var bundle = new SpawnStatBundle(worldHp, runtimeStats, isBoss);

        Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        GameObject instance = Instantiate(archetype.prefab, spawnPos, Quaternion.identity);
        var ai = instance.GetComponent<EnemyAI>();
        if (ai != null)
            ai.ApplySpawnConfiguration(bundle);
    }
}
