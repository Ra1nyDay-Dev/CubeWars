using System;
using Project.Scripts.Gameplay.Characters;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimation : MonoBehaviour
    {
        private static readonly int SecondaryAttack = Animator.StringToHash("SecondaryAttack");
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
            _weapon.SecondaryAttackStarted += OnSecondaryAttackStarted;
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
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            // if (_weapon == null)
            //     _weapon = GetComponent<IWeapon>();
            //
            // if (_owner == null)
            //     _owner = GetComponentInParent<Character>();
        }

        private void OnDestroy()
        {
            _weapon.PrimaryAttackStarted -= OnPrimaryAttackStarted;
            _weapon.SecondaryAttackStarted -= OnSecondaryAttackStarted;
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

        private void OnSecondaryAttackStarted() =>
            _animator.SetTrigger(SecondaryAttack);

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
    }
}