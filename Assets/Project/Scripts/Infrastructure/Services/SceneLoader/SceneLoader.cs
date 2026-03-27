using System;
using System.Collections;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Infrastructure.EntryPoints;
using Project.Scripts.Infrastructure.Services.CoroutineRunner;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Project.Scripts.Infrastructure.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameUI _gameUI;

        public SceneLoader(ICoroutineRunner coroutineRunner, GameUI gameUI)
        {
            _coroutineRunner = coroutineRunner;
            _gameUI = gameUI;
        }
        
        public void Load(string name, Action onLoaded = null) => 
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        public void Load<TSceneParams>(string name, TSceneParams sceneParams, Action onLoaded = null) where TSceneParams : class, ISceneParams => 
            _coroutineRunner.StartCoroutine(LoadScene(name, sceneParams, onLoaded));

        private IEnumerator LoadScene(string name, Action onLoaded = null)
        {
            _gameUI.ShowLoadingScreen();
            
            SceneManager.LoadScene(Scenes.BOOT);
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);

            while (!waitNextScene.isDone)
                yield return null;
            
            if (name != Scenes.BOOT)
            {
                var sceneEntryPoint = Object.FindFirstObjectByType<SceneEntryPoint>();
                sceneEntryPoint.Run(_gameUI);
            }

            _gameUI.HideLoadingScreen();
            onLoaded?.Invoke();
        }
        
        private IEnumerator LoadScene<TSceneParams>(string name, TSceneParams sceneParams, Action onLoaded = null) where TSceneParams : class, ISceneParams
        {
            _gameUI.ShowLoadingScreen();
            
            SceneManager.LoadScene(Scenes.BOOT);
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);

            while (!waitNextScene.isDone)
                yield return null;

            if (name != Scenes.BOOT)
            {
                var sceneEntryPoint = Object.FindFirstObjectByType<SceneEntryPoint<TSceneParams>>();
                sceneEntryPoint.Run(_gameUI, sceneParams);
            }

            _gameUI.HideLoadingScreen();
            onLoaded?.Invoke();
        }
    }
}

    