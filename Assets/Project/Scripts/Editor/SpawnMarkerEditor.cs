using Project.Scripts.Gameplay.Common;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Editor
{
    [CustomEditor(typeof(SpawnMarker))]
    public class SpawnMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SpawnMarker marker, GizmoType gizmo)
        {
            Gizmos.color = marker.GizmoColor;
            Handles.color = marker.GizmoColor;   
            
            if (marker.DrawPositionSphere)
                DrawPositionSphere(marker);
            
            if (marker.GizmoTitle != "")
                DrawLabel(marker);
            
            if (marker.DrawRotationArrow)
                DrawRotationArrow(marker);
        }

        private static void DrawPositionSphere(SpawnMarker marker) => 
            Gizmos.DrawSphere(marker.transform.position, marker.GizmoRadius);

        private static void DrawLabel(SpawnMarker marker)
        {
            var style = new GUIStyle();
            style.normal.textColor = marker.GizmoColor;
            style.fontStyle = FontStyle.Bold;

            Handles.Label(
                marker.transform.position + Vector3.up * marker.GizmoTitleHeight,
                marker.GizmoTitle,
                style
            );
        }

        private static void DrawRotationArrow(SpawnMarker marker) =>
            Handles.ArrowHandleCap(
                0,
                marker.transform.position,
                Quaternion.LookRotation(marker.transform.forward),
                marker.GizmoRotationArrowSize,
                Event.current.type);
    }
}