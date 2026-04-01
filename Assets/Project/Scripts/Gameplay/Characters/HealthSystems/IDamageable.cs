using System;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public interface IDamageable
    {
        void TakeDamage(DamageData damageData);
        event Action<DamageData> Damaged;
    }
}