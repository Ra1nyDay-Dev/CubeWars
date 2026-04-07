using System;
using System.Threading.Tasks;
using Project.Scripts.Gameplay.Characters;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimation : MonoBehaviour
    {
        private static readonly int PrimaryAttack1Hash = Animator.StringToHash("PrimaryAttack1");
        private static readonly int PrimaryAttack2Hash = Animator.StringToHash("PrimaryAttack2");
        private static readonly int SecondaryAttackHash = Animator.StringToHash("SecondaryAttack");
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        private static readonly int HorizontalSpeedHash = Animator.StringToHash("HorizontalSpeed");
        private static readonly int HorizontalSpeedXHash = Animator.StringToHash("HorizontalSpeedX");
        private static readonly int HorizontalSpeedZHash = Animator.StringToHash("HorizontalSpeedZ");
        private static readonly int JumpedHash = Animator.StringToHash("Jumped");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int VerticalVelocityHash = Animator.StringToHash("VerticalVelocity");
        private static readonly int Reload = Animator.StringToHash("Reload");

        private Animator _animator;
        private IWeapon _weapon;
        private Character _owner;
        
        private Vector3 _lastHorizontalVelocity;
        private int _lastPrimaryAttackIndex = 1;

        public void Construct(IWeapon weapon, Character owner)
        {
            _weapon = weapon;
            _owner = owner;
            
            _weapon.PrimaryAttackStarted += OnPrimaryAttackStarted;
            _weapon.PrimaryAttackEnded += OnPrimaryAttackEnded;
            _weapon.SecondaryAttackStarted += OnSecondaryAttackStarted;
            _weapon.SecondaryAttackEnded += OnSecondaryAttackEnded;
            _owner.MovingChanged += OnMovingChanged;
            _owner.HorizontalVelocityChanged += OnHorizontalVelocityChanged;
            _owner.GroundedChanged += OnGroundedChanged;
            _owner.Jumped += OnJumped;
            _owner.VerticalVelocityChanged += OnVerticalVelocityChanged;

            if (_weapon is RangeWeapon rangeWeapon)
            {
                if (rangeWeapon.IsReloadable) 
                    rangeWeapon.ReloadStarted += OnReload;
            }
            
            _animator.SetBool(IsGroundedHash, _owner.IsGrounded);
            _animator.SetBool(IsMovingHash, _owner.IsMoving);
            _animator.SetFloat(HorizontalSpeedHash, _owner.CurrentHorizontalVelocity.magnitude);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            _weapon.PrimaryAttackStarted -= OnPrimaryAttackStarted;
            _weapon.PrimaryAttackEnded -= OnPrimaryAttackEnded;
            _weapon.SecondaryAttackStarted -= OnSecondaryAttackStarted;
            _weapon.SecondaryAttackEnded -= OnSecondaryAttackEnded;
            _owner.MovingChanged -= OnMovingChanged;
            _owner.HorizontalVelocityChanged -= OnHorizontalVelocityChanged;
            _owner.GroundedChanged -= OnGroundedChanged;
            _owner.Jumped -= OnJumped;
            _owner.VerticalVelocityChanged -= OnVerticalVelocityChanged;
            
            if (_weapon is RangeWeapon rangeWeapon)
            {
                if (rangeWeapon.IsReloadable) 
                    rangeWeapon.ReloadStarted -= OnReload;
            }
        }

        private void OnJumped() => 
            _animator.SetTrigger(JumpedHash);

        private void OnPrimaryAttackStarted()
        {
            int primaryAttack = Animator.StringToHash($"PrimaryAttack{_lastPrimaryAttackIndex}");
            _animator.SetTrigger(primaryAttack);
            
            if (_weapon.PrimaryAttack.AttackAnimationsCount > 1)
                _lastPrimaryAttackIndex = _lastPrimaryAttackIndex == 1 ? 2 : 1;
        }

        private void OnSecondaryAttackEnded() => 
            _animator.speed = 1f;

        private void OnSecondaryAttackStarted() =>
            _animator.SetTrigger(SecondaryAttackHash);

        private void OnPrimaryAttackEnded() => 
            _animator.speed = 1f;

        private void OnMovingChanged(bool isMoving) => 
            _animator.SetBool(IsMovingHash, isMoving);

        private void OnVerticalVelocityChanged(float verticalVelocity) => 
            _animator.SetFloat(VerticalVelocityHash, verticalVelocity);

        private void OnHorizontalVelocityChanged(Vector3 velocity)
        {
            _lastHorizontalVelocity = velocity;
            UpdateMovementAnimation(velocity);
        }

        private void UpdateMovementAnimation(Vector3 worldVelocity)
        {
            Vector3 localVelocity = Quaternion.Inverse(_owner.CurrentRotation) * worldVelocity;
            _animator.SetFloat(HorizontalSpeedHash, worldVelocity.magnitude);
            _animator.SetFloat(HorizontalSpeedXHash, localVelocity.x);
            _animator.SetFloat(HorizontalSpeedZHash, localVelocity.z);
        }

        private void OnGroundedChanged(bool isGrounded) => 
            _animator.SetBool(IsGroundedHash, isGrounded);

        private void OnReload() => 
            _animator.SetTrigger(Reload);

        // animation event at start attack animations
        private void OnAttackAnimationStarted(string attackType) => 
            SyncAttackDelay(attackType);
        
        // Empty animation event needed to calculate the impact timing relative to the delay in the configuration.
        private void OnAttackAnimationPerformed() {}

        private void SyncAttackDelay(string attackType)
        {
            float attackDelay = 0;
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
            var clip = _animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            
            if (attackType == "Primary")
                attackDelay = _weapon.PrimaryAttack.AttackDelay;
            else if (attackType == "Secondary")
                attackDelay = _weapon.SecondaryAttack.AttackDelay;
            else
            {
                Debug.LogError($"Clip {clip.name} has wrong attackType parameter.");
                return;
            }
            
            if (attackDelay == 0)
                return;
            
            
            float attackTime = GetAnimationAttackTime(clip);
            
            if (attackTime > 0)
            {
                float speed = attackTime / attackDelay;
                _animator.speed = speed;
            }
        }

        private float GetAnimationAttackTime(AnimationClip clip)
        {
            foreach (var e in clip.events)
            {
                if (e.functionName == "OnAttackAnimationPerformed")
                    return e.time;
            }

            Debug.LogError($"Clip {clip.name} has delay but has not event OnAttackAnimationPerformed.");
            
            return -1f;
        }
    }
}