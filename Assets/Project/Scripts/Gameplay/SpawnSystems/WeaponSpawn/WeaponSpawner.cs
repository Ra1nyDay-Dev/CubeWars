using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.CharacterSystems.Interactions;
using Project.Scripts.Gameplay.CharacterSystems.Inventory;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.Services.Factories.WeaponFactory;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.SpawnSystems.WeaponSpawn
{
    public class WeaponSpawner : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject _weaponSlot;
        
        public bool IsWeaponAvailable { get; private set; }
        public WeaponType WeaponType {get; private set;}

        private IWeaponFactory _weaponFactory;

        private float _spawnTime;
        private bool _spawnOnStart;
        private WeaponSpawnerAnimation _spawnerAnimation;
        
        private CancellationTokenSource _spawnCancellationTokenSource;
        private bool _isActive;

        [Inject]
        public void Construct(IWeaponFactory weaponFactory) => 
            _weaponFactory = weaponFactory;

        public void Initialize(WeaponSpawnerData data)
        {
            WeaponType = data.WeaponType;
            _spawnTime = data.SpawnTime;
            _spawnOnStart = data.SpawnOnStart;
            
            _spawnerAnimation = GetComponent<WeaponSpawnerAnimation>();
            _weaponFactory.CreateWeaponAtSpawn(WeaponType, _weaponSlot.transform);
            _weaponSlot.SetActive(false);
            ActivateSpawn();
        }

        private void OnDestroy() => 
            HideWeapon();


        public void Interact(InteractorUnit interactor)
        {
            if (!IsWeaponAvailable
                || !interactor.TryGetComponent(out WeaponArsenal arsenal)
                || arsenal.CurrentWeapon?.WeaponType == WeaponType)
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
            IsWeaponAvailable = true;
            _weaponSlot.SetActive(true);
            _spawnerAnimation?.StartAnimation();
        }

        private void HideWeapon()
        {
            IsWeaponAvailable = false;
            CancelSpawnTask();
            _weaponSlot.SetActive(false);
            _spawnerAnimation?.StopAnimation();
        }

        private void TakeWeapon(WeaponArsenal characterArsenal)
        {
            HideWeapon();
            characterArsenal.ChangeWeapon(WeaponType);
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
