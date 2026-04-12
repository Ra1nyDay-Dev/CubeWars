using Project.Scripts.Gameplay.Services.Factories.WeaponFactory;
using Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory;
using Project.Scripts.Infrastructure.SceneBootstrapHandlers;
using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Installers.SceneInstallers
{
    public class GameplayTestInstaller : MonoInstaller
    {
        [SerializeField] private GameplayUI _sceneUIPrefab;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameplayTestBootstrap>().AsSingle();
            BindSceneUI();
            BindFactories();
        }

        private void BindFactories()
        {
            Container.Bind<IWeaponSpawnerFactory>().To<WeaponSpawnerFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
        }

        private void BindSceneUI() =>
            Container
                .BindInterfacesAndSelfTo<GameplayUI>()
                .FromComponentInNewPrefab(_sceneUIPrefab)
                .AsSingle()
                .NonLazy();
    }
}