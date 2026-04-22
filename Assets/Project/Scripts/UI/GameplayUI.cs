using System;
using Project.Scripts.Gameplay.Services.Factories.BrainFactory;
using Project.Scripts.Gameplay.WeaponSystems;
using Project.Scripts.UI.Services.WindowService;
using Project.Scripts.UI.Windows;
using Project.Scripts.UI.Windows.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI
{
    public class GameplayUI : SceneUI
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private WeaponAmmoView _weaponAmmo;
        
        private IWindowService _windowService;
        private IBrainFactory _brainFactory;

        [Inject]
        private void Construct(IWindowService windowService, IBrainFactory brainFactory)
        {
            _windowService = windowService;
            _brainFactory =  brainFactory;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            SubscribeToEvents();
        }

        public override void Dispose()
        {
            base.Dispose();
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);

            if (_brainFactory.PlayerBrain != null)
            {
                _brainFactory.PlayerBrain.Character.WeaponArsenal.WeaponChanged += OnPlayerWeaponChanged;
                _brainFactory.PlayerBrain.Character.WeaponArsenal.CurrentWeaponAmmoChanged += OnCurrenWeaponAmmoChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_brainFactory.PlayerBrain != null)
            {
                _brainFactory.PlayerBrain.Character.WeaponArsenal.WeaponChanged -= OnPlayerWeaponChanged;
                _brainFactory.PlayerBrain.Character.WeaponArsenal.CurrentWeaponAmmoChanged -= OnCurrenWeaponAmmoChanged;
            }
        }

        private void OnPlayerWeaponChanged(IWeapon weapon)
        {
            if (weapon is RangeWeapon rangeWeapon) 
                _weaponAmmo.gameObject.SetActive(true);
            else
                _weaponAmmo.gameObject.SetActive(false);
        }

        private void OnCurrenWeaponAmmoChanged(int currentAmmo, int reservedAmmo, bool infiniteAmmo) => 
            _weaponAmmo.UpdateText(currentAmmo, reservedAmmo, infiniteAmmo);

        private void OnSettingsButtonClick() => 
            _windowService.Open(WindowId.GameplaySetting);
    }
}