using System;
using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        public Vector2 CurrentHorizontalVelocity => 
            _directionalMovement.CurrentVelocity;
        
        public Vector2 CurrentMovementDirection => 
            _directionalMovement.CurrentDirection;

        public float CurrentVerticalVelocity => 
            _verticalMovement.CurrentVelocity;
        
        public Quaternion CurrentRotation => 
            _rotation.CurrentRotation;
        
        public bool IsGrounded => 
            _verticalMovement.IsGrounded;
        
        public bool IsMoving => 
            _directionalMovement.IsMoving;
        
        public float MovementSpeed { get; private set; }
        public float Acceleration { get; private set; }
        
        public event Action<Vector2> MovementDirectionChanged;
        public event Action<Vector2> HorizontalVelocityChanged;
        public event Action<bool> MovingChanged;
        
        public event Action<Quaternion> RotationChanged;
        
        public event Action<bool> GroundedChanged;
        public event Action<float> VerticalVelocityChanged;
        public event Action Jumped;

        private float _groundDeceleration;
        private float _airDeceleration;
        private float _rotationSpeed;
        private float _gravity;
        private float _groundDownForce;
        private float _jumpHeight;
        
        private CharacterController _controller;
        private RespawnBehaviour _respawnBehaviour;
        
        private CharacterDirectionalMovement _directionalMovement;
        private DirectionalRotation _rotation;
        private CharacterVerticalMovement _verticalMovement;

        public CharacterMovement(float acceleration)
        {
            Acceleration = acceleration;
        }

        public void Construct(CharacterMovementConfig config)
        {
            MovementSpeed = config.MovementSpeed;
            Acceleration =  config.Acceleration;
            _groundDeceleration = config.GroundDeceleration;
            _airDeceleration = config.AirDeceleration;
            _rotationSpeed = config.RotationSpeed;
            _gravity = config.Gravity;
            _groundDownForce = config.GroundDownForce;
            _jumpHeight = config.JumpHeight;
        }

        public void Initialize()
        {
            _controller = GetComponent<CharacterController>();
            _respawnBehaviour = GetComponent<RespawnBehaviour>();
            
            _directionalMovement = new CharacterDirectionalMovement(_controller, MovementSpeed, Acceleration,
                _groundDeceleration, _airDeceleration);
            _rotation = new DirectionalRotation(transform, _rotationSpeed);
            _verticalMovement = new CharacterVerticalMovement(_controller, _gravity, _groundDownForce, _jumpHeight);
            
            SubscribeToEvents();
        }

        private void FixedUpdate()
        {
            _verticalMovement.Update(Time.fixedDeltaTime);
            _directionalMovement.Update(Time.fixedDeltaTime, IsGrounded);
            _rotation.Update(Time.fixedDeltaTime);

            Vector3 finalVelocity = new Vector3(
                _directionalMovement.FinalVelocity.x,
                _verticalMovement.CurrentVelocity,
                _directionalMovement.FinalVelocity.y);
            
            _controller.Move(finalVelocity * Time.fixedDeltaTime);
        }

        private void OnDestroy() => 
            UnsubscribeFromEvents();

        public void SetMoveDirection(Vector2 direction) => 
            _directionalMovement.SetDirection(direction);
        
        public void SetRotationDirection(Vector3 direction) => 
            _rotation.SetDirection(direction);
        
        public void Jump() => 
            _verticalMovement.Jump();
        
        public void AddForce(Vector3 force)
        {
            _directionalMovement.AddForce(new Vector2(force.x,force.z));
            _verticalMovement.AddForce(force.y);
        }

        private void SubscribeToEvents()
        {
            _directionalMovement.DirectionChanged += OnDirectionChanged;
            _directionalMovement.VelocityChanged += OnHorizontalVelocityChanged;
            _directionalMovement.IsMovingChanged += OnMovingChanged;

            _rotation.RotationChanged += OnRotationChanged;

            _verticalMovement.GroundedChanged += OnGroundedChanged;
            _verticalMovement.VelocityChanged += OnVerticalVelocityChanged;
            _verticalMovement.Jumped += OnJumped;

            _respawnBehaviour.Respawned += OnRespawn;
        }

        private void OnRespawn()
        {
            _controller.enabled = false;
            _directionalMovement.Reset();
            _rotation.Reset();
            _verticalMovement.Reset();
            _controller.enabled = true;
        }

        private void UnsubscribeFromEvents()
        {
            _directionalMovement.DirectionChanged -= OnDirectionChanged;
            _directionalMovement.VelocityChanged -= OnHorizontalVelocityChanged;
            _directionalMovement.IsMovingChanged -= OnMovingChanged;

            _rotation.RotationChanged -= OnRotationChanged;

            _verticalMovement.GroundedChanged -= OnGroundedChanged;
            _verticalMovement.VelocityChanged -= OnVerticalVelocityChanged;
            _verticalMovement.Jumped -= OnJumped;
        }

        private void OnDirectionChanged(Vector2 direction) => MovementDirectionChanged?.Invoke(direction);
        private void OnHorizontalVelocityChanged(Vector2 velocity) => HorizontalVelocityChanged?.Invoke(velocity);
        private void OnMovingChanged(bool isMoving) => MovingChanged?.Invoke(isMoving);
        private void OnRotationChanged(Quaternion rotation) => RotationChanged?.Invoke(rotation);
        private void OnGroundedChanged(bool isGrounded) => GroundedChanged?.Invoke(isGrounded);
        private void OnVerticalVelocityChanged(float velocity) => VerticalVelocityChanged?.Invoke(velocity);
        private void OnJumped() => Jumped?.Invoke();
    }
}