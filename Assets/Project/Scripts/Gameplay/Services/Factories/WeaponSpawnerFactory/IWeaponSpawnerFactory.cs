using Project.Scripts.Gameplay.Data;

namespace Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory
{
    public interface IWeaponSpawnerFactory
    {
        void Create(WeaponSpawnerData weaponSpawnerData);
    }
}