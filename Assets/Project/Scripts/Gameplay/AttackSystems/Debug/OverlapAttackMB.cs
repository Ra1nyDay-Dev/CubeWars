using System.Collections.Generic;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.AttackSystems.Overlap
{
    public class OverlapAttackMB : AttackBehaviourMB
    {
        [SerializeField, Min(0f)] protected float _damage;
        [SerializeField] protected OverlapAttackTargetMode _targetMode = OverlapAttackTargetMode.All;
        [SerializeField, Min(1)] protected int _maxTargetsPerAttack = 1;
        
        [SerializeField] protected LayerMask _searchLayerMask;
        [SerializeField] protected LayerMask _obstacleLayerMask;
        
        [SerializeField] protected Transform _overlapStartPoint;
        [SerializeField] protected Vector3 _offset;
        [SerializeField, Min(0f)] protected float _sphereRadius = 1f;
        
        [SerializeField] protected bool _considerObstacles;
        
        [SerializeField] protected DrawGizmosType _drawGizmosType;
        [SerializeField] protected Color _gizmosColor = Color.cyan;
        
        private int _overlapResultsCount;
        private readonly Collider[] _overlapResults = new Collider[32];
        private readonly List<(IDamageable damageable, float distanceToTarget)> _validTargets = new();
        
        [ContextMenu(nameof(PerformAttack))]
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
                        ApplyDamage(_validTargets[i].damageable, _damage);
                }
            }
        }

        protected virtual bool TryFindHitBoxes()
        {
            var position = _overlapStartPoint.TransformPoint(_offset);
            _overlapResultsCount = Physics.OverlapSphereNonAlloc(position, _sphereRadius, _overlapResults, _searchLayerMask.value);
            return _overlapResultsCount > 0;
        }

        protected virtual bool TryFindValidTargets()
        {
            _validTargets.Clear();
            
            for (int i = 0; i < _overlapResultsCount; i++)
            {
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

        protected virtual float DistanceToTarget(Transform target)
        {
            return Vector3.Distance(_overlapStartPoint.position, target.position);
        }

        protected virtual bool HasObstacleOnTheWay(Vector3 targetPosition) => 
            Physics.Linecast(_overlapStartPoint.position, targetPosition, _obstacleLayerMask.value);

        protected virtual void SortTargetsByDistance()
        {
            _validTargets.Sort((a, b) => 
                a.distanceToTarget.CompareTo(b.distanceToTarget));
        }

        protected virtual void ApplyDamage(IDamageable target, float damage) => 
            target.TakeDamage(damage, Vector3.forward);

#if UNITY_EDITOR

        private void OnDrawGizmos() => 
            TryDrawGizmos(DrawGizmosType.Always);

        private void OnDrawGizmosSelected() => 
            TryDrawGizmos(DrawGizmosType.OnSelected);

        private void TryDrawGizmos(DrawGizmosType requiredType)
        {
            if (_drawGizmosType != requiredType)
                return;
            
            if (_overlapStartPoint == null)
                return;

            Gizmos.matrix = _overlapStartPoint.localToWorldMatrix;
            Gizmos.color = _gizmosColor;
            Gizmos.DrawSphere(_offset, _sphereRadius);
        }
#endif
    }
}