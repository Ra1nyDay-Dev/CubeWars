using Project.Scripts.Gameplay.Services.Fabrics.Weapon;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.Infrastructure.Services.ServiceLocator;
using Project.Scripts.UI;

namespace Project.Scripts.Infrastructure.EntryPoints
{
    public class GameplayEntryPoint : SceneEntryPoint
    {
        private IAssetProvider _assetProvider;
        private IConfigProvider _configProvider;
        
        public override void Run(GameUI gameUI)
        {
            base.Run(gameUI);
            GetProjectDependencies();
            RegisterSceneServices();
        }

        private void GetProjectDependencies()
        {
            _assetProvider = ProjectServices.Container.Get<IAssetProvider>();
            _configProvider = ProjectServices.Container.Get<IConfigProvider>();
        }

        private void RegisterSceneServices()
        {
            SceneServices.Container.Register<IWeaponFabric>(new WeaponFabric(_assetProvider, _configProvider));
        }
    }
}
