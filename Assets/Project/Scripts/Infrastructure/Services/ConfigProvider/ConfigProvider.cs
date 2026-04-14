using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using Project.Scripts.Gameplay.Data.Configs.Health;
using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.ConfigProvider
{
    public class ConfigProvider : IConfigProvider
    {
        private const string WEAPONS_CONFIGS_PATH = "Configs/Weapons";
        private const string LEVELS_CONFIGS_PATH = "Configs/Levels";
        private const string CHARACTER_MOVEMENT_CONFIG_PATH = "Configs/Characters/Movement/CharacterMovementConfig";
        private const string HEALTH_CONFIG_PATH = "Configs/Characters/Health/DefaultHealthConfig";
        private const string CHARACTER_SKIN_MATERIALS_CONFIG_PATH = "Configs/Characters/Skins/CharacterSkinMaterialsConfig";

        private Dictionary<string, LevelConfig> _levelConfigs;
        private Dictionary<WeaponType, WeaponConfig> _weaponConfigs;
        private CharacterMovementConfig _characterMovementConfig;
        private HealthConfig _healthConfig;
        private CharacterSkinMaterialsConfig _characterSkinMaterialsConfig;

        // toDo: rewrite to async load with Addressables
        public void LoadAll()
        {
            _levelConfigs = Resources
                .LoadAll<LevelConfig>(LEVELS_CONFIGS_PATH)
                .ToDictionary(x => x.SceneName, x => x );
            
            _weaponConfigs = Resources
                .LoadAll<WeaponConfig>(WEAPONS_CONFIGS_PATH)
                .ToDictionary(x => x.WeaponType, x => x );
            
            _characterMovementConfig = Resources.Load<CharacterMovementConfig>(CHARACTER_MOVEMENT_CONFIG_PATH);
            _characterSkinMaterialsConfig = Resources.Load<CharacterSkinMaterialsConfig>(CHARACTER_SKIN_MATERIALS_CONFIG_PATH);
            _healthConfig = Resources.Load<HealthConfig>(HEALTH_CONFIG_PATH);
        }

        public LevelConfig GetLevelConfig(string sceneName) => 
            _levelConfigs.GetValueOrDefault(sceneName);
        
        public WeaponConfig GetWeaponConfig(WeaponType weaponType) => 
            _weaponConfigs.GetValueOrDefault(weaponType);
        
        public CharacterMovementConfig GetMovementConfig() =>
            _characterMovementConfig;

        public CharacterSkinMaterialsConfig GetCharacterSkinMaterialsConfig() =>
            _characterSkinMaterialsConfig;

        public HealthConfig GetHealthConfig() =>
            _healthConfig;
    }
}