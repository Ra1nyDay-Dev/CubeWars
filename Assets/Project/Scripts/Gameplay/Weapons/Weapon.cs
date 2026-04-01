using System;
using System.Collections;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon
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

        public void Construct(AttackBehaviour primaryAttack,
            AttackBehaviour secondaryAttack,
            Character owner,
            WeaponType weaponType,
            Material handsSkinMaterial
            )
        {
            PrimaryAttack = primaryAttack;
            SecondaryAttack = secondaryAttack;
            Owner = owner;
            WeaponType = weaponType;
            ApplyHandsSkinMaterial(handsSkinMaterial);
        }

        public async Awaitable PerformPrimaryAttack()
        {
            if (PrimaryAttack != null && !_isAttacking)
            {
                _isAttacking = true;
                PrimaryAttackStarted?.Invoke();
                await PerformAttack(PrimaryAttack, PrimaryAttack.AttackDelay, PrimaryAttack.AttackCooldown);
                PrimaryAttackEnded?.Invoke();
            }
        }

        public async Awaitable PerformSecondaryAttack()
        {
            if (SecondaryAttack != null && !_isAttacking)
            {
                _isAttacking = true;
                SecondaryAttackStarted?.Invoke();
                await PerformAttack(SecondaryAttack, SecondaryAttack.AttackDelay, SecondaryAttack.AttackCooldown);
                SecondaryAttackEnded?.Invoke();
            }
        }

        private async Awaitable PerformAttack(AttackBehaviour attack, float delay, float cooldown)
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