using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.SpawnSystems.RespawnPont;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using Zenject;

namespace Project.Scripts.Gameplay.Services.Factories.RespawnPointFactory
{
    public class RespawnPointFactory : IRespawnPointFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        [Inject]
        public RespawnPointFactory(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator =  instantiator;
        }

        public RespawnPoint Create(RespawnPointData data)
        {
            RespawnPoint prefab = _assetProvider.LoadAsset<RespawnPoint>(AssetPath.RESPAWN_POINT);
            RespawnPoint respawnPoint = _instantiator.InstantiatePrefabForComponent<RespawnPoint>(
                    prefab,
                    position: data.PointPosition,
                    data.PointRotation,
                    parentTransform: null
                );
            respawnPoint.Construct(data);

            return respawnPoint;
        }
    }
}