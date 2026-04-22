using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.UI.Services.WindowService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Windows.MainMenu.PlayWindow
{
    public class PlayWindow : BaseWindow
    {
        [SerializeField] private MapsContainer _mapsContainer;
        [SerializeField] private Button _playButton;
        
        private ISceneLoader _sceneLoader;

        [Inject]
        public void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        protected override void Initialize()
        {
            Id = WindowId.MainMenuPlay;
            _mapsContainer.Initialize();
        }
        
        protected override void SubscribeUpdates()
        {
            _playButton.onClick.AddListener(
                () => _sceneLoader.Load(_mapsContainer.Current.SceneName));
        }
    }
}