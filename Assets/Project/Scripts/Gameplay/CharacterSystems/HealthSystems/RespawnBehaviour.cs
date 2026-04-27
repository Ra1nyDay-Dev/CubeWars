using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.HealthSystems
{
    public class RespawnBehaviour : MonoBehaviour
    {
        public bool IsDead { get; private set; }
        
        public event Action Respawned;
        public event Action <DamageData> Dead;
        public event Action Vanished;
        
        private const float VANISH_DELAY_AFTER_DEATH = 3;
        
        private IDamageable _damageable;
        
        private void Awake() => 
            _damageable = GetComponent<IDamageable>();

        public void Respawn()
        {
            Reset();
            gameObject.SetActive(true);
            Respawned?.Invoke();
        }

        private void OnEnable() => 
            _damageable.DestroyRequested += OnDestroyRequestReceived;

        private void OnDisable() => 
            _damageable.DestroyRequested -= OnDestroyRequestReceived;

        private void OnDestroyRequestReceived(DamageData damageData)
        {
            if (!IsDead)
                Die(damageData);
        }
        
        private void Die(DamageData damageData)
        {
            IsDead = true;
            _damageable.DestroyRequested -= OnDestroyRequestReceived;
            Dead?.Invoke(damageData);
            VanishTimer(this.GetCancellationTokenOnDestroy()).Forget();
        }
        
        private async UniTask VanishTimer(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(VANISH_DELAY_AFTER_DEATH), cancellationToken: cancellationToken);
            gameObject.SetActive(false);
            Vanished?.Invoke();
        }

        private void Reset() => 
            IsDead = false;
    }
}