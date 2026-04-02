using System;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.AttackSystems.Raycast;
using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class RangeWeapon : Weapon, IReloadable
    {
        private bool _isReloadable;
        private int _maxAmmoInMagazine;
        private float _reloadTime;
        private bool _infiniteAmmo;
        private int _maxAmmo;
        private int _ammoPerPrimaryShot;
        private int _ammoPerSecondaryShot;
        
        private int _currentAmmoInMagazine;
        private int _currentAmmo;
        private bool _isReloading;

        public override void Construct(
            WeaponConfig config,
            AttackBehaviour primaryAttack,
            AttackBehaviour secondaryAttack,
            Character owner,
            Material handsSkinMaterial
        )
        {
            base.Construct(config,primaryAttack,secondaryAttack,owner,handsSkinMaterial);
            
            RangeWeaponConfig rangeConfig = config as RangeWeaponConfig;
            if (rangeConfig == null)
                throw new ArgumentException($"{config.WeaponType}: Wrong config type for RangeWeapon: {config.GetType()}");
            
            _isReloadable = rangeConfig.IsReloadable;
            _maxAmmoInMagazine = rangeConfig.MaxAmmoInMagazine;
            _reloadTime =  rangeConfig.ReloadTime;
            _infiniteAmmo = rangeConfig.InfiniteAmmo;
            _maxAmmo = rangeConfig.MaxAmmo;
            _ammoPerPrimaryShot = rangeConfig.AmmoPerPrimaryShot;
            _ammoPerSecondaryShot = rangeConfig.AmmoPerSecondaryShot;

            _currentAmmoInMagazine = _maxAmmoInMagazine;
            _currentAmmo = _maxAmmo;

            Debug.Log($"Weapon {WeaponType} constructed. " +
                      $"Ammo {_currentAmmoInMagazine}/" +
                      $"{(_infiniteAmmo ? "infinity" : _currentAmmo - _currentAmmoInMagazine)}");
        }

        public virtual async Awaitable Reload()
        {
            if (!_isReloadable)
                return;
            if(_isReloading)
                return;
            if (_maxAmmoInMagazine == _currentAmmoInMagazine)
                return;
            if (!_infiniteAmmo && _currentAmmo == _currentAmmoInMagazine)
                return;

            _isReloading = true;
            await Awaitable.WaitForSecondsAsync(_reloadTime);

            _currentAmmoInMagazine = _infiniteAmmo ? 
                _maxAmmoInMagazine 
                : Mathf.Min(_currentAmmo, _maxAmmoInMagazine);
            
            _isReloading = false;
            
            Debug.Log($"Weapon {WeaponType} reloaded. " +
                      $"Ammo {_currentAmmoInMagazine}/" +
                      $"{(_infiniteAmmo ? "infinity" : _currentAmmo - _currentAmmoInMagazine)}");
        }

        protected override bool CheckAttackPossibility(AttackBehaviour attack) => 
            base.CheckAttackPossibility(attack) && CanShoot(_ammoPerPrimaryShot);

        protected override void OnAttackPerformed(AttackBehaviour attack)
        {
            if (attack == PrimaryAttack)
                ConsumeAmmo(_ammoPerPrimaryShot);
            else
                ConsumeAmmo(_ammoPerSecondaryShot);
            
            Debug.Log($"Weapon {WeaponType} shoot. " +
                      $"Ammo {_currentAmmoInMagazine}/" +
                      $"{(_infiniteAmmo ? "infinity" : _currentAmmo - _currentAmmoInMagazine)}");
        }

        protected virtual bool CanShoot(int ammoRequired)
        {
            return _currentAmmoInMagazine >= ammoRequired || !_isReloadable;
        }
        
        protected virtual void ConsumeAmmo(int amount)
        {
            if (_isReloadable)
                _currentAmmoInMagazine -= amount;
            
            if (!_infiniteAmmo)
                _currentAmmo -= amount;

            if (_isReloadable && _currentAmmoInMagazine == 0)
                Reload();
        }
        
        protected override void OnAttackEnded(AttackBehaviour attack)
        {
            base.OnAttackEnded(attack);
            
            if (_isReloadable && _currentAmmoInMagazine == 0)
                Reload();
        }


    }
}