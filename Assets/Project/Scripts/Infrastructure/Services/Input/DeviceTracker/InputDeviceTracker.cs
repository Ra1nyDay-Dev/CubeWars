using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Infrastructure.Services.Input.DeviceTracker
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputDeviceTracker : MonoBehaviour, IInputDeviceTracker
    {
        public string CurrentControlScheme => 
            _playerInput.currentControlScheme;
        
        public bool AutoSwitchEnabled 
        { 
            get => !_playerInput.neverAutoSwitchControlSchemes;
            set => _playerInput.neverAutoSwitchControlSchemes = !value;
        }

        public event Action<string> ControlSchemeChanged;

        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            DontDestroyOnLoad(this);
        }

        private void OnEnable() => 
            _playerInput.onControlsChanged += OnControlsChanged;

        private void OnDisable() => 
            _playerInput.onControlsChanged -= OnControlsChanged;
        
        public void SwitchControlScheme(ControlSchemeType schemeType)
        {
            string scheme = schemeType switch
            {
                ControlSchemeType.KeyboardMouse => ControlSchemes.KEYBOARD_MOUSE_CONTROL_SCHEME,
                ControlSchemeType.Gamepad => ControlSchemes.GAMEPAD_CONTROL_SCHEME,
                _ => throw new ArgumentOutOfRangeException(nameof(schemeType), schemeType, null)
            };
            
            _playerInput.SwitchCurrentControlScheme(scheme);
        }

        private void OnControlsChanged(PlayerInput input) => 
            ControlSchemeChanged?.Invoke(input.currentControlScheme);
    }
}