using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Movement
{
    public class CharacterDirectionalMovement
    {
        public event Action<Vector2> DirectionChanged;
        public event Action<Vector2> VelocityChanged;
        public event Action<bool> IsMovingChanged;
        
        public Vector2 CurrentDirection {get; private set;}
        public bool IsMoving { get; private set; }
        
        private readonly CharacterController _characterController;
        private readonly float _movementSpeed;
        private readonly float _acceleration;
        private readonly float _groundDeceleration;
        private readonly float _airDeceleration;
        
        private Vector2 _lastDirection = Vector2.zero; 
        private Vector2 _lastVelocity = Vector2.zero;
        private bool _lastMoving = false;
        private Vector2 _currentVelocity;
        private Vector2 _externalVelocity;

        public CharacterDirectionalMovement(CharacterController characterController, float movementSpeed,
            float acceleration, float groundDeceleration, float airDeceleration)
        {
            _characterController = characterController;
            _movementSpeed = movementSpeed;
            _acceleration = acceleration;
            _groundDeceleration = groundDeceleration;
            _airDeceleration = airDeceleration;
        }

        public Vector2 CurrentVelocity => 
            _currentVelocity;
        
        public Vector2 ExternalVelocity => 
            _externalVelocity;

        public Vector2 FinalVelocity =>
            _currentVelocity + _externalVelocity;
        
        public void SetDirection(Vector2 direction) => 
            CurrentDirection = direction;

        public void Update(float deltaTime, bool isGrounded = false)
        {
            Move(deltaTime, isGrounded);
            UpdateExternalVelocity(deltaTime, isGrounded);
            CheckDirectionChange();
            CheckVelocityChange();
            CheckMovingChange();
        }
        
        private void UpdateExternalVelocity(float deltaTime, bool isGrounded)
        {
            float deceleration = isGrounded ? _groundDeceleration : _airDeceleration;

            _externalVelocity = Vector2.MoveTowards(
                _externalVelocity,
                Vector2.zero,
                deceleration * deltaTime
            );
        }
        
        public void AddForce(Vector2 force) => 
            _externalVelocity = force;

        private void Move(float deltaTime, bool isGrounded)
        {
            Vector2 targetVelocity = CurrentDirection.normalized * _movementSpeed;
            float deceleration = isGrounded ? _groundDeceleration : _airDeceleration;

            if (CurrentDirection.magnitude > 0.05f)
                Accelerate(targetVelocity, deltaTime);
            else
                Decelerate(targetVelocity, deceleration, deltaTime);

            IsMoving = CurrentDirection.magnitude > 0.05f;
        }

        private void Accelerate(Vector2 targetVelocity, float deltaTime)
        {
            _currentVelocity =  Vector2.MoveTowards(
                _currentVelocity,
                targetVelocity,
                _acceleration * deltaTime);
        }

        private void Decelerate(Vector2 targetVelocity, float deceleration, float deltaTime)
        {
            _currentVelocity = Vector2.MoveTowards(
                _currentVelocity,
                targetVelocity,
                deceleration * deltaTime);
        }

        private void CheckDirectionChange()
        {
            if (Vector2.Distance(_lastDirection, CurrentDirection) > 0.01f)
            {
                DirectionChanged?.Invoke(CurrentDirection);
                _lastDirection = CurrentDirection;
            }
        }

        private void CheckVelocityChange()
        {
            if (Vector2.Distance(_lastVelocity, CurrentVelocity) > 0.01f)
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