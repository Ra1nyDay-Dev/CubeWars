using System;
using System.Collections;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Characters;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private GameObject[] _hands;
        
        public Character Owner {get; private set;}
        
        public event Action PrimaryAttackStarted;
        public event Action PrimaryAttackEnded;
        public event Action SecondaryAttackStarted;
        public event Action SecondaryAttackEnded;
        
        private AttackBehaviour _primaryAttack;
        private AttackBehaviour _secondaryAttack;

        private bool _isAttacking = false;

        public void Construct(
            AttackBehaviour primaryAttack, 
            AttackBehaviour secondaryAttack, 
            Character owner,
            Material handsSkinMaterial)
        {
            _primaryAttack = primaryAttack;
            _secondaryAttack = secondaryAttack;
            Owner = owner;
            ApplyHandsSkinMaterial(handsSkinMaterial);
        }

        public async Awaitable PerformPrimaryAttack()
        {
            if (_primaryAttack != null && !_isAttacking)
            {
                _isAttacking = true;
                PrimaryAttackStarted?.Invoke();
                await PerformAttack(_primaryAttack, _primaryAttack.AttackDelay, _primaryAttack.AttackCooldown);
                PrimaryAttackEnded?.Invoke();
            }
        }

        public async Awaitable PerformSecondaryAttack()
        {
            if (_secondaryAttack != null && !_isAttacking)
            {
                _isAttacking = true;
                SecondaryAttackStarted?.Invoke();
                await PerformAttack(_secondaryAttack, _secondaryAttack.AttackDelay, _secondaryAttack.AttackCooldown);
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