using System;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public interface IDamageable
    {
        event Action Damaged;
        void TakeDamage(float damage);
    }
}