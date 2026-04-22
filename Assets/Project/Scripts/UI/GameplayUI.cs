using System;
using Project.Scripts.UI.Services.WindowService;
using Project.Scripts.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI
{
    public class GameplayUI : SceneUI
    {
        [SerializeField] private Button _settingsButton;
        
        private IWindowService _windowService;

        [Inject]
        private void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            SubscribeToEvents();
        }
        

        private void SubscribeToEvents() => 
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);

        private void OnSettingsButtonClick() => 
            _windowService.Open(WindowId.GameplaySetting);
    }
}