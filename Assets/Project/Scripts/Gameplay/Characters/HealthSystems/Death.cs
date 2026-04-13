using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.HealthSystems
{
    public class Death : MonoBehaviour
    {
        [SerializeField] private Health _health;
        
        private const float DESTROY_TIME_AFTER_DEATH = 3;
        
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
            Happened?.Invoke(damageData);
            DestroyTimer(this.GetCancellationTokenOnDestroy())
                .Forget(Debug.LogException);
        }
        
        private async UniTask DestroyTimer(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DESTROY_TIME_AFTER_DEATH), cancellationToken: cancellationToken);
            Destroy(gameObject);
        }
    }
}