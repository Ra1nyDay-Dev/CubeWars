using UnityEngine;

namespace Project.Scripts.Gameplay.SpawnSystems.Markers
{
    public abstract class SpawnMarker : MonoBehaviour
    {
        // Gizmos drawing in SpawnMarkerEditor
        public virtual Color GizmoColor => Color.red;
        
        public virtual bool DrawPositionSphere => true;
        public virtual float GizmoRadius => 1.5f;
        
        public virtual string GizmoTitle => "";
        public virtual float GizmoTitleHeight => 2.5f;
        
        public virtual bool DrawRotationArrow => true;
        public virtual float GizmoRotationArrowSize => 3f;
        
    }
}