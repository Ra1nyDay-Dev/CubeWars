using System;
using System.Collections.Generic;
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

            CreateWeaponSpawners(levelConfig);
            CreateCharacters(levelConfig);
        }

        private void CreateWeaponSpawners(LevelConfig levelConfig)
        {
            foreach (var weaponSpawnerData in levelConfig.WeaponSpawners) 
                _weaponSpawnerFactory.Create(weaponSpawnerData);
        }

        private void CreateCharacters(LevelConfig levelConfig)
        {
            int playersCount = 7;
            Queue<InitialPointData> initialPoints = new Queue<InitialPointData>(levelConfig.InitialPoints);
            
            if (initialPoints.Count < playersCount)
                throw new Exception($"Not enough initial points ({initialPoints.Count}) to spawn {playersCount} characters.");
            
            CreatePlayerCharacter(initialPoints);
            CreateBotsCharacters(playersCount, initialPoints);
        }

        private void CreatePlayerCharacter(Queue<InitialPointData> initialPoints)
        {
            Character playerCharacter = _characterFactory.Create(initialPoints.Dequeue());
            _cameraProvider.SetFollowTarget(playerCharacter.transform);
            _brainFactory.Create(playerCharacter, BrainType.Player);
        }

        private void CreateBotsCharacters(int playersCount, Queue<InitialPointData> initialPoints)
        {
            for (int i = 0; i < playersCount - 1; i++)
            {
                Character emptyCharacter = _characterFactory.Create(initialPoints.Dequeue());
                _brainFactory.Create(emptyCharacter, BrainType.Empty);
            }
        }
    }
}