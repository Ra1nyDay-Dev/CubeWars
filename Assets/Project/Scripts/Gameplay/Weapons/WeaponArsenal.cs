using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Characters.Movement;
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
        [SerializeField] private Death _death;
        
        private IWeaponFactory _weaponFactory;
        private CharacterMovement _owner;

        [Inject]
        public void Construct(IWeaponFactory weaponFactory) => 
            _weaponFactory = weaponFactory;

        private void Awake()
        {
            _owner = GetComponent<CharacterMovement>();
        }

        private void Start() => 
            _death.Happened += OnDie;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O)) 
                UnequipWeapon();
        }

        private void OnDestroy() => 
            _death.Happened -= OnDie;

        public GameObject CurrentWeaponGameObject { get; private set; }

        public IWeapon CurrentWeapon { get; private set; }

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
                _owner);
            
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