using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.WeaponSystems.AttackSystems;
using UnityEngine;

namespace Project.Scripts.Gameplay.WeaponSystems
{
    public class RangeWeapon : Weapon
    {
        [SerializeField] protected ParticleSystem[] _muzzleEffects; // toDo: move to WeaponEffects?
        
        public bool IsReloadable { get; protected set; }
        public int CurrentAmmo { get; private set; }
        public int CurrentAmmoInMagazine { get; private set; }
        public bool InfiniteAmmo { get; private set; }
        
        public event Action ReloadStarted;
        public event Action<int, int, bool> AmmoChanged;
        
        private int _maxAmmoInMagazine;
        private float _reloadTime;
        private int _maxAmmo;
        private int _ammoPerPrimaryShot;
        private int _ammoPerSecondaryShot;

        private bool _isReloading;

        public override void Construct(
            WeaponConfig config,
            AttackBehaviour primaryAttack,
            AttackBehaviour secondaryAttack,
            Character owner
        )
        {
            base.Construct(config,primaryAttack,secondaryAttack,owner);
            
            RangeWeaponConfig rangeConfig = config as RangeWeaponConfig;
            if (rangeConfig == null)
                throw new ArgumentException($"{config.WeaponType}: Wrong config type for RangeWeapon: {config.GetType()}");
            
            IsReloadable = rangeConfig.IsReloadable;
            _maxAmmoInMagazine = rangeConfig.MaxAmmoInMagazine;
            _reloadTime =  rangeConfig.ReloadTime;
            InfiniteAmmo = rangeConfig.InfiniteAmmo;
            _maxAmmo = rangeConfig.MaxAmmo;
            _ammoPerPrimaryShot = rangeConfig.AmmoPerPrimaryShot;
            _ammoPerSecondaryShot = rangeConfig.AmmoPerSecondaryShot;

            CurrentAmmoInMagazine = _maxAmmoInMagazine;
            CurrentAmmo = _maxAmmo;
            
            AmmoChanged?.Invoke(CurrentAmmoInMagazine, CurrentAmmo - CurrentAmmoInMagazine, InfiniteAmmo);
        }

        public virtual async UniTask Reload()
        {
            if (!IsReloadable 
                || _isReloading 
                || _maxAmmoInMagazine == CurrentAmmoInMagazine
                ||(!InfiniteAmmo && CurrentAmmo == CurrentAmmoInMagazine))
                return;

            _isReloading = true;
            
            ReloadStarted?.Invoke();
            
            await UniTask.Delay(
                TimeSpan.FromSeconds(_reloadTime),
                cancellationToken: this.GetCancellationTokenOnDestroy()
            );

            CurrentAmmoInMagazine = InfiniteAmmo ? 
                _maxAmmoInMagazine 
                : Mathf.Min(CurrentAmmo, _maxAmmoInMagazine);
            
            _isReloading = false;
            
            AmmoChanged?.Invoke(CurrentAmmoInMagazine, CurrentAmmo - CurrentAmmoInMagazine, InfiniteAmmo);
        }

        protected override bool CanAttack(AttackBehaviour attack)
        {
            if (!base.CanAttack(attack))
                return false;
            
            int ammoRequired = attack == PrimaryAttack ? _ammoPerPrimaryShot : _ammoPerSecondaryShot;
            return (CurrentAmmoInMagazine >= ammoRequired && !_isReloading) || !IsReloadable;
        }

        protected override void OnAttackPerformed(AttackBehaviour attack)
        {
            if (attack == PrimaryAttack)
                ConsumeAmmo(_ammoPerPrimaryShot);
            else
                ConsumeAmmo(_ammoPerSecondaryShot);

            PerformEffects();
            
            AmmoChanged?.Invoke(CurrentAmmoInMagazine, CurrentAmmo - CurrentAmmoInMagazine, InfiniteAmmo);
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
                CurrentAmmoInMagazine -= amount;
            
            if (!InfiniteAmmo)
                CurrentAmmo -= amount;
        }
        
        protected override void OnAttackEnded(AttackBehaviour attack)
        {
            base.OnAttackEnded(attack);
            
            if (IsReloadable && CurrentAmmoInMagazine == 0)
                Reload().Forget();
        }
    }
}