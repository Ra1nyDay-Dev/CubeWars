using System.Collections.Generic;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Data.Configs;
using Project.Scripts.Gameplay.Data.Enums;
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
        private readonly List<(IDamageable damageable, float distanceToTarget)> _validTargets = new();
        private readonly GameObject _selfHitbox;

        public OverlapAttack(OverlapAttackConfig config, Transform startPoint, GameObject selfHitbox)
        {
            Damage = config.Damage;
            AttackCooldown = config.AttackCooldown;
            AttackDelay = config.AttackDelay;
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
                        : _maxTargetsPerAttack;
                    
                    for (int i = 0; i < targetsToAttack; i++) 
                        ApplyDamage(_validTargets[i].damageable, Damage);
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
                if (_selfHitbox ==_overlapResults[i].gameObject)
                    continue;
                    
                Transform target = _overlapResults[i].transform.parent;
                
                if (target == null || target.TryGetComponent(out IDamageable damageable) == false)
                    continue;
            
                if (_considerObstacles)
                {
                    if (HasObstacleOnTheWay(_overlapResults[i].transform.position))
                        continue;
                }

                _validTargets.Add((damageable, DistanceToTarget(target)));
            }
            
            return _validTargets.Count > 0;
        }

        private float DistanceToTarget(Transform target)
        {
            return Vector3.Distance(_overlapStartPoint.position, target.position);
        }

        private bool HasObstacleOnTheWay(Vector3 targetPosition) => 
            Physics.Linecast(_overlapStartPoint.position, targetPosition, _obstacleLayerMask.value);

        private void SortTargetsByDistance()
        {
            _validTargets.Sort((a, b) => 
                a.distanceToTarget.CompareTo(b.distanceToTarget));
        }

        private void ApplyDamage(IDamageable target, float damage) => 
            target.TakeDamage(damage);
        
    }
}