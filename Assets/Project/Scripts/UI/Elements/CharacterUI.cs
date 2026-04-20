using System;
using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.UI.Elements
{
    public class CharacterUI : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;
        private IHealth _health;
        private RespawnBehaviour _respawnBehaviour;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _respawnBehaviour = GetComponent<RespawnBehaviour>();
            UpdateHealthBar();
            SubscribeToEvents();
        }
        
        private void OnDestroy() => 
            UnsubscribeFromEvents();

        public void HideAll() => 
            _healthBar.gameObject.SetActive(false);
        
        public void ShowAll() =>
            _healthBar.gameObject.SetActive(true);
        
        private void SubscribeToEvents()
        {
            _health.HealthChanged += UpdateHealthBar;
            _respawnBehaviour.Dead += OnDie;
            _respawnBehaviour.Respawned += OnRespawn;
        }

        private void UnsubscribeFromEvents()
        {
            _health.HealthChanged -= UpdateHealthBar;
            _respawnBehaviour.Dead -= OnDie;
            _respawnBehaviour.Respawned -= OnRespawn;
        }

        private void UpdateHealthBar() => 
            _healthBar.SetValue(_health.Current, _health.Max);

        private void OnDie(DamageData damageData) => 
            HideAll();

        private void OnRespawn()
        {
            ShowAll();
            UpdateHealthBar();
        }
    }
}