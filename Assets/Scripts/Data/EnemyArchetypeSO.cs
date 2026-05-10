using UnityEngine;

[CreateAssetMenu(fileName = "EnemyArchetype", menuName = "Elemental Survivors/Run/Enemy Archetype")]
public class EnemyArchetypeSO : ScriptableObject
{
    public GameObject prefab;
    public CombatStatsSO combatBaseline;
    [Min(0.01f)] public float worldHealthBaseline = 25f;
}
