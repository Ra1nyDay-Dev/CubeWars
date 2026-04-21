using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using Project.Scripts.Gameplay.Data.Configs.Health;
using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.UI.Windows;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.ConfigProvider
{
    public interface IConfigProvider
    {
        void LoadAll();
        LevelConfig GetLevelConfig(string sceneName);
        WeaponConfig GetWeaponConfig(WeaponType weaponType);
        CharacterMovementConfig GetMovementConfig();
        HealthConfig GetHealthConfig();
        CharacterSkinMaterialsConfig GetCharacterSkinMaterialsConfig();
        GameObject GetWindowPrefab(WindowId windowId);
    }
}