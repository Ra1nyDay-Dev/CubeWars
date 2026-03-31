using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public class HealthTest : MonoBehaviour, IHealth, IDamageable
    {
        [field: SerializeField] public float Current { get; private set; }
        [field: SerializeField] public float Max { get; private set; }
        
        public event Action HealthChanged;
        public event Action<Vector3> Damaged;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X)) 
                TakeDamage(10, Vector3.forward);
            
            if (Input.GetKeyDown(KeyCode.C)) 
                Heal(10);
        }

        public void TakeDamage(float damage, Vector3 hitDirection)
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
            Debug.Log($"Health changed: {Current} / {Max}");
            HealthChanged?.Invoke();
        }
    }
}