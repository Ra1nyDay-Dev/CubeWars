using System;
using Project.Scripts.Gameplay.Characters.HealthSystems;
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
        }

        public void Start()
        {
            _health.HealthChanged += UpdateHealthBar;
            _death.Happened += OnDie;
            
            UpdateHealthBar();
        }
        
        private void OnDestroy()
        {
            if (_health != null) 
                _health.HealthChanged -= UpdateHealthBar;
            
            if (_death != null)
                _death.Happened -= OnDie;
        }

        private void UpdateHealthBar() => 
            _healthBar.SetValue(_health.Current, _health.Max);

        private void OnDie() => 
            _healthBar.gameObject.SetActive(false);
    }
}