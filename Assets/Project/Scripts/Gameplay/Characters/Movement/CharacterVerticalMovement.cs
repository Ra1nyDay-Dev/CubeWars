using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Movement
{
    public class CharacterVerticalMovement
    {
        public bool IsGrounded => 
            _controller.isGrounded;

        public float CurrentVelocity {get; private set;}
        
        public event Action<bool> GroundedChanged;
        public event Action<float> VelocityChanged;
        public event Action Jumped;
        
        private readonly CharacterController _controller;
        private readonly float _gravity;
        private readonly float _groundDownForce;
        private readonly float _jumpHeight;
        
        private bool _lastGrounded;
        private float _lastVelocity;

        public CharacterVerticalMovement(CharacterController controller, float gravity, float groundDownForce, float jumpHeight)
        {
            _gravity = gravity;
            _groundDownForce = groundDownForce;
            _jumpHeight = jumpHeight;
            _controller = controller;
        }
        
        public void Update(float deltaTime)
        {
            ApplyGroundForce();
            ApplyGravity(deltaTime);

            CheckGroundedChange();
            CheckVelocityChange();
        }

        public void Jump()
        {
            if (IsGrounded)
            {
                CurrentVelocity = Mathf.Sqrt(_jumpHeight * _groundDownForce * _gravity);
                Jumped?.Invoke();
            }
        }

        private void ApplyGroundForce()
        {
            if (IsGrounded && CurrentVelocity < 0)
                CurrentVelocity = _groundDownForce;
        }

        private void ApplyGravity(float deltaTime)
        {
            CurrentVelocity += _gravity * deltaTime;
            _controller.Move(Vector3.up * (CurrentVelocity * deltaTime));
        }

        private void CheckGroundedChange()
        {
            if (_lastGrounded != IsGrounded)
            {
                GroundedChanged?.Invoke(IsGrounded);
                _lastGrounded = IsGrounded;
            }
        }

        private void CheckVelocityChange()
        {
            if (!Mathf.Approximately(_lastVelocity, CurrentVelocity))
            {
                VelocityChanged?.Invoke(CurrentVelocity);
                _lastVelocity = CurrentVelocity;
            }
        }
    }
}