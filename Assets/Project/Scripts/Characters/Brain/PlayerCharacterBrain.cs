using System;
using System.Collections;
using Project.Scripts.Characters.HealthSystems;
using UnityEngine;

namespace Project.Scripts.Characters.Brain
{
    public class PlayerCharacterBrain : CharacterBrain
    {
        private const string HORIZONTAL_AXIS_NAME = "Horizontal";
        private const string VERTICAL_AXIS_NAME = "Vertical";
        private const string JUMP = "Jump";
        
        private readonly Character _character;
        private readonly Camera _camera;
        private readonly Death _characterDeath;

        public PlayerCharacterBrain(Character character, Camera camera)
        {
            _character = character;
            _camera = camera;
            
            _characterDeath = _character.GetComponent<Death>();
            _characterDeath.Happened += OnCharacterDeath;
        }

        public Vector3 Axis() => 
            new Vector3(Input.GetAxis(HORIZONTAL_AXIS_NAME), 0, Input.GetAxis(VERTICAL_AXIS_NAME));

        public bool IsJumpButtonDown() =>
            Input.GetButtonDown(JUMP);

        protected override void UpdateLogic(float deltaTime)
        {
            Vector3 inputDirection = Axis();
            Vector3 relativeInputDirection = GetRelativeInput(inputDirection);
            _character.SetMoveDirection(relativeInputDirection);
            
            Vector3 directionToMouse = GetDirectionToMouse();
            _character.SetRotationDirection(directionToMouse);
            
            if (IsJumpButtonDown())
                _character.Jump();
        }

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