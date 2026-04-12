using System;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data
{
    [Serializable]
    public class WeaponSpawnerData
    {
        public string Id;
        public Vector3 Position;
        public WeaponType WeaponType;
        public float SpawnTime;
        public bool SpawnOnStart;
        
        public WeaponSpawnerData(
            string id,
            Vector3 position,
            WeaponType weaponType,
            float spawnTime,
            bool spawnOnStart)
        {
            Id = id;
            Position = position;
            WeaponType = weaponType;
            SpawnTime = spawnTime;
            SpawnOnStart = spawnOnStart;
        }
    }
}