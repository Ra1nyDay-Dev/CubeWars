using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.SpawnSystems.WeaponSpawn;

namespace Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory
{
    public interface IWeaponSpawnerFactory
    {
        WeaponSpawner Create(WeaponSpawnerData weaponSpawnerData);
    }
}