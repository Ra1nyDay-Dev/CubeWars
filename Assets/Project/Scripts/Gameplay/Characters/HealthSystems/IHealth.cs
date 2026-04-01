using System;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public interface IHealth
    {
        float Current { get; }
        float Max { get; }
        event Action HealthChanged;
        event Action<DamageData> Damaged;
        void Heal(float heal);
        public void TakeDamage(DamageData damageData);
    }
}