using System.Collections;
using Project.Scripts.Gameplay.Characters.HealthSystems;
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
        private readonly Character _character;
        private readonly WeaponArsenal _weaponArsenal;
        private readonly Death _characterDeath;

        public PlayerCharacterBrain(Character character, Camera camera)
        {
            _camera = camera;
            _character = character;
            
            _characterDeath = _character.GetComponent<Death>();
            _characterDeath.Happened += OnCharacterDeath;

            _weaponArsenal = _character.GetComponent<WeaponArsenal>();
        }

        public Vector3 Axis() => 
            new Vector3(Input.GetAxis(HORIZONTAL_AXIS), 0, Input.GetAxis(VERTICAL_AXIS));

        private bool IsJumpButtonDown() =>
            Input.GetButtonDown(JUMP_AXIS);

        public bool IsPrimaryAttackButtonDown() => 
            Input.GetButtonDown(PRIMARY_ATTACK_AXIS);
        
        public bool IsSecondaryAttackButtonDown() => 
            Input.GetButtonDown(SECONDARY_ATTACK_AXIS);

        protected override void UpdateLogic(float deltaTime)
        {
            Vector3 inputDirection = Axis();
            Vector3 relativeInputDirection = GetRelativeInput(inputDirection);
            _character.SetMoveDirection(relativeInputDirection);
            
            Vector3 directionToMouse = GetDirectionToMouse();
            _character.SetRotationDirection(directionToMouse);
            
            if (IsJumpButtonDown())
                _character.Jump();

            if (IsPrimaryAttackButtonDown())
                PerformPrimaryAttack();
            
            if (IsSecondaryAttackButtonDown())
                PerformSecondaryAttack();
        }

        private void PerformPrimaryAttack() => 
            _weaponArsenal.CurrentWeapon?.PerformPrimaryAttack();

        private void PerformSecondaryAttack() => 
            _weaponArsenal.CurrentWeapon?.PerformPrimaryAttack();

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
            Plane groundPlane = new Plane(Vector3.up, _character.transform.position);
            Vector3 directionToMouse = Vector3.zero;
            
            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                directionToMouse = hitPoint - _character.transform.position;                
                directionToMouse.y = 0;
            }

            return directionToMouse;
        }

        private void OnCharacterDeath()
        {
            Disable();
            _character.StartCoroutine(StopMovementAfterDelay(0.5f));
        }

        private IEnumerator StopMovementAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _character.SetMoveDirection(Vector3.zero);
        }
    }
}