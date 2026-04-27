using Project.Scripts.Gameplay.Data.Configs.AttackConfigs;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using UnityEngine;

namespace Project.Scripts.Gameplay.WeaponSystems.AttackSystems.DebugSystems
{
    public class RaycastAttackDebugDrawer : MonoBehaviour
    {
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private bool _showPrimaryGizmos;
        [SerializeField] private bool _showSecondaryGizmos;
        
        private RaycastAttackConfig _primaryAttackConfig;
        private RaycastAttackConfig _secondaryAttackConfig;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _primaryAttackConfig = _weaponConfig.PrimaryAttackBehaviourConfig as RaycastAttackConfig;
            _secondaryAttackConfig = _weaponConfig.SecondaryAttackBehaviourConfig as RaycastAttackConfig;
            
            var ray = new Ray(_startPoint.position, transform.forward);
            
            if (_showPrimaryGizmos)
                DrawRaycast(ray, _primaryAttackConfig);
            
            if (_showSecondaryGizmos)
                DrawRaycast(ray, _secondaryAttackConfig);
        }
        
        private void DrawRaycast(Ray ray, RaycastAttackConfig  config)
        {
            if (Physics.Raycast(ray, out var hitInfo, config.Distance, config.LayerMask))
            {
                DrawRay(ray, hitInfo.point, hitInfo.distance, Color.red);
            }
            else
            {
                var hitPosition = ray.origin + ray.direction * config.Distance;
                
                DrawRay(ray, hitPosition, config.Distance, Color.green);
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