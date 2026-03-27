using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Movement
{
    public class CharacterDirectionalMovement
    {
        public event Action<Vector3> DirectionChanged;
        public event Action<Vector3> VelocityChanged;
        public event Action<bool> IsMovingChanged;
        
        public Vector3 CurrentDirection {get; private set;}
        public bool IsMoving { get; private set; }
        
        private readonly CharacterController _characterController;
        private readonly float _movementSpeed;
        
        private Vector3 _lastDirection = Vector3.zero; 
        private Vector3 _lastVelocity = Vector3.zero;
        private bool _lastMoving = false;

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
            CheckMovingChange();

            if (CurrentDirection.magnitude < 0.05f)
            {
                IsMoving = false;
                return;
            }

            Move(deltaTime);
        }

        private void Move(float deltaTime)
        {
            _characterController.Move(CurrentVelocity * deltaTime);
            IsMoving = true;
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

        private void CheckMovingChange()
        {
            if (_lastMoving != IsMoving)
            {
                IsMovingChanged?.Invoke(IsMoving);
                _lastMoving = IsMoving;
            }
        }
    }
}