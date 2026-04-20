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
using Project.Scripts.Gameplay.Services.RespawnService;
using Project.Scripts.Infrastructure.Data;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Gameplay.Services.Factories.LevelFactory
{
    public class DeathmatchLevelFactory : ILevelFactory
    {
        protected readonly IConfigProvider _configProvider;
        protected readonly IWeaponSpawnerFactory _weaponSpawnerFactory;
        protected readonly IRespawnPointFactory _respawnPointFactory;
        protected readonly ICharacterFactory _characterFactory;
        protected readonly ICameraProvider _cameraProvider;
        protected readonly IBrainFactory _brainFactory;
        protected readonly IRespawnService _respawnService;

        public DeathmatchLevelFactory(
            ICameraProvider cameraProvider,
            IConfigProvider configProvider,
            IWeaponSpawnerFactory weaponSpawnerFactory,
            IRespawnPointFactory respawnPointFactory,
            ICharacterFactory characterFactory,
            IBrainFactory brainFactory,
            IRespawnService respawnService)
        {
            _cameraProvider = cameraProvider;
            _configProvider = configProvider;
            _weaponSpawnerFactory = weaponSpawnerFactory;
            _respawnPointFactory = respawnPointFactory;
            _characterFactory = characterFactory;
            _brainFactory = brainFactory;
            _respawnService = respawnService;
        }

        public virtual void Create(int playersCount)
        {
            LevelConfig levelConfig = _configProvider.GetLevelConfig(SceneManager.GetActiveScene().name);

            CreateWeaponSpawners(levelConfig);
            CreateRespawnPoints(levelConfig);
            CreateCharacters(levelConfig, playersCount);
            _respawnService.Initialize(levelConfig.RespawnTimeSeconds);
        }
        
        protected virtual void CreateWeaponSpawners(LevelConfig levelConfig)
        {
            foreach (var weaponSpawnerData in levelConfig.WeaponSpawners) 
                _weaponSpawnerFactory.Create(weaponSpawnerData);
        }
        protected virtual void CreateRespawnPoints(LevelConfig levelConfig)
        {
            foreach (var respawnPointData in levelConfig.RespawnPoints) 
                _respawnPointFactory.Create(respawnPointData);
        }

        protected virtual void CreateCharacters(LevelConfig levelConfig, int playersCount)
        {
            Queue<InitialPointData> initialPoints = new Queue<InitialPointData>(levelConfig.InitialPoints);
            
            if (initialPoints.Count < playersCount)
                throw new Exception($"Not enough initial points ({initialPoints.Count}) to spawn {playersCount} characters.");
            
            CreatePlayerCharacter(initialPoints.Dequeue());
            CreateBotsCharacters(initialPoints, playersCount - 1);
        }

        protected virtual void CreatePlayerCharacter(InitialPointData initialPoint)
        {
            CharacterSkinMaterialsConfig skinsConfig = _configProvider.GetCharacterSkinMaterialsConfig();
            Character playerCharacter = _characterFactory.Create(
                initialPoint.Position, 
                initialPoint.Rotation,
                skinsConfig.PlayerSkinMaterial);
            _brainFactory.Create(playerCharacter, BrainType.Player);
            _cameraProvider.SetFollowTarget(playerCharacter.transform);
        }

        protected virtual void CreateBotsCharacters(Queue<InitialPointData> initialPoints, int botsCount)
        {
            CharacterSkinMaterialsConfig skinsConfig = _configProvider.GetCharacterSkinMaterialsConfig();
            Queue<Material> botsMaterials = new(
                skinsConfig.Materials
                    .ToList()
                    .Shuffle()
                    .Where(x => x != skinsConfig.PlayerSkinMaterial));
            
            for (int i = 0; i < botsCount; i++)
            {
                InitialPointData initialPointData = initialPoints.Dequeue();
                Material material = botsMaterials.Dequeue();
                
                Character emptyCharacter = _characterFactory.Create(initialPointData.Position, initialPointData.Rotation, material);
                _brainFactory.Create(emptyCharacter, BrainType.Ai);
                
                botsMaterials.Enqueue(material);
            }
        }
    }
    
}