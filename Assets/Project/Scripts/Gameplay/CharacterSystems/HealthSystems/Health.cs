using System;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using Project.Scripts.Gameplay.Data.Configs.Health;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.HealthSystems
{
    public class Health : MonoBehaviour, IHealth
    {
        public float Max { get; private set; }
        public float Current { get; private set; }
        
        public event Action HealthChanged;
        public event Action<DamageData> Damaged;
        public event Action<DamageData> DestroyRequested;
        
        private RespawnBehaviour _respawnBehaviour;

        public void Construct(HealthConfig config)
        {
            Max = config.Max;
            Current = config.Current;
            
            _respawnBehaviour = GetComponent<RespawnBehaviour>();
            _respawnBehaviour.Respawned += OnRespawn;
        }

        public void Start() => 
            HealthChanged?.Invoke();

        public void TakeDamage(DamageData damageData)
        {
            if (damageData.Damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damageData.Damage));

            if (Current <= 0)
                return;
            
            ChangeHealth(-damageData.Damage);
            Damaged?.Invoke(damageData);
            
            if (Current <= 0)
                DestroyRequested?.Invoke(damageData);
        }

        public void Heal(float heal)
        {
            if (heal < 0)
                throw new ArgumentOutOfRangeException(nameof(heal));
            
            ChangeHealth(heal);
        }

        private void OnRespawn() => 
            Reset();

        private void ChangeHealth(float value)
        {
            Current = Mathf.Clamp(Current + value, 0, Max);
            HealthChanged?.Invoke();
        }

        private void Reset()
        {
            Current = Max;
            HealthChanged?.Invoke();
        }

        private void OnDestroy() => 
            _respawnBehaviour.Respawned -= OnRespawn;
    }
}