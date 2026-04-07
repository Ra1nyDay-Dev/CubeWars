using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class RangeWeapon : Weapon
    {
        public bool IsReloadable { get; protected set; }

        [SerializeField] protected ParticleSystem[] _muzzleEffects; // toDo: move to WeaponEffects?
        
        public event Action ReloadStarted;
        
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
            
            IsReloadable = rangeConfig.IsReloadable;
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

        public virtual async UniTask Reload()
        {
            if (!IsReloadable 
                || _isReloading 
                || _maxAmmoInMagazine == _currentAmmoInMagazine
                ||(!_infiniteAmmo && _currentAmmo == _currentAmmoInMagazine))
                return;

            _isReloading = true;
            
            ReloadStarted?.Invoke();
            
            await UniTask.Delay(
                TimeSpan.FromSeconds(_reloadTime),
                cancellationToken: this.GetCancellationTokenOnDestroy()
            );

            _currentAmmoInMagazine = _infiniteAmmo ? 
                _maxAmmoInMagazine 
                : Mathf.Min(_currentAmmo, _maxAmmoInMagazine);
            
            _isReloading = false;
            
            Debug.Log($"Weapon {WeaponType} reloaded. " +
                      $"Ammo {_currentAmmoInMagazine}/" +
                      $"{(_infiniteAmmo ? "infinity" : _currentAmmo - _currentAmmoInMagazine)}");
        }

        protected override bool CanAttack(AttackBehaviour attack)
        {
            if (!base.CanAttack(attack))
                return false;
            
            int ammoRequired = attack == PrimaryAttack ? _ammoPerPrimaryShot : _ammoPerSecondaryShot;
            return (_currentAmmoInMagazine >= ammoRequired && !_isReloading) || !IsReloadable;
        }

        protected override void OnAttackPerformed(AttackBehaviour attack)
        {
            if (attack == PrimaryAttack)
                ConsumeAmmo(_ammoPerPrimaryShot);
            else
                ConsumeAmmo(_ammoPerSecondaryShot);

            PerformEffects();
            
            Debug.Log($"Weapon {WeaponType} shoot. " +
                      $"Ammo {_currentAmmoInMagazine}/" +
                      $"{(_infiniteAmmo ? "infinity" : _currentAmmo - _currentAmmoInMagazine)}");
        }

        private void PerformEffects()
        {
            if (_muzzleEffects.Length > 0)
            {
                foreach (var effect in _muzzleEffects) 
                    effect.Play();
            }
        }

        protected virtual void ConsumeAmmo(int amount)
        {
            if (IsReloadable)
                _currentAmmoInMagazine -= amount;
            
            if (!_infiniteAmmo)
                _currentAmmo -= amount;
        }
        
        protected override void OnAttackEnded(AttackBehaviour attack)
        {
            base.OnAttackEnded(attack);
            
            if (IsReloadable && _currentAmmoInMagazine == 0)
                Reload().Forget();
        }
    }
}