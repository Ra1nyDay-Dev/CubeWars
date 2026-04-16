using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.SpawnSystems.WeaponSpawn;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory
{
    public class WeaponSpawnerFactory : IWeaponSpawnerFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        [Inject]
        public WeaponSpawnerFactory(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator =  instantiator;
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

            return weaponSpawner;
        }
    }
}