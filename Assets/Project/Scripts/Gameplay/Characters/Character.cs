using System;
using Project.Scripts.Gameplay.Characters.Movement;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 10f;
        [SerializeField] private float _acceleration = 100f;
        [SerializeField] private float _groundDeceleration = 50f;
        [SerializeField] private float _airDeceleration = 10f;
        [SerializeField] private float _rotationSpeed = 20f;
        [SerializeField] private float _gravity = Physics.gravity.y;
        [SerializeField] private float _groundDownForce = -2f;
        [SerializeField] private float _jumpHeight = 3f;
        
        [SerializeField] private float _backwardForceTest;
        [SerializeField] private float _upwardForceTest;

        public Vector3 CurrentHorizontalVelocity => 
            _directionalMovement.CurrentVelocity;
        
        public Vector3 CurrentMovementDirection => 
            _directionalMovement.CurrentDirection;

        public float CurrentVerticalVelocity => 
            _verticalMovement.CurrentVelocity;
        
        public Quaternion CurrentRotation => 
            _rotation.CurrentRotation;
        
        public bool IsGrounded => 
            _verticalMovement.IsGrounded;
        
        public bool IsMoving => 
            _directionalMovement.IsMoving;
        
        public event Action<Vector3> MovementDirectionChanged;
        public event Action<Vector3> HorizontalVelocityChanged;
        public event Action<bool> MovingChanged;
        
        public event Action<Quaternion> RotationChanged;
        
        public event Action<bool> GroundedChanged;
        public event Action<float> VerticalVelocityChanged;
        public event Action Jumped;
        
        private CharacterController _controller;
        
        private CharacterDirectionalMovement _directionalMovement;
        private DirectionalRotation _rotation;
        private CharacterVerticalMovement _verticalMovement;
        
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _directionalMovement = new CharacterDirectionalMovement(_controller, _movementSpeed, _acceleration,
                _groundDeceleration, _airDeceleration);
            _rotation = new DirectionalRotation(transform, _rotationSpeed);
            _verticalMovement = new CharacterVerticalMovement(_controller, _gravity, _groundDownForce, _jumpHeight);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                
                Vector3 force =
                    -transform.forward * _backwardForceTest +
                    Vector3.up * _upwardForceTest;
                AddForce(force);
            }
        }

        private void FixedUpdate()
        {
            _verticalMovement.Update(Time.fixedDeltaTime);
            _directionalMovement.Update(Time.fixedDeltaTime, IsGrounded);
            _rotation.Update(Time.fixedDeltaTime);
            
            Vector3 finalVelocity =
                _directionalMovement.CurrentVelocity +
                _directionalMovement.ExternalVelocity +
                Vector3.up * _verticalMovement.CurrentVelocity;
            
            _controller.Move(finalVelocity * Time.fixedDeltaTime);
        }

        private void OnEnable()
        {
            _directionalMovement.DirectionChanged += OnDirectionChanged;
            _directionalMovement.VelocityChanged += OnHorizontalVelocityChanged;
            _directionalMovement.IsMovingChanged += OnMovingChanged;

            _rotation.RotationChanged += OnRotationChanged;

            _verticalMovement.GroundedChanged += OnGroundedChanged;
            _verticalMovement.VelocityChanged += OnVerticalVelocityChanged;
            _verticalMovement.Jumped += OnJumped;
        }

        private void OnDisable()
        {
            _directionalMovement.DirectionChanged -= OnDirectionChanged;
            _directionalMovement.VelocityChanged -= OnHorizontalVelocityChanged;
            _directionalMovement.IsMovingChanged -= OnMovingChanged;

            _rotation.RotationChanged -= OnRotationChanged;

            _verticalMovement.GroundedChanged -= OnGroundedChanged;
            _verticalMovement.VelocityChanged -= OnVerticalVelocityChanged;
            _verticalMovement.Jumped -= OnJumped;
        }

        public void SetMoveDirection(Vector3 direction) => 
            _directionalMovement.SetDirection(direction);
        
        public void SetRotationDirection(Vector3 direction) => 
            _rotation.SetDirection(direction);
        
        public void Jump() => 
            _verticalMovement.Jump();
        
        public void AddForce(Vector3 force)
        {
            _directionalMovement.AddForce(force);
            _verticalMovement.AddForce(force.y);
        }

        private void OnDirectionChanged(Vector3 direction) => MovementDirectionChanged?.Invoke(direction);
        private void OnHorizontalVelocityChanged(Vector3 velocity) => HorizontalVelocityChanged?.Invoke(velocity);
        private void OnMovingChanged(bool isMoving) => MovingChanged?.Invoke(isMoving);
        private void OnRotationChanged(Quaternion rotation) => RotationChanged?.Invoke(rotation);
        private void OnGroundedChanged(bool isGrounded) => GroundedChanged?.Invoke(isGrounded);
        private void OnVerticalVelocityChanged(float velocity) => VerticalVelocityChanged?.Invoke(velocity);
        private void OnJumped() => Jumped?.Invoke();
    }
}