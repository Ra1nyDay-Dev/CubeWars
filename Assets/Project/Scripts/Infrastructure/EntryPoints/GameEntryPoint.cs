using Project.Scripts.Gameplay.Data;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.Infrastructure.Services.CoroutineRunner;
using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.Infrastructure.Services.ServiceLocator;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Project.Scripts.Infrastructure.EntryPoints
{
    public class GameEntryPoint
    {
        private const string FIRST_SCENE = Scenes.GAMEPLAY;

        private static GameEntryPoint _instance;
        private ISceneLoader _sceneLoader;
        private CoroutineRunner _coroutineRunner;
        private readonly IAssetProvider _assetProvider;
        private readonly GameUI _gameUI;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnApplicationRun()
        {
            _instance = new GameEntryPoint();
            _instance.RunGame();
        }
        
        private GameEntryPoint()
        {
            SceneManager.LoadScene(Scenes.BOOT);
            _assetProvider = new AssetProvider();
            
            _gameUI = _assetProvider.Instantiate(AssetPath.GAME_UI).GetComponent<GameUI>();
            Object.DontDestroyOnLoad(_gameUI.gameObject);
            
            _gameUI.ShowLoadingScreen();
        }


        private void RunGame()
        {
            RegisterProjectServices();
            _sceneLoader.Load(FIRST_SCENE);
        }

        private void RegisterProjectServices()
        {
            RegisterCoroutineRunner();
            ProjectServices.Container.Register<IAssetProvider>(_assetProvider);
            ProjectServices.Container.Register<IConfigProvider>(new ConfigProvider());
            ProjectServices.Container.Register<IGameUI>(_gameUI); ;
            RegisterSceneLoader();
        }

        private void RegisterCoroutineRunner()
        {
            _coroutineRunner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
            ProjectServices.Container.Register<ICoroutineRunner>(_coroutineRunner);
            Object.DontDestroyOnLoad(_coroutineRunner.gameObject);
        }

        private void RegisterSceneLoader()
        {
            _sceneLoader = new SceneLoader(_coroutineRunner, _gameUI);
            ProjectServices.Container.Register<ISceneLoader>(_sceneLoader);
        }
    }
}