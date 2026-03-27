using System;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using UnityEngine;

namespace Project.Scripts.UI.Elements
{
    public class CharacterUI : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;
        private IHealth _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        public void Start()
        {
            _health.HealthChanged += UpdateHealthBar;
            UpdateHealthBar();
        }
        
        private void OnDestroy()
        {
            if (_health != null) 
                _health.HealthChanged -= UpdateHealthBar;
        }

        private void UpdateHealthBar() => 
            _healthBar.SetValue(_health.Current, _health.Max);
    }
}