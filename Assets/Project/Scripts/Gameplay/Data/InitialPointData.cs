using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data
{
    [Serializable]
    public class InitialPointData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        
        public InitialPointData(
            Vector3 position,
            Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}