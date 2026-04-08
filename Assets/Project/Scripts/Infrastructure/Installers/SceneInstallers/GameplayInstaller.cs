using Project.Scripts.Gameplay.Services.Fabrics.Weapon;
using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Installers.SceneInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayUI _sceneUIPrefab;
        
        public override void InstallBindings()
        {
            BindSceneUI();
            BindWeaponFabric();
        }

        private void BindSceneUI()
        {
            Container
                .BindInterfacesAndSelfTo<GameplayUI>()
                .FromComponentInNewPrefab(_sceneUIPrefab)
                .AsSingle()
                .NonLazy();
        }


        private void BindWeaponFabric() =>
            Container
                .Bind<IWeaponFabric>()
                .To<WeaponFabric>()
                .AsSingle();
    }
}