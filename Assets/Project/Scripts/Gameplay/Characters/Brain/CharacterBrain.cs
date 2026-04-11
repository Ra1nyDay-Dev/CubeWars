using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Characters.Interactions;
using Project.Scripts.Gameplay.Characters.Movement;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Weapons;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Brain
{
    public abstract class CharacterBrain : IDisposable
    {
        protected readonly GameObject _character;
        private readonly CharacterMovement _characterMovement;
        private readonly WeaponArsenal _weaponArsenal;
        private readonly InteractorUnit _interactorUnit;
        private readonly Death _characterDeath;

        protected CharacterBrain(GameObject character)
        {
            _character = character;
            
            _characterMovement = _character.GetComponent<CharacterMovement>();
            _weaponArsenal = _characterMovement.GetComponent<WeaponArsenal>();
            _interactorUnit = _characterMovement.GetComponent<InteractorUnit>();
            _characterDeath = _characterMovement.GetComponent<Death>();
            
            _characterDeath.Happened += OnCharacterDeath;
        }

        public bool IsEnabled { get; protected set; } = false;
        
        public virtual void Enable() => 
            IsEnabled = true;
        
        public virtual void Disable() => 
            IsEnabled = false;

        public void Update(float deltaTime)
        {
            if (!IsEnabled)
                return;
            
            UpdateLogic(deltaTime);
        }

        public virtual void Dispose()
        {
            Disable();
            _characterDeath.Happened -= OnCharacterDeath;
        }

        protected abstract void UpdateLogic(float deltaTime);

        protected void Move(Vector2 direction) =>
            _characterMovement.SetMoveDirection(direction);

        protected void Rotate(Vector3 direction) => 
            _characterMovement.SetRotationDirection(direction);

        protected void Jump() => 
            _characterMovement.Jump();

        protected void TryInteract() => 
            _interactorUnit.CurrentInteractable?.Interact(_interactorUnit);

        protected void StartPrimaryAttack() => 
            _weaponArsenal.CurrentWeapon?.StartPrimaryAttack();

        protected void StartSecondaryAttack() => 
            _weaponArsenal.CurrentWeapon?.StartSecondaryAttack();

        protected void StopPrimaryAttack() => 
            _weaponArsenal.CurrentWeapon?.StopPrimaryAttack();

        protected void StopSecondaryAttack() => 
            _weaponArsenal.CurrentWeapon?.StopSecondaryAttack();

        private bool IsReloadableWeapon(out RangeWeapon reloadable)
        {
            reloadable = _weaponArsenal.CurrentWeapon as RangeWeapon;
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