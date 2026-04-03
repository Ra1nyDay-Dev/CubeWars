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

        public virtual async Awaitable PerformPrimaryAttack() => 
            await PerformAttack(PrimaryAttack);

        public virtual async Awaitable PerformSecondaryAttack() => 
            await PerformAttack(SecondaryAttack);

        protected virtual async Awaitable PerformAttack(AttackBehaviour attack)
        {
            if (!CheckAttackPossibility(attack)) 
                return;
            
            OnAttackStarted(attack);
            await ApplyAttackDelay(attack);
            attack.PerformAttack();
            OnAttackPerformed(attack);
            await ApplyAttackCooldown(attack);
            OnAttackEnded(attack);
        }
        
        protected virtual bool CheckAttackPossibility(AttackBehaviour attack) => 
            attack != null && !_isAttacking;

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

        protected virtual void OnAttackPerformed(AttackBehaviour attack) {}

        private static Awaitable ApplyAttackCooldown(AttackBehaviour attack) => 
            Awaitable.WaitForSecondsAsync(attack.AttackCooldown);

        protected virtual void OnAttackEnded(AttackBehaviour attack)
        {
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