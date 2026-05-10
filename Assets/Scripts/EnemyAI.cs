using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject expOrb;
    [SerializeField] private CombatStatsSO combatStats;

    [Header("Settings")]
    [SerializeField] private float maxHealth = 25f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float moveSpeed = 5f;

    private CombatStatsSO runtimeCombatStatsInstance;
    private PlayerHealth playerRef;
    private Transform player;
    private Vector2 direction;
    private float currentHealth;

    public CombatStatsSO CombatStats => combatStats;

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    void Start()
    {
        player = PlayerController.Instance;

        if (player != null && playerRef == null)
            playerRef = player.GetComponent<PlayerHealth>();
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRef != null)
                playerRef.TakeDamage(damage);
        }
    }

    private void SetDirection()
    {
        if (player == null)
            return;
        direction = (player.position - transform.position).normalized;
    }

    private void FollowPlayer()
    {
        SetDirection();
        rb.linearVelocity = direction * moveSpeed;
    }

    /// <summary>Called immediately after Instantiate to apply scaled stats from the spawner.</summary>
    public void ApplySpawnConfiguration(SpawnStatBundle bundle)
    {
        if (runtimeCombatStatsInstance != null)
        {
            Destroy(runtimeCombatStatsInstance);
            runtimeCombatStatsInstance = null;
        }

        combatStats = bundle.RuntimeCombatStats;
        runtimeCombatStatsInstance = bundle.RuntimeCombatStats;

        maxHealth = bundle.WorldMaxHealth;
        currentHealth = maxHealth;

        if (bundle.RuntimeCombatStats != null)
            damage = Mathf.Max(1f, bundle.RuntimeCombatStats.attack);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        Debug.Log($"Took {amount} damage\nNew HP: {currentHealth}");

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        if (expOrb != null)
            Instantiate(expOrb, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (runtimeCombatStatsInstance != null)
        {
            Destroy(runtimeCombatStatsInstance);
            runtimeCombatStatsInstance = null;
        }
    }
}
