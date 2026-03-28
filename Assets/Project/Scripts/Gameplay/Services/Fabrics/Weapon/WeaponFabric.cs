using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.AttackSystems.Overlap;
using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Configs;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.Weapons;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Fabrics.Weapon
{
    public class WeaponFabric : IWeaponFabric
    {
        private readonly IConfigProvider _configProvider;
        private readonly IAssetProvider _assetProvider;
        
        public WeaponFabric(IAssetProvider assetProvider, IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            _assetProvider = assetProvider;
        }

        public GameObject CreateWeaponInHands(WeaponType weaponType,
            Transform weaponSlot,
            Transform overlapAttackStartPoint,
            Material handsSkinMaterial,
            GameObject selfHitbox, 
            Character owner)
        {
            WeaponConfig config = _configProvider.GetWeaponConfig(weaponType);
            GameObject weaponGameObject = _assetProvider.Instantiate(config.WeaponPrefab);
            IWeapon weapon = weaponGameObject.GetComponent<IWeapon>();
            AttackBehaviour primaryAttack = CreateAttack(config.PrimaryAttackBehaviourConfig, config.PrimaryAttackType, overlapAttackStartPoint, selfHitbox);
            AttackBehaviour secondaryAttack = CreateAttack(config.SecondaryAttackBehaviourConfig, config.SecondaryAttackType, overlapAttackStartPoint, selfHitbox);
            weapon.Construct(primaryAttack, secondaryAttack, owner, handsSkinMaterial);
            weaponGameObject.transform.SetParent(weaponSlot.transform, false);
            weaponGameObject.GetComponent<WeaponAnimation>().Construct(weapon, owner);
            
            return weaponGameObject;
        }

        private AttackBehaviour CreateAttack(AttackConfig config, AttackType attackType,
            Transform overlapAttackStartPoint, GameObject selfHitbox)
        {
            if (!config)
                return null;

            switch (attackType)
            {
                case AttackType.Overlap:
                    if (config is OverlapAttackConfig overlapConfig)
                        return new OverlapAttack(overlapConfig, overlapAttackStartPoint, selfHitbox);
                    break;
            }
            return null;
        }
    }
}