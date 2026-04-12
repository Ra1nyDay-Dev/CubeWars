using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.AttackSystems.Overlap;
using Project.Scripts.Gameplay.AttackSystems.Raycast;
using Project.Scripts.Gameplay.Characters.Movement;
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
            Material handsSkinMaterial,
            GameObject selfHitbox, 
            CharacterMovement owner)
        {
            WeaponConfig config = _configProvider.GetWeaponConfig(weaponType);
            GameObject weaponGameObject = _instantiator.InstantiatePrefab(config.WeaponPrefab);
            IWeapon weapon = weaponGameObject.GetComponent<IWeapon>();
            AttackBehaviour primaryAttack = CreateAttack(config.PrimaryAttackBehaviourConfig, config.PrimaryAttackType,
                attackStartPoint, selfHitbox, config.WeaponType);
            AttackBehaviour secondaryAttack = CreateAttack(config.SecondaryAttackBehaviourConfig, config.SecondaryAttackType,
                attackStartPoint, selfHitbox, config.WeaponType);
            weapon.Construct(config, primaryAttack, secondaryAttack, owner, handsSkinMaterial);
            weaponGameObject.transform.SetParent(weaponSlot.transform, false);
            weaponGameObject.GetComponent<WeaponAnimation>()?.Construct(weapon, owner);
            
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

        private AttackBehaviour CreateAttack(AttackConfig config, AttackType attackType,
            Transform attackStartPoint, GameObject selfHitbox, WeaponType weaponType)
        {
            if (!config)
                return null;

            switch (attackType)
            {
                case AttackType.Overlap:
                    if (config is OverlapAttackConfig overlapConfig)
                        return new OverlapAttack(overlapConfig, attackStartPoint, selfHitbox, weaponType);
                    break;
                case AttackType.Raycast:
                    if (config is RaycastAttackConfig raycastConfig)
                        return new RaycastAttack(raycastConfig, attackStartPoint, weaponType);
                    break;
            }
            return null;
        }
    }
}