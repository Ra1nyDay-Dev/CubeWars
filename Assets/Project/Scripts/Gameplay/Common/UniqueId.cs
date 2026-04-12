using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Common
{
    public class UniqueId : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
        
        public void GenerateId() => 
            Id = $"{gameObject.scene.name}_{Guid.NewGuid().ToString()}";
    }
}