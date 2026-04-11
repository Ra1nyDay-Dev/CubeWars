using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.Infrastructure.Services.Input;
using Project.Scripts.Infrastructure.Services.Input.DeviceTracker;
using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Installers.ProjectInstallers
{
    public class GameBootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private GameUI _gameUIPrefab;
        [SerializeField] private InputDeviceTracker _inputDeviceTrackerPrefab;

        public override void InstallBindings()
        {
            BindBootstrapServices();
            BindUIServices();
            BindAssetManagementServices();
            BindConfigProvider();
            BindSceneLoader();
            BindInputServices();
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

        public void BindInputServices()
        {
            Container.Bind<IInputDeviceTracker>().To<InputDeviceTracker>()
                .FromComponentInNewPrefab(_inputDeviceTrackerPrefab).AsSingle();
            
            Container.BindInterfacesTo<UnityNewInputService>().AsSingle();
        }

        public void Initialize() => 
            Container.Resolve<GameBootstrap>().ConfigureAndStartGame(); // toDo: Maybe rewrite to GameStateMachine somewhere
    }
}