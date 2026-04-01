using System;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public class Health : MonoBehaviour, IHealth, IDamageable
    {
        [field: SerializeField] public float Current { get; private set; }
        [field: SerializeField] public float Max { get; private set; }
        
        public event Action HealthChanged;
        public event Action<DamageData> Damaged;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) 
                TakeDamage(new(10, DamageSource.Environment));
            
            if (Input.GetKeyDown(KeyCode.H)) 
                Heal(10);
        }

        public void TakeDamage(DamageData damageData)
        {
            if (damageData.Damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damageData.Damage));

            if (Current <= 0)
                return;
            
            ChangeHealth(-damageData.Damage);
            Damaged?.Invoke(damageData);
        }

        public void Heal(float heal)
        {
            if (heal < 0)
                throw new ArgumentOutOfRangeException(nameof(heal));
            
            ChangeHealth(heal);
        }
        
        private void ChangeHealth(float value)
        {
            Current = Mathf.Clamp(Current + value, 0, Max);
            HealthChanged?.Invoke();
        }
    }
}