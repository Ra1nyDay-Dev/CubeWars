using System;
using Project.Scripts.Infrastructure.Services.Input.ActionMaps;
using Project.Scripts.Infrastructure.Services.Input.ActionMaps.Gameplay;
using Project.Scripts.Infrastructure.Services.Input.DeviceTracker;
using UnityEngine.InputSystem;
using Zenject;

namespace Project.Scripts.Infrastructure.Services.Input
{
    public class UnityNewInputService : IInputService, IDisposable
    {
        public IGameplayActions GameplayActions { get; }
        public ActionMapType CurrentActionMap {get; private set;}
        
        private readonly InputActions _actions;
        private readonly IInputDeviceTracker _inputDeviceTracker;

        [Inject]
        public UnityNewInputService(IInputDeviceTracker inputDeviceTracker)
        {
            _inputDeviceTracker = inputDeviceTracker;
            _actions = new InputActions();
            
            GameplayActions = new GameplayActions(_inputDeviceTracker, _actions.Gameplay);
            
            _actions.bindingMask = InputBinding.MaskByGroup(_inputDeviceTracker.CurrentControlScheme);
            _actions.Enable();

            _inputDeviceTracker.ControlSchemeChanged += OnControlsSchemeChanged;
        }

        public void SwitchActionMap(ActionMapType actionMapType)
        {
            _actions.asset.Disable();

            switch (actionMapType)
            {
                case ActionMapType.Gameplay:
                    _actions.Gameplay.Enable();
                    break;
                case ActionMapType.UI:
                    _actions.UI.Enable();
                    break;
            }
            CurrentActionMap =  actionMapType;
        }

        public void Dispose() => 
            _inputDeviceTracker.ControlSchemeChanged -= OnControlsSchemeChanged;

        private void OnControlsSchemeChanged(string scheme) => 
            _actions.bindingMask = InputBinding.MaskByGroup(scheme);
    }
}