using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.AttackConfigs
{
    [CreateAssetMenu(fileName = "RaycastAttackConfig", menuName = "Configs/AttacksBehaviour/RaycastAttack")]
    public class RaycastAttackConfig : AttackConfig
    {
        [Header("Ray")]
        public LayerMask LayerMask;
        [Min(0)] public float Distance = 30f;
        [Min(1)] public int ShotCount = 1;
        
        [Header("Spread")]
        public bool UseSpread = false;
        [Min(0)] public float SpreadFactor = 1f;
    }
}