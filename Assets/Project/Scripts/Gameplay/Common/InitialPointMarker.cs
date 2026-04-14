

using Project.Scripts.Gameplay.Data.Configs.AttackConfigs;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Gameplay.Common
{
    public class InitialPointMarker : SpawnMarker
    {
        public override Color GizmoColor => Color.mediumSeaGreen;
        public override float GizmoRadius => 1.5f;
        public override string GizmoTitle => "Initial point";
    }
}