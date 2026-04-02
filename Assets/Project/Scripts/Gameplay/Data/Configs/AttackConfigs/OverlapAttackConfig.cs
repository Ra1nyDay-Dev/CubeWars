using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.AttackConfigs
{
    [CreateAssetMenu(fileName = "OverlapAttackConfig", menuName = "Configs/AttacksBehaviour/OverlapAttack")]
    public class OverlapAttackConfig : AttackConfig
    {
        [Header("Target mode")]
        public OverlapAttackTargetMode TargetMode = OverlapAttackTargetMode.All;
        [Min(1)] public int MaxTargetsPerAttack = 1;
        
        [Header("Masks")]
        public LayerMask SearchLayerMask;
        public LayerMask ObstacleLayerMask;
        
        [Header("Overlap Area")]
        public Vector3 Offset;
        [Min(0f)] public float SphereRadius = 1f;
        
        [Header("Obstacles")]
        public bool ConsiderObstacles;
        
        [Header("Gizmos")]
        public DrawGizmosType DrawGizmosType;
        public Color GizmosColor = Color.cyan;
    }
}