using System;
using System.Collections.Generic;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.SpawnSystems.WeaponSpawn;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory
{
    public class WeaponSpawnerFactory : IWeaponSpawnerFactory, IDisposable
    {
        public IReadOnlyList<WeaponSpawner> Spawners => _spawners;
        
        private readonly List<WeaponSpawner> _spawners;
        
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        [Inject]
        public WeaponSpawnerFactory(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator =  instantiator;

            _spawners = new List<WeaponSpawner>();
        }

        public WeaponSpawner Create(WeaponSpawnerData weaponSpawnerData)
        {
            WeaponSpawner prefab = _assetProvider.LoadAsset<WeaponSpawner>(AssetPath.WEAPON_SPAWNER);
            WeaponSpawner weaponSpawner = _instantiator.InstantiatePrefabForComponent<WeaponSpawner>(
                    prefab,
                    position: weaponSpawnerData.Position,
                    Quaternion.identity,
                    parentTransform: null
                );
            weaponSpawner.Initialize(weaponSpawnerData);
            _spawners.Add(weaponSpawner);

            return weaponSpawner;
        }

        public void Dispose() => 
            _spawners.Clear();
    }
}