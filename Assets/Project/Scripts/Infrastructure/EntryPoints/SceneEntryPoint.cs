using Project.Scripts.Infrastructure.Services.ServiceLocator;
using Project.Scripts.UI;
using UnityEngine;

namespace Project.Scripts.Infrastructure.EntryPoints
{
    public abstract class SceneEntryPoint : MonoBehaviour, ISceneEntryPoint
    {
        [SerializeField] protected GameObject _sceneUIPrefab;
        
        protected GameObject _sceneUI;
        
        public virtual void Run(GameUI gameUI)
        {
            SceneServices.Dispose();
            _sceneUI = Instantiate(_sceneUIPrefab);
            gameUI.AttachSceneUI(_sceneUI.gameObject);
        }
    }
    
    public abstract class SceneEntryPoint<TSceneParams> : SceneEntryPoint, ISceneEntryPointWithParams<TSceneParams>
    {
        protected TSceneParams _sceneParams;
        
        public virtual void Run(GameUI gameUI, TSceneParams sceneParams)
        {
            base.Run(gameUI);
            _sceneParams = sceneParams;
        }
    }
}