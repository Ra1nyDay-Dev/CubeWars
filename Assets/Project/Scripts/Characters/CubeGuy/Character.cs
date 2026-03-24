using System;
using Project.Scripts.Characters.CubeGuy.Movement;
using UnityEngine;

namespace Project.Scripts.Characters.CubeGuy
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 20f;
        [SerializeField] private float _gravity = Physics.gravity.y;
        [SerializeField] private float _groundDownForce = -2f;
        [SerializeField] private float _jumpHeight = 3f;

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
        
        public event Action<Vector3> MovementDirectionChanged;
        public event Action<Vector3> HorizontalVelocityChanged;
        public event Action Moved;
        
        public event Action<Quaternion> RotationChanged;
        
        public event Action<bool> GroundedChanged;
        public event Action<float> VerticalVelocityChanged;
        public event Action Jumped;
        
        private CharacterDirectionalMovement _directionalMovement;
        private DirectionalRotation _rotation;
        private CharacterVerticalMovement _verticalMovement;
        
        private void Awake()
        {
            var controller = GetComponent<CharacterController>();
            
            _directionalMovement = new CharacterDirectionalMovement(controller, _movementSpeed);
            _rotation = new DirectionalRotation(transform, _rotationSpeed);
            _verticalMovement = new CharacterVerticalMovement(controller, _gravity, _groundDownForce, _jumpHeight);
        }

        private void FixedUpdate()
        {
            _directionalMovement.Update(Time.fixedDeltaTime);
            _rotation.Update(Time.fixedDeltaTime);
            _verticalMovement.Update(Time.fixedDeltaTime);
        }

        private void OnEnable()
        {
            _directionalMovement.DirectionChanged += OnDirectionChanged;
            _directionalMovement.VelocityChanged += OnHorizontalVelocityChanged;
            _directionalMovement.Moved += OnMoved;

            _rotation.RotationChanged += OnRotationChanged;

            _verticalMovement.GroundedChanged += OnGroundedChanged;
            _verticalMovement.VelocityChanged += OnVerticalVelocityChanged;
            _verticalMovement.Jumped += OnJumped;
        }

        private void OnDisable()
        {
            _directionalMovement.DirectionChanged -= OnDirectionChanged;
            _directionalMovement.VelocityChanged -= OnHorizontalVelocityChanged;
            _directionalMovement.Moved -= OnMoved;

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
        
        private void OnDirectionChanged(Vector3 direction) => MovementDirectionChanged?.Invoke(direction);
        private void OnHorizontalVelocityChanged(Vector3 velocity) => HorizontalVelocityChanged?.Invoke(velocity);
        private void OnMoved() => Moved?.Invoke();
        private void OnRotationChanged(Quaternion rotation) => RotationChanged?.Invoke(rotation);
        private void OnGroundedChanged(bool isGrounded) => GroundedChanged?.Invoke(isGrounded);
        private void OnVerticalVelocityChanged(float velocity) => VerticalVelocityChanged?.Invoke(velocity);
        private void OnJumped() => Jumped?.Invoke();
    }
}