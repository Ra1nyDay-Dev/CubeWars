using System;
using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using Project.Scripts.Gameplay.Data.Configs.Health;

namespace Project.Scripts.Gameplay.CharacterSystems.HealthSystems
{
    public interface IHealth : IDamageable
    {
        void Construct(HealthConfig config);
        float Current { get; }
        float Max { get; }
        event Action HealthChanged;
        void Heal(float heal);
        void Reset();
    }
}