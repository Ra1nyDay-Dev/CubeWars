using Project.Scripts.Gameplay.Weapons.WeaponSpawn;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Gameplay.Common
{
    public abstract class SpawnMarker : MonoBehaviour
    {
        // Gizmos drawing in SpawnMarkerEditor
        public virtual Color GizmoColor { get; protected set; } = Color.red;
        
        public virtual bool DrawPositionSphere {get; protected set;} = true;
        public virtual float GizmoRadius { get; protected set; } = 1f;
        
        public virtual string GizmoTitle { get; protected set; } = "";
        public virtual float GizmoTitleHeight { get; protected set; } = 3f;
        
        public virtual bool DrawRotationArrow {get; protected set; } = true;
        public virtual float GizmoRotationArrowSize { get; protected set; } = 3f;
    }
}