using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Gameplay.Data.Configs.AI
{
    [CreateAssetMenu(fileName = "AiBotConfig", menuName = "Configs/AI/Bot")]
    public class AiBotConfig : ScriptableObject
    {
        [Header("Patrol")]
        [Range(1f, 200f)] public float PatrolRadius = 30f;  // for RandomPatrolPointProvider
        [Range(0.1f, 5f)] public float PatrolSampleDistance = 5f; // for RandomPatrolPointProvider
        [Range(0.1f, 5f)] public float PatrolArriveDistance = 0.8f;
        [Range(0f, 10f)]  public float PatrolIdleTime = 0.1f;
 
        [Header("Stuck detection")]
        [Range(0.2f, 5f)]  public float StuckCheckInterval = 1.2f;
        [Range(0.05f, 5f)] public float StuckDistanceThreshold = 0.5f;
    }
}