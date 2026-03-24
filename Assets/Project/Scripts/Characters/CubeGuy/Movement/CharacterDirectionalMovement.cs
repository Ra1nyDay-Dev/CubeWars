using System;
using UnityEngine;

namespace Project.Scripts.Characters.CubeGuy.Movement
{
    public class CharacterDirectionalMovement
    {
        public event Action<Vector3> DirectionChanged;
        public event Action<Vector3> VelocityChanged;
        public event Action Moved;
        
        public Vector3 CurrentDirection {get; private set;}
        
        private readonly CharacterController _characterController;
        private readonly float _movementSpeed;
        
        private Vector3 _lastDirection = Vector3.zero; 
        private Vector3 _lastVelocity = Vector3.zero;

        public CharacterDirectionalMovement(CharacterController characterController, float movementSpeed)
        {
            _characterController = characterController;
            _movementSpeed = movementSpeed;
        }
        
        public Vector3 CurrentVelocity => 
            CurrentDirection.normalized * _movementSpeed;
        
        public void SetDirection(Vector3 direction) => 
            CurrentDirection = direction;

        public void Update(float deltaTime)
        {
            CheckDirectionChange();
            CheckVelocityChange();
            
            if (CurrentDirection.magnitude < 0.05f)
                return;

            Move(deltaTime);
        }

        private void Move(float deltaTime)
        {
            _characterController.Move(CurrentVelocity * deltaTime);
            Moved?.Invoke();
        }

        private void CheckDirectionChange()
        {
            if (Vector3.Distance(_lastDirection, CurrentDirection) > 0.01f)
            {
                DirectionChanged?.Invoke(CurrentDirection);
                _lastDirection = CurrentDirection;
            }
        }
        
        private void CheckVelocityChange()
        {
            if (Vector3.Distance(_lastVelocity, CurrentVelocity) > 0.01f)
            {
                VelocityChanged?.Invoke(CurrentVelocity);
                _lastVelocity = CurrentVelocity;
            }
        }
    }
}