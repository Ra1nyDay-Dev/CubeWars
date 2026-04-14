using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs.AttackConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Gameplay.AttackSystems.Raycast
{
    public class RaycastAttack : AttackBehaviour
    {
        private readonly LayerMask _layerMask;
        private readonly float _distance;
        private readonly int _shotCount;
        
        private readonly bool _useSpread;
        private readonly float _spreadFactor;
        
        private readonly ParticleSystem _hitEffectPrefab; // toDo: move to ParticleService
        private readonly ParticleSystem _missEffectPrefab;
        private readonly float _hitEffectDestroyDelay;
        
        private readonly Transform _startPoint;

        public RaycastAttack(RaycastAttackConfig config, Transform startPoint, WeaponType weaponType)
        {
            AttackAnimationsCount = config.AttackAnimationsCount;
            Damage = config.Damage;
            AttackInterval = config.AttackInterval;
            AttackDelay = config.AttackDelay;
            HorizontalForceOnHit = config.HorizontalForceOnHit;
            VerticalForceOnHit = config.VerticalForceOnHit;
            AttackDeathType = config.AnimationOnDeath;
            WeaponType = weaponType;
            HoldingButtonContinuesAttack = config.HoldingButtonContinuesAttack;
            
            _startPoint = startPoint;
            _layerMask = config.LayerMask;
            _distance = config.Distance;
            _shotCount = config.ShotCount;
            _useSpread = config.UseSpread;
            _spreadFactor = config.SpreadFactor;
            _hitEffectPrefab = config.HitEffectPrefab;
            _missEffectPrefab = config.MissEffectPrefab;
            _hitEffectDestroyDelay = config.HitEffectDestroyDelay;
        }
        
        public override void PerformAttack()
        {
            for (var i = 0; i < _shotCount; i++) 
                PerformRaycast();
        }

        private void PerformRaycast()
        {
            var direction = _useSpread ? _startPoint.forward + CalculateSpread() : _startPoint.forward;
            var ray = new Ray(_startPoint.position, direction);
            
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _distance, _layerMask))
            {
                var hitCollider = hitInfo.collider;
                Transform target = hitCollider.transform.root;

                if (target != null && target.TryGetComponent(out IDamageable damageable))
                {
                    SpawnParticleEffectOnHit(hitInfo, _hitEffectPrefab);
                    ApplyDamage(damageable, target.transform);
                }
                else
                    SpawnParticleEffectOnHit(hitInfo, _missEffectPrefab);
            }
        }

        private void ApplyDamage(IDamageable target, Transform targetTransform)
        {
            DamageData damageData = new DamageData(
                Damage,
                DamageSource.Weapon,
                AttackDeathType,
                WeaponType,
                GetHitDirection(targetTransform),
                HorizontalForceOnHit,
                VerticalForceOnHit
            );
            target.TakeDamage(damageData);
        }

        private Vector3 GetHitDirection(Transform targetTransform) => 
            (targetTransform.position - _startPoint.transform.position).normalized;

        private Vector3 CalculateSpread()
        {
            return new Vector3
            {
                x = Random.Range(-_spreadFactor, _spreadFactor),
                y = Random.Range(-_spreadFactor, _spreadFactor),
                z = Random.Range(-_spreadFactor, _spreadFactor)
            };
        }
        
        // toDo: rewrite to object pool
        private void SpawnParticleEffectOnHit(RaycastHit hitInfo, ParticleSystem hitEffectPrefab)
        {
            if (hitEffectPrefab != null)
            {
                var hitEffectRotation = Quaternion.LookRotation(hitInfo.normal);
                var hitEffect = Object.Instantiate(hitEffectPrefab, hitInfo.point, hitEffectRotation);
                    
                Object.Destroy(hitEffect.gameObject, _hitEffectDestroyDelay);
            }
        }
    }
}