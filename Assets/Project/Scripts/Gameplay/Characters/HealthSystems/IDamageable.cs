using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        event Action Damaged;
    }
}