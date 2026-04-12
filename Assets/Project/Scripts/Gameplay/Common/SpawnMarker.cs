using Project.Scripts.Gameplay.Weapons.WeaponSpawn;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Gameplay.Common
{
    public abstract class SpawnMarker : MonoBehaviour
    {
        public virtual Color GizmoColor { get; protected set; } = Color.red;
        public virtual float GizmoRadius { get; protected set; } = 0.5f;
        public virtual string GizmoTitle { get; protected set; } = "";
        public virtual float GizmoTitleHeight { get; protected set; } = 3f;
        
#if UNITY_EDITOR
        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected)]
        static void DrawLabel(WeaponSpawnerMarker marker, GizmoType gizmo)
        {
            if (marker.GizmoTitle == "")
                return;
            
            var style = new GUIStyle();
            style.normal.textColor = marker.GizmoColor;
            style.fontStyle = FontStyle.Bold;

            Handles.Label(
                marker.transform.position + Vector3.up * marker.GizmoTitleHeight,
                marker.GizmoTitle,
                style
            );
        }
#endif
    }
}