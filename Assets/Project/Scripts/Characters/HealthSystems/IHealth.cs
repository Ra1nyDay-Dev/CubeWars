using System;

namespace Project.Scripts.Characters.HealthSystems
{
    public interface IHealth
    {
        float Current { get; }
        float Max { get; }
        event Action HealthChanged;
        public void TakeDamage(float damage);
        void Heal(float heal);
    }
}