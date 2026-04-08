using System.Collections;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Characters.Movement;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Weapons;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Brain
{
    public class PlayerCharacterBrain : CharacterBrain
    {
        private const string HORIZONTAL_AXIS = "Horizontal";
        private const string VERTICAL_AXIS = "Vertical";
        private const string JUMP_AXIS = "Jump";
        private const string PRIMARY_ATTACK_AXIS = "Fire1";
        private const string SECONDARY_ATTACK_AXIS = "Fire2";
        
        private readonly Camera _camera;
        private readonly CharacterMovement _characterMovement;
        private readonly WeaponArsenal _weaponArsenal;
        private readonly Death _characterDeath;

        public PlayerCharacterBrain(CharacterMovement characterMovement, Camera camera)
        {
            _camera = camera;
            _characterMovement = characterMovement;
            
            _characterDeath = _characterMovement.GetComponent<Death>();
            _characterDeath.Happened += OnCharacterDeath;

            _weaponArsenal = _characterMovement.GetComponent<WeaponArsenal>();
        }

        public Vector3 Axis() => 
            new Vector3(Input.GetAxis(HORIZONTAL_AXIS), 0, Input.GetAxis(VERTICAL_AXIS));

        private bool IsJumpButtonDown() =>
            Input.GetButtonDown(JUMP_AXIS);

        public bool IsPrimaryAttackButtonDown() => 
            Input.GetButtonDown(PRIMARY_ATTACK_AXIS);
        
        public bool IsPrimaryAttackButtonUp() => 
            Input.GetButtonUp(PRIMARY_ATTACK_AXIS);

        public bool IsSecondaryAttackButtonDown() => 
            Input.GetButtonDown(SECONDARY_ATTACK_AXIS);
        
        public bool IsSecondaryAttackButtonUp() => 
            Input.GetButtonUp(SECONDARY_ATTACK_AXIS);

        public bool IsReloadButtonDown() => 
            Input.GetKeyDown(KeyCode.R);

        protected override void UpdateLogic(float deltaTime)
        {
            Vector3 inputDirection = Axis();
            Vector3 relativeInputDirection = GetRelativeInput(inputDirection);
            _characterMovement.SetMoveDirection(relativeInputDirection);
            
            Vector3 directionToMouse = GetDirectionToMouse();
            _characterMovement.SetRotationDirection(directionToMouse);
            
            if (IsJumpButtonDown())
                _characterMovement.Jump();

            if (IsPrimaryAttackButtonDown())
                PerformPrimaryAttack();
            
            if (IsPrimaryAttackButtonUp())
                StopPrimaryAttack();
            
            if (IsSecondaryAttackButtonDown())
                PerformSecondaryAttack();
            
            if (IsSecondaryAttackButtonUp())
                StopSecondaryAttack();

            if (IsReloadButtonDown() && IsReloadableWeapon(out RangeWeapon reloadable))
                reloadable.Reload().Forget();
        }

        private bool IsReloadableWeapon(out RangeWeapon reloadable)
        {
            reloadable = _weaponArsenal.CurrentWeapon as RangeWeapon;
            return reloadable != null && reloadable.IsReloadable;
        }

        private void PerformPrimaryAttack() => 
            _weaponArsenal.CurrentWeapon?.StartPrimaryAttack();

        private void PerformSecondaryAttack() => 
            _weaponArsenal.CurrentWeapon?.StartSecondaryAttack();

        private void StopPrimaryAttack() => 
            _weaponArsenal.CurrentWeapon?.StopPrimaryAttack();
        private void StopSecondaryAttack() => 
            _weaponArsenal.CurrentWeapon?.StopSecondaryAttack();

        private Vector3 GetRelativeInput(Vector3 inputDirection)
        {
            Vector3 cameraForward = _camera.transform.forward;
            Vector3 cameraRight = _camera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();
            
            Vector3 relativeInput = (cameraForward * inputDirection.z) + (cameraRight * inputDirection.x);
            return relativeInput;
        }

        private Vector3 GetDirectionToMouse()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, _characterMovement.transform.position);
            Vector3 directionToMouse = Vector3.zero;
            
            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                directionToMouse = hitPoint - _characterMovement.transform.position;                
                directionToMouse.y = 0;
            }

            return directionToMouse;
        }

        private void OnCharacterDeath(DamageData damageData)
        {
            Disable();
            _characterMovement.StartCoroutine(StopMovementAfterDelay(0.5f));
        }

        private IEnumerator StopMovementAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _characterMovement.SetMoveDirection(Vector3.zero);
        }
    }
}