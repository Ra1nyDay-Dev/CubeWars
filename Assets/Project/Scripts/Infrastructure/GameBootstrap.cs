using Cysharp.Threading.Tasks;
using Project.Scripts.Infrastructure.Data;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.UI;
using Project.Scripts.UI.Services.LoadingScreen;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class GameBootstrap
    {
        private const string FIRST_SCENE = Scenes.MAIN_MENU;
        
        private readonly ILoadingScreen _loadingScreen;
        private readonly IConfigProvider _configProvider;
        private readonly ISceneLoader _sceneLoader;

        [Inject]
        public GameBootstrap(
            ILoadingScreen loadingScreen,
            IConfigProvider configProvider,
            ISceneLoader sceneLoader)
        {
            _loadingScreen = loadingScreen;
            _configProvider = configProvider;
            _sceneLoader = sceneLoader;
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnGameRun() => 
            SceneManager.LoadScene(Scenes.BOOT);

        public void ConfigureAndStartGame()
        {
            _loadingScreen.Show();
            _configProvider.LoadAll();
            _sceneLoader.Load(FIRST_SCENE).Forget();
        }
    }
}