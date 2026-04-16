using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data
{
    [Serializable]
    public class RespawnPointData
    {
        public Vector3 PointPosition;
        public Quaternion PointRotation;
        public Vector3 EnemyCheckZoneCenter;
        public Vector3 EnemyCheckZoneSize;

        public RespawnPointData(
            Vector3 pointPosition,
            Quaternion pointRotation,
            Vector3 enemyCheckZoneCenter,
            Vector3 enemyCheckZoneSize)
        {
            PointPosition = pointPosition;
            PointRotation = pointRotation;
            EnemyCheckZoneCenter = enemyCheckZoneCenter;
            EnemyCheckZoneSize = enemyCheckZoneSize;
        }
    }
}