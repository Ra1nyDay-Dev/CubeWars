using System;
using Project.Scripts.Infrastructure.Services.ServiceLocator;

namespace Project.Scripts.Infrastructure.Services.SceneLoader
{
    public interface ISceneLoader : IProjectService
    {
        void Load(string name, Action onLoaded = null);
        void Load<TSceneParams>(string name, TSceneParams sceneParams, Action onLoaded = null) 
            where TSceneParams : class, ISceneParams;
    }
}