using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.Infrastructure.SceneBootstrapHandlers
{
    public class GameplayTestBootstrap : IInitializable
    {
        private readonly IConfigProvider _configProvider;
        private readonly IWeaponSpawnerFactory _weaponSpawnerFactory;

        [Inject]
        public GameplayTestBootstrap(IConfigProvider configProvider, IWeaponSpawnerFactory weaponSpawnerFactory)
        {
            _configProvider = configProvider;
            _weaponSpawnerFactory = weaponSpawnerFactory;
        }

        public void Initialize() => 
            PrepareGame();

        public void PrepareGame()
        {
            LevelConfig levelConfig = _configProvider.GetLevelConfig(SceneManager.GetActiveScene().name);

            foreach (var weaponSpawnerData in levelConfig.WeaponSpawners) 
                _weaponSpawnerFactory.Create(weaponSpawnerData);
        }
    }
}