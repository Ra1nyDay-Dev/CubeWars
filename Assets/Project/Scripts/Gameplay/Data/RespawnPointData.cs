using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data
{
    [Serializable]
    public class RespawnPointData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public RespawnPointData(
            Vector3 position,
            Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}