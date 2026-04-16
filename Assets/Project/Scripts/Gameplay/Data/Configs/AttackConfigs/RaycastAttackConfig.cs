using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.WeaponSystems.AttackSystems;
using Project.Scripts.Gameplay.WeaponSystems.AttackSystems.Raycast;
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
        
        [Header("FireRate")]
        [Min(1f)] public float RoundsPerMinute = 600f;
        public override float AttackInterval => 60f / RoundsPerMinute;
        
        [Header("Spread")]
        public bool UseSpread = false;
        [Min(0)] public float SpreadFactor = 1f;
        
        [Header("Particle System")]
        [SerializeField] public ParticleSystem HitEffectPrefab;
        [SerializeField] public ParticleSystem MissEffectPrefab;
        [SerializeField, Min(0f)] public float HitEffectDestroyDelay = 2f;
        
        public override AttackBehaviour CreateAttack(
            Transform attackStartPoint,
            GameObject selfHitbox,
            WeaponType weaponType) =>
            new RaycastAttack(this, attackStartPoint, weaponType);
    }
}