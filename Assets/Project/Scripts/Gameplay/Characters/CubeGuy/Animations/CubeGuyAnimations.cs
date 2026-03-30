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
        [SerializeField] private GameObject _animatedMesh;
        
        private static readonly int JumpedHash = Animator.StringToHash("Jumped");
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int HorizontalSpeedHash = Animator.StringToHash("HorizontalSpeed");
        private static readonly int HorizontalSpeedXHash = Animator.StringToHash("HorizontalSpeedX");
        private static readonly int HorizontalSpeedZHash = Animator.StringToHash("HorizontalSpeedZ");
        private static readonly int FlipXHash = Animator.StringToHash("FlipX");
        private static readonly int FlipZHash = Animator.StringToHash("FlipZ");
        private static readonly int VerticalVelocityHash = Animator.StringToHash("VerticalVelocity");
        
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");

        private Animator _animator;
        private Character _character;
        private IDamageable _damageable;
        private Renderer _renderer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private Sequence _hitSequence;
        
        private Vector3 _lastHorizontalVelocity;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _character = GetComponent<Character>();
            _damageable = _character.GetComponent<IDamageable>();
            _renderer = _animatedMesh.GetComponent<Renderer>();
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
        }

        private void OnDisable()
        {
            _character.MovingChanged -= OnMovingChanged;
            _character.HorizontalVelocityChanged -= OnHorizontalVelocityChanged;
            _character.GroundedChanged -= OnGroundedChanged;
            _character.Jumped -= OnJumped;
            _character.RotationChanged -= OnRotationChanged;
            _damageable.Damaged -= OnHit;
        }

        private void OnDestroy() => 
            _hitSequence.Kill();

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
            Vector3 localVelocity = Quaternion.Inverse(_character.CurrentRotation) * worldVelocity;
            _animator.SetFloat(HorizontalSpeedHash, worldVelocity.magnitude);
            _animator.SetFloat(HorizontalSpeedXHash, localVelocity.x);
            _animator.SetFloat(HorizontalSpeedZHash, localVelocity.z);
        }

        private void OnJumped()
        {
            Vector3 moveDirection = _character.CurrentMovementDirection;
    
            if (moveDirection.magnitude > 0.1f)
            {
                Vector3 localMoveDir = Quaternion.Inverse(_character.CurrentRotation) * moveDirection.normalized;
                _animator.SetFloat(FlipXHash, localMoveDir.x);
                _animator.SetFloat(FlipZHash, localMoveDir.z);
            }
            else
            {
                _animator.SetFloat(FlipXHash, 0f);
                _animator.SetFloat(FlipZHash, 0f);
            }
    
            _animator.SetTrigger(JumpedHash);
        }

        private void OnMovingChanged(bool isMoving) => 
            _animator.SetBool(IsMovingHash, isMoving);

        private void OnGroundedChanged(bool isGrounded) => 
            _animator.SetBool(IsGroundedHash, isGrounded);

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
            );
            
            _hitSequence.Join(
                _animatedMesh.transform.DOPunchPosition(Vector3.forward * 0.5f, 0.4f, 10, 1)
            );
            
            _hitSequence.Join(
                _animatedMesh.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 1)
            );
        }
        
        private void SetFlash(float value)
        {
            _renderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(FlashAmount, value);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void OnHit()
        {
            _hitSequence.Restart();
        }
    }
}