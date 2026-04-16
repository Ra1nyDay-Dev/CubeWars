using System.Collections.Generic;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.SpawnSystems.RespawnPont;

namespace Project.Scripts.Gameplay.Services.Factories.RespawnPointFactory
{
    public interface IRespawnPointFactory
    {
        List<RespawnPoint> RepawnPoints { get; }
        RespawnPoint Create(RespawnPointData weaponSpawnerData);
    }
}