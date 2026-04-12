using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;

namespace Project.Scripts.Infrastructure.Services.ConfigProvider
{
    public interface IConfigProvider
    {
        void LoadAll();
        LevelConfig GetLevelConfig(string sceneName);
        WeaponConfig GetWeaponConfig(WeaponType weaponType);
    }
}