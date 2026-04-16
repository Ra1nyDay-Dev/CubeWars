using Project.Scripts.Gameplay.Data.Configs.AttackConfigs;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.WeaponSystems.AttackSystems.DebugSystems
{
    public class OverlapAttackDebugDrawer : MonoBehaviour
    {
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private Transform _overlapStartPoint;
        [SerializeField] private bool _showPrimaryGizmos;
        [SerializeField] private bool _showSecondaryGizmos;
        
        private OverlapAttackConfig _primaryAttackConfig;
        private OverlapAttackConfig _secondaryAttackConfig;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _primaryAttackConfig = _weaponConfig.PrimaryAttackBehaviourConfig as OverlapAttackConfig;
            _secondaryAttackConfig = _weaponConfig.SecondaryAttackBehaviourConfig as OverlapAttackConfig;
            
            if (_showPrimaryGizmos)
                TryDrawGizmos(DrawGizmosType.Always, _primaryAttackConfig);
            
            if (_showSecondaryGizmos)
                TryDrawGizmos(DrawGizmosType.Always, _secondaryAttackConfig);
        }

        private void OnDrawGizmosSelected()
        {
            if (_showPrimaryGizmos)
                TryDrawGizmos(DrawGizmosType.OnSelected, _primaryAttackConfig);
            
            if (_showSecondaryGizmos)
                TryDrawGizmos(DrawGizmosType.OnSelected, _secondaryAttackConfig);
        }

        private void TryDrawGizmos(DrawGizmosType requiredType, OverlapAttackConfig config)
        {
            if (config.DrawGizmosType != requiredType)
                return;
            
            if (_overlapStartPoint == null)
                return;

            Gizmos.matrix = _overlapStartPoint.localToWorldMatrix;
            Gizmos.color = config.GizmosColor;
            Gizmos.DrawSphere(config.Offset, config.SphereRadius);
        }
    }
#endif
}