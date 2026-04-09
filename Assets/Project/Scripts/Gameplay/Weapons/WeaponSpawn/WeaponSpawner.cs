using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.Services.Fabrics.Weapon;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Weapons.WeaponSpawn
{
    public class WeaponSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponSlot;
        
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private float _spawnTime;
        [SerializeField] private bool _spawnOnStart;
        
        private IWeaponFabric _weaponFabric;
        private WeaponSpawnerAnimation _spawnerAnimation;
        
        private bool _isActive;
        private bool _isWeaponAvailable;
        private readonly List<WeaponArsenal> _charactersInZone = new();
        
        private bool CanTakeWeapon => 
            _charactersInZone.Count > 0 && _isWeaponAvailable;

        [Inject]
        public void Construct(IWeaponFabric weaponFabric) => 
            _weaponFabric = weaponFabric;

        private void Awake()
        {
            _spawnerAnimation = GetComponent<WeaponSpawnerAnimation>();
            
            _weaponFabric.CreateWeaponAtSpawn(_weaponType, _weaponSlot.transform);
            _weaponSlot.SetActive(false);
            ActivateSpawn();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E) && CanTakeWeapon)
                TakeWeapon(_charactersInZone.First());
        }

        private void OnTriggerEnter(Collider character)
        {
            Debug.Log("Entered in zone");
            if (character.TryGetComponent(out WeaponArsenal arsenal))
            {
                _charactersInZone.Add(arsenal);
                Debug.Log("Arsenal found");
            }
        }

        private void OnTriggerExit(Collider character)
        {
            if (character.TryGetComponent(out WeaponArsenal arsenal))
            {
                if (_charactersInZone.Contains(arsenal))
                    _charactersInZone.Remove(arsenal);
            }
        }

        private void ActivateSpawn()
        {
            if (_isActive) 
                return;
            
            _isActive = true;
            
            if (_spawnOnStart)
                RespawnWeapon();
            else
                WaitAndRespawnWeapon().Forget();
        }

        private void DeactivateSpawn()
        {
            HideWeapon();
            _isActive = false;
        }

        private void RespawnWeapon()
        {
            _isWeaponAvailable = true;
            _weaponSlot.SetActive(true);
            _spawnerAnimation?.StartAnimation();
        }

        private void HideWeapon()
        {
            _isWeaponAvailable = false;
            _weaponSlot.SetActive(false);
            _spawnerAnimation?.StopAnimation();
        }

        private void TakeWeapon(WeaponArsenal characterArsenal)
        {
            if (characterArsenal.CurrentWeapon?.WeaponType == _weaponType)
                return;
                    
            HideWeapon();
            characterArsenal.ChangeWeapon(_weaponType);
            WaitAndRespawnWeapon().Forget();
        }

        private async UniTask WaitAndRespawnWeapon()
        {
            await SpawnDelay();
            RespawnWeapon();
        }

        private UniTask SpawnDelay() => 
            UniTask.Delay(TimeSpan.FromSeconds(_spawnTime));
    }
}
