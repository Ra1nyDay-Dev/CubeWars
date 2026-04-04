using System;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private GameObject[] _hands;

        public Character Owner { get; private set; }
        public int PrimaryAttackAnimationsCount { get; private set; }
        public WeaponType WeaponType { get; private set; }

        public event Action PrimaryAttackStarted;
        public event Action PrimaryAttackEnded;
        public event Action SecondaryAttackStarted;
        public event Action SecondaryAttackEnded;

        public AttackBehaviour PrimaryAttack { get; protected set; }
        public AttackBehaviour SecondaryAttack { get; protected set; }

        protected bool _isPrimaryAttackButtonHeldDown;
        protected bool _isSecondaryAttackButtonHeldDown;
        protected bool _isAttacking = false;

        public virtual void Construct(
            WeaponConfig config,
            AttackBehaviour primaryAttack,
            AttackBehaviour secondaryAttack,
            Character owner,
            Material handsSkinMaterial
        )
        {
            PrimaryAttack = primaryAttack;
            SecondaryAttack = secondaryAttack;
            Owner = owner;
            WeaponType = config.WeaponType;
            ApplyHandsSkinMaterial(handsSkinMaterial);
        }

        public virtual async Awaitable StartPrimaryAttack()
        {
            if (_isPrimaryAttackButtonHeldDown || PrimaryAttack == null || _isAttacking)
                return;
            
            _isPrimaryAttackButtonHeldDown = true;
            await AttackLoop(PrimaryAttack, () => _isPrimaryAttackButtonHeldDown);
        }

        public virtual async Awaitable StartSecondaryAttack()
        {
            if (_isSecondaryAttackButtonHeldDown || SecondaryAttack == null || _isAttacking)
                return;
            
            _isSecondaryAttackButtonHeldDown = true;
            await AttackLoop(SecondaryAttack, () => _isSecondaryAttackButtonHeldDown);
        }

        public virtual void StopPrimaryAttack() => _isPrimaryAttackButtonHeldDown = false;
        public virtual void StopSecondaryAttack() => _isSecondaryAttackButtonHeldDown = false;

        protected async Awaitable AttackLoop(
            AttackBehaviour attack,
            Func<bool> isHeld)
        {
            while (isHeld())
            {
                await PerformAttack(attack);

                if (!attack.HoldingButtonContinuesAttack)
                    break;
            }
        }

        protected virtual async Awaitable PerformAttack(AttackBehaviour attack)
        {
            if (!CanAttack(attack))
                return;
            
            OnAttackStarted(attack);
            
            await ApplyAttackDelay(attack);
            
            attack.PerformAttack();
            OnAttackPerformed(attack);
            
            await ApplyAttackCooldown(attack);
            
            OnAttackEnded(attack);
        }
        
        protected virtual bool CanAttack(AttackBehaviour attack) => 
            !_isAttacking;

        protected virtual void OnAttackStarted(AttackBehaviour attack)
        {
            _isAttacking = true;
            
            if (attack == PrimaryAttack)
                PrimaryAttackStarted?.Invoke();
            else
                SecondaryAttackStarted?.Invoke();
        }

        protected virtual Awaitable ApplyAttackDelay(AttackBehaviour attack) => 
            Awaitable.WaitForSecondsAsync(attack.AttackDelay);
        
        protected virtual Awaitable ApplyAttackCooldown(AttackBehaviour attack) => 
            Awaitable.WaitForSecondsAsync(attack.AttackInterval);

        protected virtual void OnAttackPerformed(AttackBehaviour attack)
        {
            Debug.Log($"{Time.time} Attacked");
        }

        protected virtual void OnAttackEnded(AttackBehaviour attack)
        {
            Debug.Log($"{Time.time} Attack ready");
            _isAttacking = false;
            
            if (attack == PrimaryAttack)
                PrimaryAttackEnded?.Invoke();
            else
                SecondaryAttackEnded?.Invoke();
        }

        private void ApplyHandsSkinMaterial(Material material)
        {
            foreach (GameObject hand in _hands)
                hand.GetComponent<Renderer>().material = material;
        }
    }
}