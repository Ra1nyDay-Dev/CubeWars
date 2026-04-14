using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.Services.Factories.WeaponFactory;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Weapons
{
    public class WeaponArsenal : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponSlot;
        [SerializeField] private GameObject _attackStartPoint;
        [SerializeField] private GameObject _selfHitbox;

        public IWeapon CurrentWeapon { get; private set; }

        private GameObject _currentWeaponGameObject;
        private IWeaponFactory _weaponFactory;
        private Character _owner;
        private Death _death;
        
        [Inject]
        public void Construct(IWeaponFactory weaponFactory) => 
            _weaponFactory = weaponFactory;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _death = GetComponent<Death>();
        }

        private void OnEnable() => 
            _death.Happened += OnDie;

        private void OnDisable() => 
            _death.Happened -= OnDie;

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
        }

        private void UnequipWeapon()
        {
            if (_currentWeaponGameObject != null)
            {
                Destroy(_currentWeaponGameObject);
                _currentWeaponGameObject = null;
                CurrentWeapon = null;
            }
        }

        private void OnDie(DamageData damageData) => 
            UnequipWeapon();
    }
}