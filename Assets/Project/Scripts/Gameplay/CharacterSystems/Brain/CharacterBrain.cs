using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.CharacterSystems.Interactions;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain
{
    public abstract class CharacterBrain : ITickable, IDisposable
    {
        protected readonly Character _character;

        protected CharacterBrain(Character character)
        {
            _character = character;
            // _characterDeath.Happened += OnCharacterDeath;
        }

        public bool IsEnabled { get; protected set; } = false;
        
        public virtual void Enable() => 
            IsEnabled = true;
        
        public virtual void Disable() => 
            IsEnabled = false;

        public void Tick()
        {
            if (!IsEnabled)
                return;
            
            UpdateLogic(Time.deltaTime);
        }

        public virtual void Dispose()
        {
            Disable();
            // _characterDeath.Happened -= OnCharacterDeath;
        }

        protected abstract void UpdateLogic(float deltaTime);

        protected void Move(Vector2 direction) =>
            _character.Movement.SetMoveDirection(direction);

        protected void Rotate(Vector3 direction) => 
            _character.Movement.SetRotationDirection(direction);

        protected void Jump() => 
            _character.Movement.Jump();

        protected void TryInteract()
        {
            if (_character.Interactor.TryGetNearInteractable(out IInteractable interactable))
                interactable.Interact(_character.Interactor);
        }

        protected void StartPrimaryAttack() => 
            _character.WeaponArsenal.CurrentWeapon?.StartPrimaryAttack();

        protected void StartSecondaryAttack() => 
            _character.WeaponArsenal.CurrentWeapon?.StartSecondaryAttack();

        protected void StopPrimaryAttack() => 
            _character.WeaponArsenal.CurrentWeapon?.StopPrimaryAttack();

        protected void StopSecondaryAttack() => 
            _character.WeaponArsenal.CurrentWeapon?.StopSecondaryAttack();

        private bool IsReloadableWeapon(out RangeWeapon reloadable)
        {
            reloadable = _character.WeaponArsenal.CurrentWeapon as RangeWeapon;
            return reloadable != null && reloadable.IsReloadable;
        }

        protected void Reload()
        {
            if (IsReloadableWeapon(out RangeWeapon reloadable))
                reloadable.Reload().Forget();
        }

        private void OnCharacterDeath(DamageData damageData) => 
            Dispose();
    }
}