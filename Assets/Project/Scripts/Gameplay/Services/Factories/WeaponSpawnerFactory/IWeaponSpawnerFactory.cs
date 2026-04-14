using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Weapons.WeaponSpawn;

namespace Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory
{
    public interface IWeaponSpawnerFactory
    {
        WeaponSpawner Create(WeaponSpawnerData weaponSpawnerData);
    }
}