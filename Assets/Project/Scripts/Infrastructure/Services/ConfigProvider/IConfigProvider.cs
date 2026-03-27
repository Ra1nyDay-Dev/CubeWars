using Project.Scripts.Gameplay.Data.Configs;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Infrastructure.Services.ServiceLocator;

namespace Project.Scripts.Infrastructure.Services.ConfigProvider
{
    public interface IConfigProvider : IProjectService
    {
        WeaponConfig GetWeaponConfig(WeaponType weaponType);
    }
}