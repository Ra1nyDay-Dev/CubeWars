using System;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data
{
    [Serializable]
    public class WeaponSpawnerData
    {
        public Vector3 Position;
        public WeaponType WeaponType;
        public float SpawnTime;
        public bool SpawnOnStart;
        
        public WeaponSpawnerData(
            Vector3 position,
            WeaponType weaponType,
            float spawnTime,
            bool spawnOnStart)
        {
            Position = position;
            WeaponType = weaponType;
            SpawnTime = spawnTime;
            SpawnOnStart = spawnOnStart;
        }
    }
}