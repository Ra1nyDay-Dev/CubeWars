using Project.Scripts.Gameplay.Services.CameraProvider;
using Project.Scripts.Gameplay.Services.Factories.BrainFactory;
using Project.Scripts.Gameplay.Services.Factories.CharacterFactory;
using Project.Scripts.Gameplay.Services.Factories.RespawnPointFactory;
using Project.Scripts.Gameplay.Services.Factories.WeaponFactory;
using Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory;
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
            Container.BindInterfacesTo<GameplayTestInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameplayTestBootstrap>().AsSingle();
            BindCameraServices();
            BindSceneUI();
            BindFactories();
        }

        private void BindCameraServices() =>
            Container
                .Bind<ICameraProvider>().To<CinemachineCameraProvider>()
                .AsSingle()
                .WithArguments(_mainCamera, _cinemachineCamera);
                

        private void BindFactories()
        {
            Container.Bind<IWeaponSpawnerFactory>().To<WeaponSpawnerFactory>().AsSingle();
            Container.Bind<IRespawnPointFactory>().To<RespawnPointFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
            Container.Bind<ICharacterFactory>().To<CharacterFactory>().AsSingle();
            Container.BindInterfacesTo<BrainFactory>().AsSingle();
        }

        private void BindSceneUI() =>
            Container
                .BindInterfacesAndSelfTo<GameplayUI>()
                .FromComponentInNewPrefab(_sceneUIPrefab)
                .AsSingle()
                .NonLazy();

        public void Initialize() => 
            Container.Resolve<GameplayTestBootstrap>().PrepareGame();
    }
}