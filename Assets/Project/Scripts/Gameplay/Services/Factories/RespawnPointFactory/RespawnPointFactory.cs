using System;
using System.Collections.Generic;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.SpawnSystems.RespawnPont;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using Zenject;

namespace Project.Scripts.Gameplay.Services.Factories.RespawnPointFactory
{
    public class RespawnPointFactory : IRespawnPointFactory, IDisposable
    {
        public List<RespawnPoint> RepawnPoints => new(_repawnPoints);
        
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;
        
        private readonly List<RespawnPoint> _repawnPoints;

        [Inject]
        public RespawnPointFactory(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator =  instantiator;

            _repawnPoints = new List<RespawnPoint>();
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
            _repawnPoints.Add(respawnPoint);

            return respawnPoint;
        }
        
        public void Dispose() => 
            _repawnPoints.Clear();
    }
}