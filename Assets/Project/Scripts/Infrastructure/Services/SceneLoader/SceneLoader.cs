using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Infrastructure.EntryPoints;
using Project.Scripts.UI;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Project.Scripts.Infrastructure.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly GameUI _gameUI;

        public SceneLoader(GameUI gameUI) => 
            _gameUI = gameUI;

        public UniTask Load(string name, Action onLoaded = null) => 
            LoadScene(name, RunSceneEntryPoint, onLoaded);

        public UniTask Load<TSceneParams>(
            string name,
            TSceneParams sceneParams,
            Action onLoaded = null
        ) where TSceneParams : class, ISceneParams => 
            LoadScene(name, () => RunSceneEntryPoint(sceneParams), onLoaded);

        private async UniTask LoadScene(string name, Action runEntryPoint, Action onLoaded = null)
        {
            _gameUI.ShowLoadingScreen();
            
            SceneManager.LoadScene(Scenes.BOOT);
            await SceneManager.LoadSceneAsync(name).ToUniTask();
            
            if (name != Scenes.BOOT) 
                runEntryPoint?.Invoke();

            _gameUI.HideLoadingScreen();
            onLoaded?.Invoke();
        }
        
        private void RunSceneEntryPoint()
        {
            var entry = Object.FindFirstObjectByType<SceneEntryPoint>();
            entry.Run(_gameUI);
        }
        
        private void RunSceneEntryPoint<TSceneParams>(TSceneParams sceneParams)
        {
            var entry = Object.FindFirstObjectByType<SceneEntryPoint<TSceneParams>>();
            entry.Run(_gameUI, sceneParams);
        }
    }
}

    