using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Gameplay.Services.CameraProvider;
using Project.Scripts.Gameplay.Services.Factories.BrainFactory;
using Project.Scripts.Gameplay.Services.Factories.CharacterFactory;
using Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.Infrastructure.SceneBootstrapHandlers
{
    public class GameplayTestBootstrap
    {
        private readonly IConfigProvider _configProvider;
        private readonly IWeaponSpawnerFactory _weaponSpawnerFactory;
        private readonly ICharacterFactory _characterFactory;
        private readonly ICameraProvider _cameraProvider;
        private readonly IBrainFactory _brainFactory;

        [Inject]
        public GameplayTestBootstrap(
            ICameraProvider cameraProvider,
            IConfigProvider configProvider,
            IWeaponSpawnerFactory weaponSpawnerFactory,
            ICharacterFactory characterFactory,
            IBrainFactory brainFactory)
        {
            _cameraProvider = cameraProvider;
            _configProvider = configProvider;
            _weaponSpawnerFactory = weaponSpawnerFactory;
            _characterFactory = characterFactory;
            _brainFactory = brainFactory;
        }

        public void PrepareGame()
        {
            // toDo: move to match factory or state machine or something better
            LevelConfig levelConfig = _configProvider.GetLevelConfig(SceneManager.GetActiveScene().name);

            foreach (var weaponSpawnerData in levelConfig.WeaponSpawners) 
                _weaponSpawnerFactory.Create(weaponSpawnerData);
            
            Character character = _characterFactory.Create(Vector3.zero.With(y:2));
            _cameraProvider.SetFollowTarget(character.transform);
            
            _brainFactory.Create(character, BrainType.Player);
            
            Character character2 = _characterFactory.Create(Vector3.zero.With(y:2, x:4));
            _brainFactory.Create(character2, BrainType.Empty);
        }
    }
}