using DG.Tweening;
using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.CharacterSystems.Movement;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Animations
{
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimations : MonoBehaviour
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
        private static readonly int DeathType = Animator.StringToHash("DeathType");
        
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");

        private Animator _animator;
        private CharacterMovement _characterMovement;
        private IDamageable _damageable;
        private RespawnBehaviour _respawnBehaviour;
        private Renderer _renderer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private Sequence _damagedSequence;
        
        private Vector2 _lastHorizontalVelocity;
        private Vector3 _jumpDirection;

        public void Awake()
        {
            _animator = GetComponent<Animator>();
            _characterMovement = GetComponent<CharacterMovement>();
            _damageable = _characterMovement.GetComponent<IDamageable>();
            _respawnBehaviour = _characterMovement.GetComponent<RespawnBehaviour>();
            _renderer = _tweensMesh.GetComponent<Renderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();
            SetTweens();
        }

        private void OnEnable() => 
            SubscribeToEvents();
        private void OnDisable() =>
            UnsubscribeFromEvents();

        private void OnDestroy() => 
            _damagedSequence.Kill();

        private void SubscribeToEvents()
        {
            _characterMovement.MovingChanged += OnMovingChanged;
            _characterMovement.HorizontalVelocityChanged += OnHorizontalVelocityChanged;
            _characterMovement.GroundedChanged += OnGroundedChanged;
            _characterMovement.Jumped += OnJumped;
            _characterMovement.RotationChanged += OnRotationChanged;
            _characterMovement.VerticalVelocityChanged += OnVerticalVelocityChanged;
            _damageable.Damaged += OnDamaged;
            _respawnBehaviour.Dead += OnDie;
        }

        private void UnsubscribeFromEvents()
        {
            _characterMovement.MovingChanged -= OnMovingChanged;
            _characterMovement.HorizontalVelocityChanged -= OnHorizontalVelocityChanged;
            _characterMovement.GroundedChanged -= OnGroundedChanged;
            _characterMovement.Jumped -= OnJumped;
            _characterMovement.RotationChanged -= OnRotationChanged;
            _damageable.Damaged -= OnDamaged;
            _respawnBehaviour.Dead -= OnDie;
        }

        private void OnDie(DamageData damageData)
        {
            _animator.SetInteger(DeathType, (int)damageData.DeathType);  
            _animator.SetBool(IsDeadHash, true);
            _animator.SetTrigger(DeadHash);
        }

        private void OnVerticalVelocityChanged(float verticalVelocity) => 
            _animator.SetFloat(VerticalVelocityHash, verticalVelocity);

        private void OnHorizontalVelocityChanged(Vector2 velocity)
        {
            _lastHorizontalVelocity = velocity;
            UpdateMovementAnimation(velocity);
        }

        private void OnRotationChanged(Quaternion rotation) => 
            UpdateMovementAnimation(_lastHorizontalVelocity);

        private void UpdateMovementAnimation(Vector2 worldVelocity)
        {
            if (!_characterMovement.IsGrounded)
                return;
            
            Vector3 worldDirection = new Vector3(worldVelocity.x, 0, worldVelocity.y);
            Vector3 localDirection = _characterMovement.transform.InverseTransformDirection(worldDirection);

            _animator.SetFloat(HorizontalSpeedHash, worldDirection.magnitude);
            _animator.SetFloat(HorizontalSpeedXHash, localDirection.x);
            _animator.SetFloat(HorizontalSpeedZHash, localDirection.z);
        }

        private void OnJumped()
        {
            Vector2 moveDirection = _characterMovement.CurrentHorizontalVelocity;
            Vector3 worldDirection  = new Vector3(moveDirection.x, 0, moveDirection.y);
            
            if (worldDirection.magnitude > 0.1f)
                _jumpDirection = worldDirection.normalized;
            else
                _jumpDirection = Vector3.zero;

            Vector3 localDirection = _characterMovement.transform.InverseTransformDirection(_jumpDirection);

            _animator.SetFloat(FlipXHash, localDirection.x);
            _animator.SetFloat(FlipZHash, localDirection.z);
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
            _damagedSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .SetUpdate(UpdateType.Late);
                
            float flash = 0;
            
            _damagedSequence.Append(
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

        private void OnDamaged(DamageData damageData) => 
            _damagedSequence.Restart();
        
    }
}