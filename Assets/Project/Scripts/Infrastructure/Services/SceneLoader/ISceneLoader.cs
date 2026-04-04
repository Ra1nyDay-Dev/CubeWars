using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Infrastructure.Services.ServiceLocator;

namespace Project.Scripts.Infrastructure.Services.SceneLoader
{
    public interface ISceneLoader : IProjectService
    {
        UniTask Load(string name, Action onLoaded = null);
        UniTask Load<TSceneParams>(string name, TSceneParams sceneParams, Action onLoaded = null) 
            where TSceneParams : class, ISceneParams;
    }
}