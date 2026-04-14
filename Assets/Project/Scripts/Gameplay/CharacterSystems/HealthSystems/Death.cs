using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.HealthSystems
{
    public class Death : MonoBehaviour
    {
        public event Action <DamageData> Happened;
        
        private const float DESTROY_TIME_AFTER_DEATH = 3;
        
        private IDamageable _damageable;
        private bool _isDead;
        
        public void Awake() => 
            _damageable = GetComponent<IDamageable>();

        private void OnEnable() => 
            _damageable.DestroyRequested += OnDestroyRequestReceived;

        private void OnDisable() => 
            _damageable.DestroyRequested -= OnDestroyRequestReceived;

        private void OnDestroyRequestReceived(DamageData damageData)
        {
            if (!_isDead)
                Die(damageData);
        }
        
        private void Die(DamageData damageData)
        {
            _isDead = true;
            _damageable.DestroyRequested -= OnDestroyRequestReceived;
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