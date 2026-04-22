using System;
using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.Services.Factories.WeaponFactory;
using Project.Scripts.Gameplay.WeaponSystems;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.CharacterSystems.Inventory
{
    public class WeaponArsenal : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponSlot;
        [SerializeField] private GameObject _attackStartPoint;
        [SerializeField] private GameObject _selfHitbox;

        public IWeapon CurrentWeapon { get; private set; }

        public event Action<IWeapon> WeaponChanged;
        public event Action<int, int, bool> CurrentWeaponAmmoChanged;

        private GameObject _currentWeaponGameObject;
        private IWeaponFactory _weaponFactory;
        private Character _owner;
        private RespawnBehaviour _respawnBehaviour;
        
        [Inject]
        public void Construct(IWeaponFactory weaponFactory) => 
            _weaponFactory = weaponFactory;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _respawnBehaviour = GetComponent<RespawnBehaviour>();
        }

        private void OnEnable() => 
            _respawnBehaviour.Dead += OnDie;

        private void OnDisable() => 
            _respawnBehaviour.Dead -= OnDie;

        public void ChangeWeapon(WeaponType weaponType)
        {
            UnequipWeapon();
            EquipWeapon(weaponType);
        }
        
        private void EquipWeapon(WeaponType weaponType)
        {
            _currentWeaponGameObject = _weaponFactory.CreateWeaponInHands(
                weaponType,
                _weaponSlot.transform,
                _attackStartPoint.transform,
                _selfHitbox,
                _owner);
            
            CurrentWeapon = _currentWeaponGameObject.GetComponent<IWeapon>();
            WeaponChanged?.Invoke(CurrentWeapon);
            
            if (CurrentWeapon is RangeWeapon rangeWeapon)
            {
                rangeWeapon.AmmoChanged += OnCurrentWeaponAmmoChanged;
                OnCurrentWeaponAmmoChanged(
                    rangeWeapon.CurrentAmmoInMagazine,
                    rangeWeapon.CurrentAmmo - rangeWeapon.CurrentAmmoInMagazine,
                    rangeWeapon.InfiniteAmmo );
            } 
        }

        private void UnequipWeapon()
        {
            if (_currentWeaponGameObject != null)
            {
                if (CurrentWeapon is RangeWeapon rangeWeapon) 
                    rangeWeapon.AmmoChanged -= OnCurrentWeaponAmmoChanged;
                
                Destroy(_currentWeaponGameObject);
                _currentWeaponGameObject = null;
                CurrentWeapon = null;
                WeaponChanged?.Invoke(CurrentWeapon);
            }
        }

        private void OnCurrentWeaponAmmoChanged(int currentAmmo, int reservedAmmo, bool infiniteAmmo) => 
            CurrentWeaponAmmoChanged?.Invoke(currentAmmo, reservedAmmo, infiniteAmmo);

        private void OnDie(DamageData damageData) => 
            UnequipWeapon();
    }
}