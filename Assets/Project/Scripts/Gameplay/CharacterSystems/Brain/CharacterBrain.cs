using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.CharacterSystems.Interactions;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.WeaponSystems;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain
{
    public abstract class CharacterBrain : ITickable, IDisposable
    {
        public Character Character { get; protected set; }

        public CharacterBrain(Character character)
        {
            Character = character;
            Character.RespawnBehaviour.Dead += OnCharacterDead;
            Character.RespawnBehaviour.Respawned += OnCharacterRespawned;
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
            Character.RespawnBehaviour.Dead -= OnCharacterDead;
            Character.RespawnBehaviour.Respawned -= OnCharacterRespawned;
        }

        protected abstract void UpdateLogic(float deltaTime);

        protected void Move(Vector2 direction) =>
            Character.Movement.SetMoveDirection(direction);

        protected void Rotate(Vector3 direction) => 
            Character.Movement.SetRotationDirection(direction);

        protected void Jump() => 
            Character.Movement.Jump();

        protected void TryInteract()
        {
            if (Character.Interactor.TryGetNearInteractable(out IInteractable interactable))
                interactable.Interact(Character.Interactor);
        }

        protected void StartPrimaryAttack() => 
            Character.WeaponArsenal.CurrentWeapon?.StartPrimaryAttack();

        protected void StartSecondaryAttack() => 
            Character.WeaponArsenal.CurrentWeapon?.StartSecondaryAttack();

        protected void StopPrimaryAttack() => 
            Character.WeaponArsenal.CurrentWeapon?.StopPrimaryAttack();

        protected void StopSecondaryAttack() => 
            Character.WeaponArsenal.CurrentWeapon?.StopSecondaryAttack();

        private bool IsReloadableWeapon(out RangeWeapon reloadable)
        {
            reloadable = Character.WeaponArsenal.CurrentWeapon as RangeWeapon;
            return reloadable != null && reloadable.IsReloadable;
        }

        protected void Reload()
        {
            if (IsReloadableWeapon(out RangeWeapon reloadable))
                reloadable.Reload().Forget();
        }

        private void OnCharacterDead(DamageData damageData)
        {
            Disable();
            Move(Vector3.zero);
        }

        private void OnCharacterRespawned() => 
            Enable();
    }
}