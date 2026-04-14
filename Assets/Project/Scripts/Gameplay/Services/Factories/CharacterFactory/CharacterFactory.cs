using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.CharacterSystems.Movement;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using Project.Scripts.Gameplay.Data.Configs.Health;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Services.Factories.CharacterFactory
{
    public class CharacterFactory : ICharacterFactory
    {
        private readonly IConfigProvider _configProvider;
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        [Inject]
        public CharacterFactory(
            IConfigProvider configProvider,
            IAssetProvider assetProvider,
            IInstantiator instantiator)
        {
            _configProvider = configProvider;
            _assetProvider = assetProvider;
            _instantiator =  instantiator;
        }
        
        public Character Create(Vector3 position, Quaternion rotation, Material material)
        {
            GameObject prefab = _assetProvider.LoadAsset(AssetPath.CUBE_GUY_CHARACTER);
            Character character = _instantiator.InstantiatePrefabForComponent<Character>(
                prefab,
                position: position,
                rotation,
                parentTransform: null
            );
            character.gameObject.SetActive(false);

            CharacterMovementConfig movementConfig = _configProvider.GetMovementConfig();
            character.Movement.Construct(movementConfig);
            character.Movement.Initialize();

            HealthConfig healthConfig = _configProvider.GetHealthConfig();
            character.Health.Construct(healthConfig);

            character.SetSkinMaterial(material);
            
            character.gameObject.SetActive(true);

            return character;
        }
    }
}