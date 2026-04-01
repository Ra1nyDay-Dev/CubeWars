using System.Collections.Generic;
using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.Weapons;
using UnityEngine;

namespace Project.Scripts.Gameplay.AttackSystems.Overlap
{
    public class OverlapAttack : AttackBehaviour
    {
        private readonly OverlapAttackTargetMode _targetMode;
        private readonly int _maxTargetsPerAttack;
        
        private LayerMask _searchLayerMask;
        private LayerMask _obstacleLayerMask;
        
        private readonly Transform _overlapStartPoint;
        private readonly Vector3 _offset;
        private readonly float _sphereRadius;

        private readonly bool _considerObstacles;

        private int _overlapResultsCount;
        private readonly Collider[] _overlapResults = new Collider[32];
        private readonly List<(IDamageable damageable, Transform transform, float distanceToTarget)> _validTargets = new();
        private readonly GameObject _selfHitbox;

        public OverlapAttack(OverlapAttackConfig config, Transform startPoint, GameObject selfHitbox, WeaponType weaponType)
        {
            AttackAnimationsCount = config.AttackAnimationsCount;
            Damage = config.Damage;
            AttackCooldown = config.AttackCooldown;
            AttackDelay = config.AttackDelay;
            HorizontalForceOnHit = config.HorizontalForceOnHit;
            VerticalForceOnHit = config.VerticalForceOnHit;
            AttackDeathType = config.AnimationOnDeath;
            WeaponType = weaponType;
            _targetMode = config.TargetMode;
            _maxTargetsPerAttack = config.MaxTargetsPerAttack;
            _searchLayerMask = config.SearchLayerMask;
            _obstacleLayerMask = config.ObstacleLayerMask;
            _overlapStartPoint = startPoint;
            _offset = config.Offset;
            _sphereRadius = config.SphereRadius;
            _considerObstacles = config.ConsiderObstacles;
            _selfHitbox = selfHitbox;
        }
        
        public override void PerformAttack()
        {
            if (TryFindHitBoxes())
            {
                if (TryFindValidTargets())
                {
                    SortTargetsByDistance();

                    int targetsToAttack = _targetMode == OverlapAttackTargetMode.All
                        ? _validTargets.Count
                        : Mathf.Min(_maxTargetsPerAttack, _validTargets.Count);

                    
                    
                    for (int i = 0; i < targetsToAttack; i++)
                    {
                        
                        
                        ApplyDamage(_validTargets[i].damageable, _validTargets[i].transform);
                    }
                }
            }
        }

        private bool TryFindHitBoxes()
        {
            var position = _overlapStartPoint.TransformPoint(_offset);
            _overlapResultsCount = Physics.OverlapSphereNonAlloc(position, _sphereRadius, _overlapResults, _searchLayerMask.value);
            return _overlapResultsCount > 0;
        }

        private bool TryFindValidTargets()
        {
            _validTargets.Clear();
            
            for (int i = 0; i < _overlapResultsCount; i++)
            {
                Collider hitbox = _overlapResults[i];
                
                if (_selfHitbox == hitbox.gameObject)
                    continue;
                
                Transform target = hitbox.transform.parent;
                
                if (target == null || target.TryGetComponent(out IDamageable damageable) == false)
                    continue;
            
                if (_considerObstacles && HasObstacleOnTheWay(hitbox.transform.position))
                    continue;

                _validTargets.Add((damageable, target, DistanceToTarget(target)));
            }
            
            return _validTargets.Count > 0;
        }

        private float DistanceToTarget(Transform target) => 
            Vector3.Distance(_overlapStartPoint.position, target.position);

        private bool HasObstacleOnTheWay(Vector3 targetPosition) => 
            Physics.Linecast(_overlapStartPoint.position, targetPosition, _obstacleLayerMask.value);

        private void SortTargetsByDistance()
        {
            _validTargets.Sort((a, b) => 
                a.distanceToTarget.CompareTo(b.distanceToTarget));
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
            (targetTransform.position - _overlapStartPoint.transform.position).normalized;

        // private void ApplyHitReactionToTarget(Transform targetTransform)
        // {
        //     IReactable reactable = targetTransform.GetComponent<IReactable>();
        //
        //     if (reactable != null)
        //     {
        //         Vector3 hitDirection = GetHitDirection(targetTransform);
        //         reactable.GetHitForce(hitDirection, HorizontalForceOnHit, VerticalForceOnHit);
        //     }
        // }

        
    }
}