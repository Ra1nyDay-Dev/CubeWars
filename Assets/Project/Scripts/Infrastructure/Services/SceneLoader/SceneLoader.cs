using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.Infrastructure.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly IGameUI _gameUI;
        private readonly ZenjectSceneLoader _zenjectLoader;

        [Inject]
        public SceneLoader(
            IGameUI gameUI, 
            ZenjectSceneLoader zenjectLoader)
        {
            _gameUI = gameUI;
            _zenjectLoader = zenjectLoader;
        }

        public async UniTask Load(string sceneName, Action onLoaded = null)
        {
            _gameUI.ShowLoadingScreen();
            await _zenjectLoader.LoadSceneAsync(sceneName).ToUniTask();

            _gameUI.HideLoadingScreen();
            onLoaded?.Invoke();
        }

        public async UniTask Load<TSceneParams>(
            string sceneName,
            TSceneParams sceneParams,
            Action onLoaded = null
        ) where TSceneParams : class, ISceneParams 
        {
            _gameUI.ShowLoadingScreen();
            await _zenjectLoader.LoadSceneAsync(
                sceneName, 
                LoadSceneMode.Single,
                container =>
                {
                    container.BindInstance(sceneParams);
                }
            ).ToUniTask();

            _gameUI.HideLoadingScreen();
            onLoaded?.Invoke();
        }
    }
}

    