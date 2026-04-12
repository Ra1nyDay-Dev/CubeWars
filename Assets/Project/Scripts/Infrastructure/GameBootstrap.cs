using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.UI;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class GameBootstrap
    {
        private const string FIRST_SCENE = Scenes.GAMEPLAY_TEST;
        
        private readonly IGameUI _gameUI;
        private readonly IConfigProvider _configProvider;
        private readonly ISceneLoader _sceneLoader;

        [Inject]
        public GameBootstrap(
            IGameUI gameUI,
            IConfigProvider configProvider,
            ISceneLoader sceneLoader)
        {
            _gameUI = gameUI;
            _configProvider = configProvider;
            _sceneLoader = sceneLoader;
        }

        public void ConfigureAndStartGame()
        {
            _gameUI.ShowLoadingScreen();
            _configProvider.LoadAll();
            _sceneLoader.Load(FIRST_SCENE).Forget();
        }
    }
}