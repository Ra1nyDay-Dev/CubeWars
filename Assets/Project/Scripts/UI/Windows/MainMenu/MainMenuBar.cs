using System.Collections.Generic;
using Project.Scripts.UI.Elements;
using Project.Scripts.UI.Services.WindowService;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI.Windows.MainMenu
{
    public class MainMenuBar : BaseWindow
    {
        [SerializeField] private List<NavButton> _buttons;
        
        private WindowId _current = WindowId.None;
        
        private IWindowService _windowService;

        [Inject]
        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }

        protected override void Initialize()
        {
            Id = WindowId.MainMenuNavBar;
            Select(WindowId.None);
        }

        protected override void SubscribeUpdates()
        {
            foreach (var button in _buttons)
                button.Clicked += OnButtonClicked;
        }

        private void OnButtonClicked(WindowId windowId)
        {
            if (_current == windowId)
                return;

            Select(windowId);
        }

        private void Select(WindowId windowId)
        {
            _windowService.Close(_current);
            
            if (windowId != WindowId.None) 
                _windowService.Open(windowId);
                
            foreach (var button in _buttons)
                button.SetSelected(button.WindowId == windowId);
            
            _current = windowId;
        }
    }
}