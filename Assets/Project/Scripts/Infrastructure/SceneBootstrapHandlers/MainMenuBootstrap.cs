using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Gameplay.Data.Enums;
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
using Random = System.Random;

namespace Project.Scripts.Infrastructure.SceneBootstrapHandlers
{
    public class MainMenuBootstrap
    {
        private readonly Vector3 _characterInitialPosition = new Vector3(0, 1.8f, 0);  
        private readonly Quaternion _characterInitialRotation = Quaternion.Euler(new Vector3(0,245,0));  
            
        private readonly ICharacterFactory _characterFactory;
        private readonly IConfigProvider _configProvider;

        [Inject]
        public MainMenuBootstrap(
            IConfigProvider configProvider, 
            ICharacterFactory characterFactory)
        {
            _configProvider = configProvider;
            _characterFactory = characterFactory;
        }

        public void PrepareScene()
        {
            Character playerCharacter = CreateCharacter();
            GetRandomWeapon(playerCharacter);
        }

        private static void GetRandomWeapon(Character playerCharacter)
        {
            WeaponType[] weaponTypes = (WeaponType[])Enum.GetValues(typeof(WeaponType));
            WeaponType randomWeapon = weaponTypes[UnityEngine.Random.Range(0, weaponTypes.Length)];
            playerCharacter.WeaponArsenal.ChangeWeapon(randomWeapon);
        }

        private Character CreateCharacter()
        {
            CharacterSkinMaterialsConfig skinsConfig = _configProvider.GetCharacterSkinMaterialsConfig();
            Character playerCharacter = _characterFactory.Create(
                _characterInitialPosition, 
                _characterInitialRotation,
                skinsConfig.PlayerSkinMaterial);
            
            playerCharacter.CharacterUI.HideAll();
            
            return playerCharacter;
        }
    }
}