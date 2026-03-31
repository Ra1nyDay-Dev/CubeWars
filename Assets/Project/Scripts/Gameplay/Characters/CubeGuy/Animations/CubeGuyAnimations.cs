using System;
using DG.Tweening;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.CubeGuy.Animations
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Animator))]
    public class CubeGuyAnimations : MonoBehaviour
    {
        [SerializeField] private GameObject _animatorMesh;
        [SerializeField] private GameObject _tweensMesh;
        
        private static readonly int JumpedHash = Animator.StringToHash("Jumped");
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int HorizontalSpeedHash = Animator.StringToHash("HorizontalSpeed");
        private static readonly int HorizontalSpeedXHash = Animator.StringToHash("HorizontalSpeedX");
        private static readonly int HorizontalSpeedZHash = Animator.StringToHash("HorizontalSpeedZ");
        private static readonly int FlipXHash = Animator.StringToHash("FlipX");
        private static readonly int FlipZHash = Animator.StringToHash("FlipZ");
        private static readonly int VerticalVelocityHash = Animator.StringToHash("VerticalVelocity");
        private static readonly int DeadHash = Animator.StringToHash("Dead");
        private static readonly int IsDeadHash = Animator.StringToHash("IsDead");
        
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");

        private Animator _animator;
        private Character _character;
        private IDamageable _damageable;
        private Death _death;
        private Renderer _renderer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private Sequence _hitSequence;
        
        private Vector3 _lastHorizontalVelocity;
        private Vector3 _jumpDirection;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _character = GetComponent<Character>();
            _damageable = _character.GetComponent<IDamageable>();
            _death = _character.GetComponent<Death>();
            _renderer = _tweensMesh.GetComponent<Renderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();
            SetTweens();
        }

        private void OnEnable()
        {
            _character.MovingChanged += OnMovingChanged;
            _character.HorizontalVelocityChanged += OnHorizontalVelocityChanged;
            _character.GroundedChanged += OnGroundedChanged;
            _character.Jumped += OnJumped;
            _character.RotationChanged += OnRotationChanged;
            _character.VerticalVelocityChanged += OnVerticalVelocityChanged;
            _damageable.Damaged += OnHit;
            _death.Happened += OnDie;
        }

        private void OnDisable()
        {
            _character.MovingChanged -= OnMovingChanged;
            _character.HorizontalVelocityChanged -= OnHorizontalVelocityChanged;
            _character.GroundedChanged -= OnGroundedChanged;
            _character.Jumped -= OnJumped;
            _character.RotationChanged -= OnRotationChanged;
            _damageable.Damaged -= OnHit;
            _death.Happened -= OnDie;
        }

        private void OnDestroy() => 
            _hitSequence.Kill();

        private void OnDie()
        {
            _animator.SetBool(IsDeadHash, true);
            _animator.SetTrigger(DeadHash);
        }

        private void OnVerticalVelocityChanged(float verticalVelocity) => 
            _animator.SetFloat(VerticalVelocityHash, verticalVelocity);

        private void OnHorizontalVelocityChanged(Vector3 velocity)
        {
            _lastHorizontalVelocity = velocity;
            UpdateMovementAnimation(velocity);
        }

        private void OnRotationChanged(Quaternion rotation) => 
            UpdateMovementAnimation(_lastHorizontalVelocity);

        private void UpdateMovementAnimation(Vector3 worldVelocity)
        {
            if (!_character.IsGrounded)
                return;
            
            Vector3 localVelocity = Quaternion.Inverse(_character.CurrentRotation) * worldVelocity;
            _animator.SetFloat(HorizontalSpeedHash, worldVelocity.magnitude);
            _animator.SetFloat(HorizontalSpeedXHash, localVelocity.x);
            _animator.SetFloat(HorizontalSpeedZHash, localVelocity.z);
        }

        private void OnJumped()
        {
            Vector3 moveDirection = _character.CurrentHorizontalVelocity;

            if (moveDirection.magnitude > 0.1f)
                _jumpDirection = moveDirection.normalized;
            else
                _jumpDirection = Vector3.zero;

            Vector3 localMoveDir = Quaternion.Inverse(_character.CurrentRotation) * _jumpDirection;

            _animator.SetFloat(FlipXHash, localMoveDir.x);
            _animator.SetFloat(FlipZHash, localMoveDir.z);
            _animator.SetTrigger(JumpedHash);
        }

        private void OnMovingChanged(bool isMoving) => 
            _animator.SetBool(IsMovingHash, isMoving);

        private void OnGroundedChanged(bool isGrounded)
        {
            _animator.SetBool(IsGroundedHash, isGrounded);

            if (isGrounded)
            {
                _animator.SetTrigger(Grounded);
            }
        }

        private void SetTweens()
        {
            _hitSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .SetUpdate(UpdateType.Late);
                
            float flash = 0;
            
            _hitSequence.Append(
                DOTween.To(
                        () => flash,
                        x => {
                            flash = x;
                            SetFlash(x);
                        },
                        1f,
                        0.1f
                    ).SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.OutQuad)
            ).Join(
                _tweensMesh.transform.DOPunchPosition(Vector3.forward * 0.5f, 0.4f, 10, 1)
            ).Join(
                _tweensMesh.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 1)
            );
        }
        
        private void SetFlash(float value)
        {
            _renderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(FlashAmount, value);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void OnHit(Vector3 hitDirection) => 
            _hitSequence.Restart();
        
    }
}