using System;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Infrastructure.Services.Input.DeviceTracker;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Infrastructure.Services.Input.ActionMaps.Gameplay
{
    public class GameplayActions : IGameplayActions, IDisposable
    {
        public Vector2 MoveInput => 
            _gameplayActions.Move.ReadValue<Vector2>();
        
        public Vector2 LookInput =>
            _gameplayActions.Look.ReadValue<Vector2>();

        public bool IsPrimaryButtonDown =>
            _gameplayActions.PrimaryAttack.WasPressedThisFrame();

        public bool IsPrimaryButtonUp =>
            _gameplayActions.PrimaryAttack.WasReleasedThisFrame();
        
        public bool IsSecondaryButtonDown =>
            _gameplayActions.SecondaryAttack.WasPressedThisFrame();
        
        public bool IsSecondaryButtonUp =>
            _gameplayActions.SecondaryAttack.WasReleasedThisFrame();

        public bool IsReloadButtonDown =>
            _gameplayActions.Reload.WasPressedThisFrame();

        public bool IsInteractButtonDown =>
            _gameplayActions.Interact.WasPressedThisFrame();

        public bool IsJumpButtonDown =>
            _gameplayActions.Jump.WasPressedThisFrame();

        public event Action<Vector2> MoveInputChanged;
        public event Action<Vector2> LookInputChanged;
        public event Action PrimaryButtonDown;
        public event Action PrimaryButtonUp;
        public event Action SecondaryButtonDown;
        public event Action SecondaryButtonUp;
        public event Action ReloadButtonDown;
        public event Action InteractButtonDown;
        public event Action JumpButtonDown;
        
        private readonly IInputDeviceTracker _inputDeviceTracker;
        private readonly InputActions.GameplayActions _gameplayActions;
        
        public GameplayActions(
            IInputDeviceTracker inputDeviceTracker,
            InputActions.GameplayActions gameplayActions)
        {
            _inputDeviceTracker = inputDeviceTracker;
            _gameplayActions = gameplayActions;
            
            _gameplayActions.SetCallbacks(this);
        }

        public Vector2 GetRelativeMoveInput(Camera camera)
        {
            Vector3 relativeVector = GetRelativeInputVector(MoveInput, camera);
            
            return new Vector2(relativeVector.x, relativeVector.z);
        }

        public Vector3 GetRelativeLookInput(Vector3 playerPosition, Camera camera)
        {
            if (_inputDeviceTracker.CurrentControlScheme == ControlSchemes.KEYBOARD_MOUSE_CONTROL_SCHEME)
                return GetLookDirectionToMousePosition(playerPosition, camera);
            
            return GetCommonLookDirection(camera);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.started)
                MoveInputChanged?.Invoke(context.ReadValue<Vector2>());
            else if (context.canceled) 
                MoveInputChanged?.Invoke(Vector2.zero);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.performed)
                LookInputChanged?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPrimaryAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                PrimaryButtonDown?.Invoke();
            else if (context.canceled) 
                PrimaryButtonUp?.Invoke();
        }

        public void OnSecondaryAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                SecondaryButtonDown?.Invoke();
            else if (context.canceled) 
                SecondaryButtonUp?.Invoke();
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            if(context.performed)
                ReloadButtonDown?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed)
                InteractButtonDown?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
                JumpButtonDown?.Invoke();
        }
        
        public void Dispose() => 
            _gameplayActions.SetCallbacks(null);

        private Vector3 GetLookDirectionToMousePosition(Vector3 playerPosition, Camera camera)
        {
            Ray rayToMouse = camera.ScreenPointToRay(LookInput);
            Plane groundPlane = new Plane(Vector3.up, playerPosition);
            
            if (groundPlane.Raycast(rayToMouse, out float distance))
            {
                Vector3 mousePosition = rayToMouse.GetPoint(distance);
                Vector3 directionToMouse = (mousePosition - playerPosition).With(y: 0).normalized;
                return directionToMouse;
            }
            
            return Vector3.zero;
        }

        private Vector3 GetCommonLookDirection(Camera camera) => 
            GetRelativeInputVector(LookInput, camera);

        private Vector3 GetRelativeInputVector(Vector3 vector, Camera camera)
        {
            Vector3 cameraForward = camera.transform.forward.With(y: 0).normalized;
            Vector3 cameraRight = camera.transform.right.With(y: 0).normalized;
            
            return (cameraForward * vector.y + cameraRight * vector.x).normalized;
        }
    }
}