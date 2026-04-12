using Project.Scripts.Gameplay.Common;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons.WeaponSpawn
{
    public class WeaponSpawnerMarker : SpawnMarker
    {
        public override Color GizmoColor => Color.yellow;
        public override float GizmoRadius => 1.5f;
        public override string GizmoTitle => WeaponType.ToString();
        public override float GizmoTitleHeight => 3f;
        
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field: SerializeField] public float SpawnTime { get; private set; }
        [field: SerializeField] public bool SpawnOnStart { get; private set; }
    }
}