using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.CharacterSystems.Movement;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    //toDo: 
    // 1. Move AttackTypes to WeaponAttacks class with attack slots list
    // 2. Move Ammo to IAmmo Behaviour
    // 3. Move Reload to IReloadable Behaviour
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private GameObject[] _hands;

        public CharacterMovement Owner { get; private set; }
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
            CharacterMovement owner,
            Material handsSkinMaterial
        )
        {
            PrimaryAttack = primaryAttack;
            SecondaryAttack = secondaryAttack;
            Owner = owner;
            WeaponType = config.WeaponType;
            ApplyHandsSkinMaterial(handsSkinMaterial);
        }

        public virtual async UniTask StartPrimaryAttack()
        {
            if (_isPrimaryAttackButtonHeldDown || PrimaryAttack == null || _isAttacking)
                return;
            
            _isPrimaryAttackButtonHeldDown = true;

            try
            {
                await AttackLoop(PrimaryAttack, () => 
                    _isPrimaryAttackButtonHeldDown, this.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException) { }
        }

        public virtual async UniTask StartSecondaryAttack()
        {
            if (_isSecondaryAttackButtonHeldDown || SecondaryAttack == null || _isAttacking)
                return;
            
            _isSecondaryAttackButtonHeldDown = true;

            try
            {
                await AttackLoop(SecondaryAttack, () => 
                    _isSecondaryAttackButtonHeldDown, this.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException) { }
        }

        public virtual void StopPrimaryAttack() => 
            _isPrimaryAttackButtonHeldDown = false;

        public virtual void StopSecondaryAttack() => 
            _isSecondaryAttackButtonHeldDown = false;

        protected virtual async UniTask AttackLoop(
            AttackBehaviour attack,
            Func<bool> isHeld,
            CancellationToken token)
        {
            while (isHeld())
            {
                token.ThrowIfCancellationRequested();
                
                if (!CanAttack(attack))
                {
                    await UniTask.Yield(token);
                    continue;
                }
                
                await PerformAttack(attack, token);

                if (!attack.HoldingButtonContinuesAttack)
                    break;
            }
        }

        protected virtual async UniTask PerformAttack(
            AttackBehaviour attack,
            CancellationToken token)
        {
            OnAttackStarted(attack);
            
            await ApplyAttackDelay(attack, token);
            
            attack.PerformAttack();
            OnAttackPerformed(attack);
            
            await ApplyAttackCooldown(attack, token);
            
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

        protected virtual UniTask ApplyAttackDelay(
            AttackBehaviour attack, CancellationToken token) => 
            UniTask.Delay(TimeSpan.FromSeconds(attack.AttackDelay), cancellationToken: token);

        protected virtual UniTask ApplyAttackCooldown(
            AttackBehaviour attack, CancellationToken token) => 
            UniTask.Delay(TimeSpan.FromSeconds(attack.AttackInterval), cancellationToken: token);

        protected virtual void OnAttackPerformed(AttackBehaviour attack)
        {
            Debug.Log($"{Time.time} Attacked"); // toDo: DEBUG_DELETE 
        }

        protected virtual void OnAttackEnded(AttackBehaviour attack)
        {
            Debug.Log($"{Time.time} Attack ready"); // toDo: DEBUG_DELETE 
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