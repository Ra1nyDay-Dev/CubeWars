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
        
        [Header("Search radius")]
        [Range(1f, 100f)] public float WeaponCheckRadius = 30f;
        
    }
}