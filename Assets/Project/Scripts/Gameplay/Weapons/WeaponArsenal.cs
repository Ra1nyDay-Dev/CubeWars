using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.CharacterSystems.Movement;
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
        [SerializeField] Material _handsSkinMaterial;
        [SerializeField] private GameObject _selfHitbox;
        
        public GameObject CurrentWeaponGameObject { get; private set; }

        public IWeapon CurrentWeapon { get; private set; }

        private IWeaponFactory _weaponFactory;
        private CharacterMovement _ownerMovement;
        private Death _death;
        
        [Inject]
        public void Construct(IWeaponFactory weaponFactory) => 
            _weaponFactory = weaponFactory;

        private void Awake()
        {
            _ownerMovement = GetComponent<CharacterMovement>();
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
            CurrentWeaponGameObject = _weaponFactory.CreateWeaponInHands(
                weaponType,
                _weaponSlot.transform,
                _attackStartPoint.transform,
                _handsSkinMaterial,
                _selfHitbox,
                _ownerMovement);
            
            CurrentWeapon = CurrentWeaponGameObject.GetComponent<IWeapon>();
        }

        private void UnequipWeapon()
        {
            if (CurrentWeaponGameObject != null)
            {
                Destroy(CurrentWeaponGameObject);
                CurrentWeaponGameObject = null;
                CurrentWeapon = null;
            }
        }

        private void OnDie(DamageData damageData) => 
            UnequipWeapon();
    }
}