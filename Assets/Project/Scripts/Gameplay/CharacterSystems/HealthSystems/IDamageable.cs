using System;
using Project.Scripts.Gameplay.Data;

namespace Project.Scripts.Gameplay.CharacterSystems.HealthSystems
{
    public interface IDamageable
    {
        void TakeDamage(DamageData damageData);
        event Action<DamageData> Damaged;
        event Action<DamageData> DestroyRequested;
    }
}