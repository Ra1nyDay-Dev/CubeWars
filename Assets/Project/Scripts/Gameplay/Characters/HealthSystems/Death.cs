using System;
using System.Collections;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public class Death : MonoBehaviour
    {
        [SerializeField] private Health _health;
        
        public event Action Happened;
        
        private bool _isDead;
        
        private void Start() => 
            _health.HealthChanged += HealthChanged;

        private void OnDestroy() => 
            _health.HealthChanged -= HealthChanged;
        
        private void HealthChanged()
        {
            if (!_isDead && _health.Current <= 0)
                Die();
        }
        
        private void Die()
        {
            _isDead = true;
            _health.HealthChanged -= HealthChanged;
            StartCoroutine(DestroyTimer());
            Happened?.Invoke();
        }
        
        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}