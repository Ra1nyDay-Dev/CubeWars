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
using Project.Scripts.Gameplay.Services.Factories.LevelFactory;
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
        private readonly ILevelFactory _levelFactory;

        [Inject]
        public GameplayTestBootstrap(ILevelFactory levelFactory) => 
            _levelFactory = levelFactory;

        public void PrepareGame() => 
            _levelFactory.Create(playersCount: 7); // toDo: send players count as scene params
    }
}