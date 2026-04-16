using Project.Scripts.Gameplay.SpawnSystems.RespawnPont;

namespace Project.Scripts.Gameplay.Services.RespawnService
{
    public interface IRespawnService
    {
        bool TryGetAvailableRespawnPoint(out RespawnPoint respawnPoint);
        void Initialize(float levelRespawnTime);
    }
}