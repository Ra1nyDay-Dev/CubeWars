using UnityEngine;

namespace Project.Scripts.Gameplay.Common
{
    public class RespawnPointMarker : SpawnMarker
    {
        [SerializeField] BoxCollider _respawnZoneTrigger;
        
        public override Color GizmoColor => Color.softRed;
        public override float GizmoRadius => 1.7f;
        
        
#if UNITY_EDITOR       
        private void OnDrawGizmos()
        {
            if (!_respawnZoneTrigger)
                return;
            
            Gizmos.color = new Color32(220, 49, 49, 130);
            Gizmos.DrawCube(transform.position + _respawnZoneTrigger.center, _respawnZoneTrigger.size);
        }
#endif
    }
}