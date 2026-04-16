using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Gameplay.Services.CameraProvider;
using Project.Scripts.Gameplay.Services.Factories.BrainFactory;
using Project.Scripts.Gameplay.Services.Factories.CharacterFactory;
using Project.Scripts.Gameplay.Services.Factories.RespawnPointFactory;
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
        private readonly IRespawnPointFactory _respawnPointFactory;
        private readonly ICharacterFactory _characterFactory;
        private readonly ICameraProvider _cameraProvider;
        private readonly IBrainFactory _brainFactory;

        [Inject]
        public GameplayTestBootstrap(
            ICameraProvider cameraProvider,
            IConfigProvider configProvider,
            IWeaponSpawnerFactory weaponSpawnerFactory,
            IRespawnPointFactory respawnPointFactory,
            ICharacterFactory characterFactory,
            IBrainFactory brainFactory)
        {
            _cameraProvider = cameraProvider;
            _configProvider = configProvider;
            _weaponSpawnerFactory = weaponSpawnerFactory;
            _respawnPointFactory = respawnPointFactory;
            _characterFactory = characterFactory;
            _brainFactory = brainFactory;
        }

        public void PrepareGame()
        {
            // toDo: move to match factory or state machine or something better
            LevelConfig levelConfig = _configProvider.GetLevelConfig(SceneManager.GetActiveScene().name);

            CreateWeaponSpawners(levelConfig);
            CreateRespawnPoints(levelConfig);
            CreateCharacters(levelConfig);
        }

        private void CreateWeaponSpawners(LevelConfig levelConfig)
        {
            foreach (var weaponSpawnerData in levelConfig.WeaponSpawners) 
                _weaponSpawnerFactory.Create(weaponSpawnerData);
        }
        private void CreateRespawnPoints(LevelConfig levelConfig)
        {
            foreach (var respawnPointData in levelConfig.RespawnPoints) 
                _respawnPointFactory.Create(respawnPointData);
        }

        private void CreateCharacters(LevelConfig levelConfig)
        {
            int playersCount = 7; // test. Will be moved to sceneParams in menu
           
            Queue<InitialPointData> initialPoints = new Queue<InitialPointData>(levelConfig.InitialPoints);
            
            if (initialPoints.Count < playersCount)
                throw new Exception($"Not enough initial points ({initialPoints.Count}) to spawn {playersCount} characters.");
            
            CreatePlayerCharacter(initialPoints.Dequeue());
            CreateBotsCharacters(initialPoints, playersCount - 1);
        }

        private void CreatePlayerCharacter(InitialPointData initialPoint)
        {
            CharacterSkinMaterialsConfig skinsConfig = _configProvider.GetCharacterSkinMaterialsConfig();
            Character playerCharacter = _characterFactory.Create(
                initialPoint.Position, 
                initialPoint.Rotation,
                skinsConfig.PlayerSkinMaterial);
            _cameraProvider.SetFollowTarget(playerCharacter.transform);
            _brainFactory.Create(playerCharacter, BrainType.Player);
        }

        private void CreateBotsCharacters(Queue<InitialPointData> initialPoints, int botsCount)
        {
            CharacterSkinMaterialsConfig skinsConfig = _configProvider.GetCharacterSkinMaterialsConfig();
            Queue<Material> botsMaterials = new(
                skinsConfig.Materials
                    .Where(x => x != skinsConfig.PlayerSkinMaterial));
            
            for (int i = 0; i < botsCount; i++)
            {
                InitialPointData initialPointData = initialPoints.Dequeue();
                Material material = botsMaterials.Dequeue();
                Character emptyCharacter = _characterFactory.Create(initialPointData.Position, initialPointData.Rotation, material);
                _brainFactory.Create(emptyCharacter, BrainType.Empty);
                botsMaterials.Enqueue(material);
            }
        }
    }
}