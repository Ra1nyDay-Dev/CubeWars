using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public abstract class SceneUI : MonoBehaviour, ISceneUI, IInitializable
    {
        public GameObject Root => gameObject;
        
        private IGameUI _gameUI;

        [Inject]
        public void Construct(IGameUI gameUI) => 
            _gameUI = gameUI;

        public void Initialize() => 
            _gameUI.AttachSceneUI(gameObject);
    }
}