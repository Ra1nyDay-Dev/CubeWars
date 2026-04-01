using System;
using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.Services.Fabrics.Weapon;
using Project.Scripts.Infrastructure.Services.ServiceLocator;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class WeaponArsenal : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponSlot;
        [SerializeField] private GameObject _overlapAttackStartPoint;
        [SerializeField] Material _handsSkinMaterial;
        [SerializeField] private GameObject _selfHitbox;
        [SerializeField] private Death _death;
        
        private IWeaponFabric _weaponFabric;
        private Character _owner;

        private void Awake()
        {
            _owner = GetComponent<Character>();
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
            //toDo: rewrite to inject in Characters Fabric
            _weaponFabric = SceneServices.Container.Get<IWeaponFabric>();
            
            CurrentWeaponGameObject = _weaponFabric.CreateWeaponInHands(
                weaponType,
                _weaponSlot.transform,
                _overlapAttackStartPoint.transform,
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