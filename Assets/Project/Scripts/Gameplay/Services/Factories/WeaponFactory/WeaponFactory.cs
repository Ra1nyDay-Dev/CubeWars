using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Movement;
using Project.Scripts.Gameplay.Data.Configs.AttackConfigs;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.Weapons;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Services.Factories.WeaponFactory
{
    public class WeaponFactory : IWeaponFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly IConfigProvider _configProvider;
        
        [Inject]
        public WeaponFactory(
            IAssetProvider assetProvider,
            IInstantiator instantiator,
            IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            _assetProvider = assetProvider;
            _instantiator =  instantiator;
        }

        public GameObject CreateWeaponInHands(WeaponType weaponType,
            Transform weaponSlot,
            Transform attackStartPoint,
            GameObject selfHitbox, 
            Character owner)
        {
            WeaponConfig config = _configProvider.GetWeaponConfig(weaponType);
            
            GameObject weaponGameObject = _instantiator.InstantiatePrefab(config.WeaponPrefab);
            weaponGameObject.transform.SetParent(weaponSlot.transform, false);
            
            AttackBehaviour primaryAttack = CreateAttack(config.PrimaryAttackBehaviourConfig,
                attackStartPoint, selfHitbox, config.WeaponType);
            AttackBehaviour secondaryAttack = CreateAttack(config.SecondaryAttackBehaviourConfig,
                attackStartPoint, selfHitbox, config.WeaponType);
            
            IWeapon weapon = weaponGameObject.GetComponent<IWeapon>();
            weapon.Construct(config, primaryAttack, secondaryAttack, owner);
            
            weaponGameObject.GetComponent<WeaponAnimation>().Initialize();
            
            return weaponGameObject;
        }

        public GameObject CreateWeaponAtSpawn(WeaponType weaponType, Transform weaponSlot)
        {
            string assetPath = $"Weapons/{weaponType}/Prefabs/{weaponType}_WeaponSpawner";
            var prefab = _assetProvider.LoadAsset(assetPath);
            var weaponGameObject = _instantiator.InstantiatePrefab(prefab);
            weaponGameObject.transform.SetParent(weaponSlot.transform, false);

            return weaponGameObject;
        }

        private AttackBehaviour CreateAttack(
            AttackConfig config,
            Transform attackStartPoint,
            GameObject selfHitbox,
            WeaponType weaponType
        ) =>
            config?.CreateAttack(attackStartPoint, selfHitbox, weaponType);
    }
}