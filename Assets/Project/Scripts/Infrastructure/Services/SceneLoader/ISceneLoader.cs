using System;
using Cysharp.Threading.Tasks;

namespace Project.Scripts.Infrastructure.Services.SceneLoader
{
    public interface ISceneLoader
    {
        UniTask Load(string sceneName, Action onLoaded = null);
        UniTask Load<TSceneParams>(string sceneName, TSceneParams sceneParams, Action onLoaded = null) 
            where TSceneParams : class, ISceneParams;
    }
}