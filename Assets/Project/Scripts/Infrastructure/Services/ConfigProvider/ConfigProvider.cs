using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Gameplay.Data.Configs.AI;
using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using Project.Scripts.Gameplay.Data.Configs.Health;
using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.UI.Data.Configs;
using Project.Scripts.UI.Windows;
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
        private const string WINDOWS_CONFIG_PATH = "Configs/UI/Windows/WindowsConfig";
        private const string AI_BOT_CONFIG_PATH = "Configs/AI/AiBotConfig";
        private const string AI_WEAPON_PRIORITY_CONFIG_PATH = "Configs/AI/AiWeaponPriorityConfig";

        private Dictionary<string, LevelConfig> _levelConfigs;
        private Dictionary<WeaponType, WeaponConfig> _weaponConfigs;
        private CharacterMovementConfig _characterMovementConfig;
        private HealthConfig _healthConfig;
        private CharacterSkinMaterialsConfig _characterSkinMaterialsConfig;
        private Dictionary<WindowId, GameObject> _windowPrefabsById;
        private BotConfig _botsConfig;
        private AiWeaponPriorityConfig _weaponPriorityConfig;

        // toDo: rewrite to async load with Addressables
        public void LoadAll()
        {
            LoadLevels();
            LoadWeapons();
            LoadCharacterConfigs();
            LoadWindows();
            LoadAIConfigs();
        }

        public LevelConfig GetLevelConfig(string sceneName) => 
            _levelConfigs.GetValueOrDefault(sceneName);

        public List<LevelConfig> GetAllLevelsConfigs() => 
           new List<LevelConfig>(_levelConfigs.Values);

        public WeaponConfig GetWeaponConfig(WeaponType weaponType) => 
            _weaponConfigs.GetValueOrDefault(weaponType);

        public CharacterMovementConfig GetMovementConfig() =>
            _characterMovementConfig;

        public CharacterSkinMaterialsConfig GetCharacterSkinMaterialsConfig() =>
            _characterSkinMaterialsConfig;

        public HealthConfig GetHealthConfig() =>
            _healthConfig;

        public GameObject GetWindowPrefab(WindowId id) =>
            _windowPrefabsById.TryGetValue(id, out GameObject prefab)
                ? prefab
                : throw new Exception($"Prefab config for window {id} was not found");

        public BotConfig GetAiBotConfig() =>
            _botsConfig;
        
        public AiWeaponPriorityConfig GetAiWeaponPriorityConfig() =>
            _weaponPriorityConfig;

        private void LoadLevels() =>
            _levelConfigs = Resources
                .LoadAll<LevelConfig>(LEVELS_CONFIGS_PATH)
                .ToDictionary(x => x.SceneName, x => x );

        private void LoadWeapons() =>
            _weaponConfigs = Resources
                .LoadAll<WeaponConfig>(WEAPONS_CONFIGS_PATH)
                .ToDictionary(x => x.WeaponType, x => x );

        private void LoadCharacterConfigs()
        {
            _characterMovementConfig = Resources.Load<CharacterMovementConfig>(CHARACTER_MOVEMENT_CONFIG_PATH);
            _characterSkinMaterialsConfig = Resources.Load<CharacterSkinMaterialsConfig>(CHARACTER_SKIN_MATERIALS_CONFIG_PATH);
            _healthConfig = Resources.Load<HealthConfig>(HEALTH_CONFIG_PATH);
        }

        private void LoadWindows() =>
            _windowPrefabsById = Resources
                .Load<WindowsConfig>(WINDOWS_CONFIG_PATH)
                .WindowConfigs
                .ToDictionary(x => x.Id, x => x.Prefab);

        private void LoadAIConfigs()
        {
            _botsConfig = Resources.Load<BotConfig>(AI_BOT_CONFIG_PATH);
            _weaponPriorityConfig = Resources.Load<AiWeaponPriorityConfig>(AI_WEAPON_PRIORITY_CONFIG_PATH);
        }
    }
}