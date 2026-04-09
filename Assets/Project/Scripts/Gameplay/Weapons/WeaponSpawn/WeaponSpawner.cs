using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Characters.Interactions;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.Services.Fabrics.Weapon;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Weapons.WeaponSpawn
{
    public class WeaponSpawner : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject _weaponSlot;
        
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private float _spawnTime;
        [SerializeField] private bool _spawnOnStart;
        
        private IWeaponFabric _weaponFabric;
        private WeaponSpawnerAnimation _spawnerAnimation;
        private CancellationTokenSource _spawnCancellationTokenSource;
        
        private bool _isActive;
        private bool _isWeaponAvailable;

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

        private void OnDestroy() => 
            HideWeapon();

        public void Interact(InteractorUnit interactor)
        {
            if (!_isWeaponAvailable
                || !interactor.TryGetComponent(out WeaponArsenal arsenal)
                || arsenal.CurrentWeapon?.WeaponType == _weaponType)
                return;

            TakeWeapon(arsenal);
        }

        private void ActivateSpawn()
        {
            if (_isActive) 
                return;
            
            _isActive = true;
            
            if (_spawnOnStart)
                ShowWeapon();
            else
                WaitAndShowWeapon().Forget();
        }

        private void DeactivateSpawn()
        {
            HideWeapon();
            _isActive = false;
        }

        private void ShowWeapon()
        {
            _isWeaponAvailable = true;
            _weaponSlot.SetActive(true);
            _spawnerAnimation?.StartAnimation();
        }

        private void HideWeapon()
        {
            CancelSpawnTask();
            _isWeaponAvailable = false;
            _weaponSlot.SetActive(false);
            _spawnerAnimation?.StopAnimation();
        }

        private void TakeWeapon(WeaponArsenal characterArsenal)
        {
            HideWeapon();
            characterArsenal.ChangeWeapon(_weaponType);
            WaitAndShowWeapon().Forget();
        }

        private async UniTask WaitAndShowWeapon()
        {
            CancelSpawnTask();
            _spawnCancellationTokenSource = new CancellationTokenSource();
            await SpawnDelay(_spawnTime, _spawnCancellationTokenSource.Token);
            ShowWeapon();
        }

        private UniTask SpawnDelay(float spawnTime, CancellationToken cancellationToken) => 
            UniTask.Delay(
                TimeSpan.FromSeconds(spawnTime), 
                cancellationToken: cancellationToken);
        
        private void CancelSpawnTask()
        {
            _spawnCancellationTokenSource?.Cancel();
            _spawnCancellationTokenSource?.Dispose();
            _spawnCancellationTokenSource = null;
        }
    }
}
