using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Installers.ProjectInstallers
{
    public class GameBootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameUI _gameUIPrefab;

        public override void InstallBindings()
        {
            BindGameUI();
            BindAssetProvider();
            BindConfigProvider();
            BindSceneLoader();
            BindGameBootstrap();
            
            SetInitializeExecutionOrders();
        }

        private void BindGameUI() =>
            Container
                .BindInterfacesAndSelfTo<GameUI>()
                .FromComponentInNewPrefab(_gameUIPrefab)
                .AsSingle()
                .NonLazy();

        private void BindAssetProvider() =>
            Container
                .Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle()
                .NonLazy();

        private void BindConfigProvider() =>
            Container
                .Bind<IConfigProvider>()
                .To<ConfigProvider>()
                .AsSingle()
                .NonLazy();

        private void BindSceneLoader() =>
            Container
                .Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle()
                .NonLazy();

        private void BindGameBootstrap() =>
            Container.BindInterfacesTo<GameBootstrap>()
                .AsSingle()
                .NonLazy();

        private void SetInitializeExecutionOrders()
        {
            Container.BindExecutionOrder<GameUI>(-1000); // Show loading screen before all service inits
            Container.BindExecutionOrder<GameBootstrap>(1000); // Configure and start game after all service inits
        }
    }
}