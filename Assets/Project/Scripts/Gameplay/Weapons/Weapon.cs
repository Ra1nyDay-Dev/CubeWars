using System;
using System.Collections;
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
        
        public Character Owner {get; private set;}
        public int PrimaryAttackAnimationsCount { get; private set; }
        public WeaponType WeaponType {get; private set;}

        public event Action PrimaryAttackStarted;
        public event Action PrimaryAttackEnded;
        public event Action SecondaryAttackStarted;
        public event Action SecondaryAttackEnded;

        public AttackBehaviour PrimaryAttack { get; protected set; }
        public AttackBehaviour SecondaryAttack { get; protected set; }

        private bool _isAttacking = false;

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

        public virtual async Awaitable PerformPrimaryAttack()
        {
            if (PrimaryAttack != null && !_isAttacking)
            {
                _isAttacking = true;
                PrimaryAttackStarted?.Invoke();
                await PerformAttack(PrimaryAttack, PrimaryAttack.AttackDelay, PrimaryAttack.AttackCooldown);
                PrimaryAttackEnded?.Invoke();
            }
        }

        public virtual async Awaitable PerformSecondaryAttack()
        {
            if (SecondaryAttack != null && !_isAttacking)
            {
                _isAttacking = true;
                SecondaryAttackStarted?.Invoke();
                await PerformAttack(SecondaryAttack, SecondaryAttack.AttackDelay, SecondaryAttack.AttackCooldown);
                SecondaryAttackEnded?.Invoke();
            }
        }

        protected virtual async Awaitable PerformAttack(AttackBehaviour attack, float delay, float cooldown)
        {
            await Awaitable.WaitForSecondsAsync(delay);
            attack.PerformAttack();
            await Awaitable.WaitForSecondsAsync(cooldown);

            _isAttacking = false;
        }

        private void ApplyHandsSkinMaterial(Material material)
        {
            foreach (GameObject hand in _hands) 
                hand.GetComponent<Renderer>().material = material;
        }
    }
}