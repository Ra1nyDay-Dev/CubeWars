using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Gameplay.Data.Configs.AI
{
    [CreateAssetMenu(fileName = "BotConfig", menuName = "Configs/AI/Bot")]
    public class BotConfig : ScriptableObject
    {
        [Header("Patrol")]
        [Range(1f, 200f)] public float PointSearchRadius = 30f;
        [Range(0.1f, 5f)] public float PatrolSampleDistance = 5f; // for RandomPatrolPointProvider
        [Range(0.1f, 5f)] public float PatrolArriveDistance = 1f;
 
        [Header("Stuck detection")]
        [Range(0.2f, 5f)]  public float StuckCheckInterval = 2f;
        [Range(0.05f, 5f)] public float StuckDistanceThreshold = 1f;
        
        [Header("Weapon search radius")]
        [Range(1f, 100f)] public float WeaponCheckRadius = 30f;
        
        [Header("Enemy detection")]
        [Range(1f, 200f)] public float EnemyDetectionRadius = 30f;
        [Range(0.1f, 50f)] public float MaxVerticalDelta = 3f;
        
        [Header("Combat — LineOfSight")]
        public LayerMask AttackObstacleLayerMask;
        [Range(0.1f, 5f)] public float EyeHeight = 1f;
 
        [Header("Combat — Melee")]
        [Range(0.3f, 5f)] public float MeleeAttackRange = 1.8f;
        [Range(0.3f, 5f)] public float MeleeApproachMinRadius = 0.5f;
        [Range(0.3f, 10f)] public float MeleeApproachMaxRadius = 1.5f;
 
        [Header("Combat — Ranged")]
        [Range(2f, 100f)] public float RangedAttackRange = 25f;
        [Range(2f, 50f)] public float RangedApproachMinRadius = 8f;
        [Range(2f, 100f)] public float RangedApproachMaxRadius = 15f;
 
        [Header("Combat — Common")]
        [Range(0.2f, 10f)] public float CombatRepositionInterval = 2f;
        [Range(0.5f, 5f)] public float CombatSampleDistance = 3f;
    }
}