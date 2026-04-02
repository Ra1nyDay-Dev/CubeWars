using Project.Scripts.Gameplay.Data.Configs;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.ConfigProvider
{
    public class ConfigProvider : IConfigProvider
    {
        private const string WEAPONS_CONFIGS_PATH = "Configs/Weapons";
        
        public WeaponConfig GetWeaponConfig(WeaponType weaponType)
        {
            string weaponTypeText = weaponType.ToString();
            return Resources.Load<WeaponConfig>($"{WEAPONS_CONFIGS_PATH}/{weaponTypeText}/{weaponTypeText}_Config");
        }
    }
}