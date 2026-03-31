using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Vector3 hitDirection);
        event Action<Vector3> Damaged;
    }
}