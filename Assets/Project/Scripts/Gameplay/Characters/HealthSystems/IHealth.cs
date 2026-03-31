using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public interface IHealth
    {
        float Current { get; }
        float Max { get; }
        event Action HealthChanged;
        void Heal(float heal);
        public void TakeDamage(float damage, Vector3 hitDirection);
        public event Action<Vector3> Damaged;
    }
}