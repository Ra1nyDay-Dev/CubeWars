using System;

namespace Project.Scripts.Characters.CombatSystems
{
    public interface IHealth
    {
        float Current { get; }
        float Max { get; }
        event Action HealthChanged;
        void TakeDamage(float damage);
        void Heal(float heal);
    }
}