using System;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Gameplay.AttackSystems.DebugSystems
{
    public class RaycastAttackMB : AttackBehaviourMB
    {
        [SerializeField, Min(0f)] private float _damage = 10f;
        
        [SerializeField] private LayerMask _layerMask;
        [SerializeField, Min(0)] private float _distance = Mathf.Infinity;
        [SerializeField, Min(0)] private int _shotCount = 1;
        
        [SerializeField] private bool _useSpread;
        [SerializeField, Min(0)] private float _spreadFactor = 1f;

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Fire1");
                PerformAttack();
            }
        }


        [ContextMenu(nameof(PerformAttack))]
        public override void PerformAttack()
        {
            for (var i = 0; i < _shotCount; i++)
            {
                PerformRaycast();
            }
        }

        private void PerformRaycast()
        {
            var direction = _useSpread ? transform.forward + CalculateSpread() : transform.forward;
            var ray = new Ray(transform.position, direction);
            
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _distance, _layerMask))
            {
                var hitCollider = hitInfo.collider;
                Transform target = hitCollider.transform.parent;

                if (target != null && target.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(new(_damage, DamageSource.Weapon));
                }
                else
                {
                    // hit something not damageable
                }
            }
            
        }

        private Vector3 CalculateSpread()
        {
            return new Vector3
            {
                x = Random.Range(-_spreadFactor, _spreadFactor),
                y = Random.Range(-_spreadFactor, _spreadFactor),
                z = Random.Range(-_spreadFactor, _spreadFactor)
            };
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var ray = new Ray(transform.position, transform.forward);
            DrawRaycast(ray);
        }
        
        private void DrawRaycast(Ray ray)
        {
            if (Physics.Raycast(ray, out var hitInfo, _distance, _layerMask))
            {
                DrawRay(ray, hitInfo.point, hitInfo.distance, Color.red);
            }
            else
            {
                var hitPosition = ray.origin + ray.direction * _distance;
                
                DrawRay(ray, hitPosition, _distance, Color.green);
            }
        }
        
        private void DrawRay(Ray ray, Vector3 hitPosition, float distance, Color color)
        {
            const float hitPointRadius = 0.15f;

            Debug.DrawRay(ray.origin, ray.direction * distance, color);
            Gizmos.color = color;
            Gizmos.DrawSphere(hitPosition, hitPointRadius);
        }
#endif
    }
}