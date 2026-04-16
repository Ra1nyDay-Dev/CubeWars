using UnityEngine;

namespace Project.Scripts.Gameplay.SpawnSystems.Markers
{
    public class RespawnPointMarker : SpawnMarker
    {
        [SerializeField] private BoxCollider _enemyChekZoneTrigger;
        
        public BoxCollider EnemyCheckZone => _enemyChekZoneTrigger;
        
        public override Color GizmoColor => Color.softRed;
        public override float GizmoRadius => 1.7f;
        
        
#if UNITY_EDITOR       
        private void OnDrawGizmosSelected()
        {
            if (!_enemyChekZoneTrigger)
                return;
            
            Gizmos.color = new Color32(220, 49, 49, 130);
            Gizmos.DrawCube(transform.position + _enemyChekZoneTrigger.center, _enemyChekZoneTrigger.size);
        }
#endif
    }
}