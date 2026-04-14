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
        private Death _death;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _death = GetComponent<Death>();
            UpdateHealthBar();
        }

        private void OnEnable() => 
            SubscribeToEvents();

        private void OnDestroy() => 
            UnsubscribeFromEvents();


        private void SubscribeToEvents()
        {
            _health.HealthChanged += UpdateHealthBar;
            _death.Happened += OnDie;
        }

        private void UnsubscribeFromEvents()
        {
            _health.HealthChanged -= UpdateHealthBar;
            _death.Happened -= OnDie;
        }

        private void UpdateHealthBar() => 
            _healthBar.SetValue(_health.Current, _health.Max);

        private void OnDie(DamageData damageData) => 
            _healthBar.gameObject.SetActive(false);
    }
}