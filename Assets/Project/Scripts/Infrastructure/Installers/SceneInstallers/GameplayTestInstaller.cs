using Project.Scripts.Gameplay.Services.CameraProvider;
using Project.Scripts.Gameplay.Services.Factories.BrainFactory;
using Project.Scripts.Gameplay.Services.Factories.CharacterFactory;
using Project.Scripts.Gameplay.Services.Factories.LevelFactory;
using Project.Scripts.Gameplay.Services.Factories.RespawnPointFactory;
using Project.Scripts.Gameplay.Services.Factories.WeaponFactory;
using Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory;
using Project.Scripts.Gameplay.Services.RespawnService;
using Project.Scripts.Infrastructure.SceneBootstrapHandlers;
using Project.Scripts.UI;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Installers.SceneInstallers
{
    public class GameplayTestInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private GameplayUI _sceneUIPrefab;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        
        public override void InstallBindings()
        {
            BindBootstrapServices();
            Container.BindInterfacesTo<RespawnService>().AsSingle();
            BindCameraServices();
            BindUIServices();
            BindFactories();
        }

        public void Initialize() => 
            Container.Resolve<GameplayTestBootstrap>().PrepareGame();

        private void BindBootstrapServices()
        {
            Container.BindInterfacesTo<GameplayTestInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameplayTestBootstrap>().AsSingle();
        }

        private void BindCameraServices() =>
            Container
                .Bind<ICameraProvider>().To<CinemachineCameraProvider>()
                .AsSingle()
                .WithArguments(_mainCamera, _cinemachineCamera);


        private void BindUIServices() =>
            Container
                .BindInterfacesAndSelfTo<GameplayUI>()
                .FromComponentInNewPrefab(_sceneUIPrefab)
                .AsSingle()
                .NonLazy();

        private void BindFactories()
        {
            Container.Bind<ILevelFactory>().To<LevelFactory>().AsSingle();
            Container.Bind<IWeaponSpawnerFactory>().To<WeaponSpawnerFactory>().AsSingle();
            Container.BindInterfacesTo<RespawnPointFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
            Container.BindInterfacesTo<CharacterFactory>().AsSingle();
            Container.BindInterfacesTo<BrainFactory>().AsSingle();
        }
    }
}