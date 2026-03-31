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
        private readonly float _acceleration;
        private readonly float _groundDeceleration;
        private readonly float _airDeceleration;
        
        private Vector3 _lastDirection = Vector3.zero; 
        private Vector3 _lastVelocity = Vector3.zero;
        private bool _lastMoving = false;
        private Vector3 _currentVelocity;
        private Vector3 _externalVelocity;

        public CharacterDirectionalMovement(CharacterController characterController, float movementSpeed,
            float acceleration, float groundDeceleration, float airDeceleration)
        {
            _characterController = characterController;
            _movementSpeed = movementSpeed;
            _acceleration = acceleration;
            _groundDeceleration = groundDeceleration;
            _airDeceleration = airDeceleration;
        }

        public Vector3 CurrentVelocity => 
            _currentVelocity;
        
        public Vector3 ExternalVelocity => 
            _externalVelocity;
        
        public void SetDirection(Vector3 direction) => 
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

            _externalVelocity = Vector3.MoveTowards(
                _externalVelocity,
                Vector3.zero,
                deceleration * deltaTime
            );
        }
        
        public void AddForce(Vector3 force) => 
            _externalVelocity = force;

        private void Move(float deltaTime, bool isGrounded)
        {
            Vector3 targetVelocity = CurrentDirection.normalized * _movementSpeed;
            float deceleration = isGrounded ? _groundDeceleration : _airDeceleration;

            if (CurrentDirection.magnitude > 0.05f)
                Accelerate(targetVelocity, deltaTime);
            else
                Decelerate(targetVelocity, deceleration, deltaTime);

            IsMoving = CurrentDirection.magnitude > 0.05f;
        }

        private void Accelerate(Vector3 targetVelocity, float deltaTime)
        {
            _currentVelocity =  Vector3.MoveTowards(
                _currentVelocity,
                targetVelocity,
                _acceleration * deltaTime);
        }

        private void Decelerate(Vector3 targetVelocity, float deceleration, float deltaTime)
        {
            _currentVelocity = Vector3.MoveTowards(
                _currentVelocity,
                targetVelocity,
                deceleration * deltaTime);
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