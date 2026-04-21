using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.UI;
using Project.Scripts.UI.Services.LoadingScreen;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.Infrastructure.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly ZenjectSceneLoader _zenjectLoader;

        [Inject]
        public SceneLoader(
            ILoadingScreen loadingScreen, 
            ZenjectSceneLoader zenjectLoader)
        {
            _loadingScreen = loadingScreen;
            _zenjectLoader = zenjectLoader;
        }

        public async UniTask Load(string sceneName, Action onLoaded = null)
        {
            _loadingScreen.Show();
            await _zenjectLoader.LoadSceneAsync(sceneName).ToUniTask();

            _loadingScreen.Hide();
            onLoaded?.Invoke();
        }

        public async UniTask Load<TSceneParams>(
            string sceneName,
            TSceneParams sceneParams,
            Action onLoaded = null
        ) where TSceneParams : class, ISceneParams 
        {
            _loadingScreen.Show();
            await _zenjectLoader.LoadSceneAsync(
                sceneName, 
                LoadSceneMode.Single,
                container =>
                {
                    container.BindInstance(sceneParams);
                }
            ).ToUniTask();

            _loadingScreen.Hide();
            onLoaded?.Invoke();
        }
    }
}

    