using System;
using System.Collections;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public class Death : MonoBehaviour
    {
        [SerializeField] private Health _health;
        
        public event Action <DamageData> Happened;
        
        private bool _isDead;
        
        private void Start() => 
            _health.Damaged += OnDamaged;

        private void OnDestroy() => 
            _health.Damaged -= OnDamaged;

        private void OnDamaged(DamageData damageData)
        {
            if (!_isDead && _health.Current <= 0)
                Die(damageData);
        }
        
        private void Die(DamageData damageData)
        {
            _isDead = true;
            _health.Damaged -= OnDamaged;
            StartCoroutine(DestroyTimer());
            Happened?.Invoke(damageData);
        }
        
        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}