using Project.Scripts.Gameplay.Common;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Editor
{
    [CustomEditor(typeof(SpawnMarker))]
    public class SpawnMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SpawnMarker spawner, GizmoType gizmo)
        {
            Gizmos.color = spawner.GizmoColor;
            Gizmos.DrawSphere(spawner.transform.position, spawner.GizmoRadius);
        }
    }
}