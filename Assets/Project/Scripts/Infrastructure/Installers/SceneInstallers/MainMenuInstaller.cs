using Project.Scripts.Gameplay.Services.Factories.CharacterFactory;
using Project.Scripts.Gameplay.Services.Factories.WeaponFactory;
using Project.Scripts.Infrastructure.SceneBootstrapHandlers;
using Project.Scripts.UI;
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