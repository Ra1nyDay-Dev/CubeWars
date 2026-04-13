using System;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public interface IHealth : IDamageable
    {
        float Current { get; }
        float Max { get; }
        event Action HealthChanged;
        void Heal(float heal);
    }
}