using System;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.Input
{
    public class KeyboardOldInputService : IInputService
    {
        private const string HORIZONTAL_AXIS = "Horizontal";
        private const string VERTICAL_AXIS = "Vertical";
        private const string PRIMARY_ATTACK_AXIS = "Fire1";
        private const string SECONDARY_ATTACK_AXIS = "Fire2";
        private const string JUMP_AXIS = "Jump";
        private const KeyCode RELOAD_KEY_CODE = KeyCode.R;
        private const KeyCode INTERACT_KEY_CODE = KeyCode.E;
        
        private Camera _mainCamera;
        private Vector3 _screenPosition;

        private Camera MainCamera
        {
            get
            {
                if(_mainCamera == null && Camera.main != null)
                    _mainCamera = Camera.main;
        
                return _mainCamera;
            }
        }

        public GameplayInput GetGameplayInput(Vector3 playerPosition) =>
            new GameplayInput()
            {
                Move = GetRelativeAxis(),
                Look = GetWorldDirectionToMouseFromPoint(playerPosition),
                Jump = IsJumpButtonDown(),
                StartPrimaryFire = IsPrimaryAttackButtonDown(),
                StopPrimaryFire = IsPrimaryAttackButtonUp(),
                StartSecondaryFire = IsSecondaryAttackButtonDown(),
                StopSecondaryFire = IsSecondaryAttackButtonUp(),
                Reload = IsReloadButtonDown(),
                Interact = IsInteractButtonDown(),
            };

        public Vector2 GetAxis() => 
            new Vector2(
                UnityEngine.Input.GetAxis(KeyboardOldInputService.HORIZONTAL_AXIS),
                UnityEngine.Input.GetAxis(KeyboardOldInputService.VERTICAL_AXIS)
            );

        public Vector2 GetRelativeAxis()
        {
            Vector2 input = GetAxis();
            
            Vector3 cameraForward = MainCamera.transform.forward
                .With(forward => forward.y = 0)
                .With(forward => forward.Normalize());
            
            Vector3 cameraRight = MainCamera.transform.right
                .With(right => right.y = 0)
                .With(right => right.Normalize());
            
            Vector3 relativeInput = cameraForward * input.y + cameraRight * input.x;
            return new Vector2(relativeInput.x, relativeInput.z);
        }

        public bool IsPrimaryAttackButtonDown() => 
            UnityEngine.Input.GetButtonDown(KeyboardOldInputService.PRIMARY_ATTACK_AXIS);

        public bool IsPrimaryAttackButtonUp() => 
            UnityEngine.Input.GetButtonUp(KeyboardOldInputService.PRIMARY_ATTACK_AXIS);

        public bool IsSecondaryAttackButtonDown() => 
            UnityEngine.Input.GetButtonDown(KeyboardOldInputService.SECONDARY_ATTACK_AXIS);

        public bool IsSecondaryAttackButtonUp() => 
            UnityEngine.Input.GetButtonUp(KeyboardOldInputService.SECONDARY_ATTACK_AXIS);

        public bool IsJumpButtonDown() =>
            UnityEngine.Input.GetButtonDown(KeyboardOldInputService.JUMP_AXIS);

        public bool IsReloadButtonDown() => 
            UnityEngine.Input.GetKeyDown(KeyboardOldInputService.RELOAD_KEY_CODE);

        public bool IsInteractButtonDown() =>
            UnityEngine.Input.GetKeyDown(KeyboardOldInputService.INTERACT_KEY_CODE);

        private Vector2 GetScreenMousePosition() => 
            MainCamera ? UnityEngine.Input.mousePosition : new Vector2();

        private Vector3 GetWorldDirectionToMouseFromPoint(Vector3 point)
        {
            Ray ray = MainCamera.ScreenPointToRay(GetScreenMousePosition());
            Plane groundPlane = new Plane(Vector3.up, point);
            Vector3 directionToMouse = Vector3.zero;
            
            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 rayHitPoint = ray.GetPoint(distance);
                directionToMouse = rayHitPoint - point;                
                directionToMouse.y = 0;
            }

            return directionToMouse;
        }
    }
}