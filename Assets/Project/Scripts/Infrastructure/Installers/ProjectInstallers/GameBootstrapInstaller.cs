using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.Infrastructure.Services.Input;
using Project.Scripts.Infrastructure.Services.Input.DeviceTracker;
using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.UI;
using Project.Scripts.UI.Services.LoadingScreen;
using Project.Scripts.UI.Services.WindowFactory;
using Project.Scripts.UI.Services.WindowService;
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
        
        public void Initialize() => 
            Container.Resolve<GameBootstrap>().ConfigureAndStartGame(); // toDo: Maybe rewrite to GameStateMachine somewhere

        private void BindBootstrapServices()
        {
            Container.BindInterfacesTo<GameBootstrapInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameBootstrap>().AsSingle();
        }

        private void BindUIServices()
        {
            GameUI gameUi = Container.InstantiatePrefabForComponent<GameUI>(_gameUIPrefab);
            Container.Bind<ILoadingScreen>().To<LoadingScreen>().FromInstance(gameUi.LoadingScreen).AsSingle();
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle().WithArguments(gameUi.GameUIRoot, gameUi.SceneUiRoot);
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
        }

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
    }
}