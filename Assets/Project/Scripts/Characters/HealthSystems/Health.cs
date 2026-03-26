using System;
using UnityEngine;

namespace Project.Scripts.Characters.HealthSystems
{
    public class Health : MonoBehaviour, IHealth, IDamageable
    {
        [field: SerializeField] public float Current { get; private set; }
        [field: SerializeField] public float Max { get; private set; }
        
        public event Action HealthChanged;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) 
                TakeDamage(10);
            
            if (Input.GetKeyDown(KeyCode.H)) 
                Heal(10);
        }

        public void TakeDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            ChangeHealth(-damage);
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