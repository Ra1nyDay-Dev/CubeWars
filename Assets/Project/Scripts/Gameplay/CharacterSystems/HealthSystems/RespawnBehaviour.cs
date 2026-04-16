using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.HealthSystems
{
    public class RespawnBehaviour : MonoBehaviour
    {
        public event Action <DamageData> Dead;
        public event Action Respawned;
        
        private const float VANISH_DELAY_AFTER_DEATH = 3;
        
        private IDamageable _damageable;
        private bool _isDead;
        
        private void Awake() => 
            _damageable = GetComponent<IDamageable>();

        public void Respawn()
        {
            Reset();
            Respawned?.Invoke();
            gameObject.SetActive(true);
        }

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
            Dead?.Invoke(damageData);
            VanishTimer(this.GetCancellationTokenOnDestroy())
                .Forget(Debug.LogException);
        }
        
        private async UniTask VanishTimer(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(VANISH_DELAY_AFTER_DEATH), cancellationToken: cancellationToken);
            gameObject.SetActive(false);
        }

        private void Reset() => 
            _isDead = false;
    }
}