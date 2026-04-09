using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.Infrastructure.Services.Input;
using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Installers.ProjectInstallers
{
    public class GameBootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private GameUI _gameUIPrefab;

        public override void InstallBindings()
        {
            BindBootstrapServices();
            BindUIServices();
            BindAssetManagementServices();
            BindConfigProvider();
            BindSceneLoader();
            BindInputService();
        }

        private void BindBootstrapServices()
        {
            Container.BindInterfacesTo<GameBootstrapInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameBootstrap>().AsSingle();
        }

        private void BindUIServices() =>
            Container.Bind<IGameUI>().To<GameUI>().FromComponentInNewPrefab(_gameUIPrefab).AsSingle();

        private void BindAssetManagementServices() =>
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();

        private void BindConfigProvider() =>
            Container.Bind<IConfigProvider>().To<ConfigProvider>().AsSingle();

        private void BindSceneLoader() =>
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();

        private void BindInputService() => 
            Container.Bind<IInputService>().To<KeyboardOldInputService>().AsSingle();

        public void Initialize() => 
            Container.Resolve<GameBootstrap>().ConfigureAndStartGame(); // toDo: Maybe rewrite to GameStateMachine somewhere
    }
}