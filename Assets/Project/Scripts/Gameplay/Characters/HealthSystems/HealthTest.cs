using System;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    // toDo: DEBUG_DELETE 
    public class HealthTest : MonoBehaviour, IHealth, IDamageable
    {
        [field: SerializeField] public float Current { get; private set; }
        [field: SerializeField] public float Max { get; private set; }
        
        public event Action HealthChanged;
        public event Action<DamageData> Damaged;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X)) 
                TakeDamage(new(10, DamageSource.Environment));
            
            if (Input.GetKeyDown(KeyCode.C)) 
                Heal(10);
        }

        public void TakeDamage(DamageData damageData)
        {
            if (damageData.Damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damageData.Damage));

            ChangeHealth(-damageData.Damage);
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
            Debug.Log($"Health changed: {Current} / {Max}");
            HealthChanged?.Invoke();
        }
    }
}