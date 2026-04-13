using System.Collections.Generic;
using System.Linq;
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

        private Dictionary<string, LevelConfig> _levels;
        private Dictionary<WeaponType, WeaponConfig> _weapons;

        // toDo: rewrite to async load with Addressables
        public void LoadAll()
        {
            _levels = Resources
                .LoadAll<LevelConfig>(LEVELS_CONFIGS_PATH)
                .ToDictionary(x => x.SceneName, x => x );
            
            _weapons = Resources
                .LoadAll<WeaponConfig>(WEAPONS_CONFIGS_PATH)
                .ToDictionary(x => x.WeaponType, x => x );
        }
        
        public LevelConfig GetLevelConfig(string sceneName) => 
            _levels.GetValueOrDefault(sceneName);
        
        public WeaponConfig GetWeaponConfig(WeaponType weaponType) => 
            _weapons.GetValueOrDefault(weaponType);
    }
}