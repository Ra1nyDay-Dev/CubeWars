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
    public class MainMenuInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private MainMenuUI _sceneUIPrefab;
        
        public override void InstallBindings()
        {
            BindBootstrapServices();
            BindUIServices();
            BindFactories();
        }

        public void Initialize() => 
            Container.Resolve<MainMenuBootstrap>().PrepareScene();

        private void BindBootstrapServices()
        {
            Container.BindInterfacesTo<MainMenuInstaller>().FromInstance(this).AsSingle();
            Container.Bind<MainMenuBootstrap>().AsSingle();
        }

        private void BindUIServices() =>
            Container
                .BindInterfacesAndSelfTo<MainMenuUI>()
                .FromComponentInNewPrefab(_sceneUIPrefab)
                .AsSingle()
                .NonLazy();
        
        private void BindFactories()
        {
            Container.BindInterfacesTo<CharacterFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
        }
    }
}