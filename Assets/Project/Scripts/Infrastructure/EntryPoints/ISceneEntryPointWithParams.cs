using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.UI;

namespace Project.Scripts.Infrastructure.EntryPoints
{
    public interface ISceneEntryPointWithParams<TSceneParams> : ISceneParams
    {
        void Run(GameUI gameUI, TSceneParams sceneParams);
    }
}