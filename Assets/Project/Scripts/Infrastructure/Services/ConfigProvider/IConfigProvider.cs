using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;

namespace Project.Scripts.Infrastructure.Services.ConfigProvider
{
    public interface IConfigProvider
    {
        WeaponConfig GetWeaponConfig(WeaponType weaponType);
    }
}