using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs;
using Project.Scripts.Gameplay.Data.Configs.AttackConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Gameplay.AttackSystems.Raycast
{
    public class RaycastAttack : AttackBehaviour
    {
        private LayerMask _layerMask;
        private readonly float _distance;
        private readonly int _shotCount;
        
        private readonly bool _useSpread;
        private readonly float _spreadFactor;

        private readonly Transform _startPoint;

        public RaycastAttack(RaycastAttackConfig config, Transform startPoint, WeaponType weaponType)
        {
            AttackAnimationsCount = config.AttackAnimationsCount;
            Damage = config.Damage;
            AttackCooldown = config.AttackCooldown;
            AttackDelay = config.AttackDelay;
            HorizontalForceOnHit = config.HorizontalForceOnHit;
            VerticalForceOnHit = config.VerticalForceOnHit;
            AttackDeathType = config.AnimationOnDeath;
            WeaponType = weaponType;
            _startPoint = startPoint;
            _layerMask = config.LayerMask;
            _distance = config.Distance;
            _shotCount = config.ShotCount;
            _useSpread = config.UseSpread;
            _spreadFactor = config.SpreadFactor;
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
                Transform target = hitCollider.transform.parent;

                if (target != null && target.TryGetComponent(out IDamageable damageable))
                    ApplyDamage(damageable, target.transform);
                else
                {
                    // hit something not damageable
                }
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
    }
}