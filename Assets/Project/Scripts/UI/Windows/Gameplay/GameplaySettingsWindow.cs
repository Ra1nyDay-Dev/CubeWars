using Project.Scripts.Infrastructure.Data;
using Project.Scripts.Infrastructure.Services.SceneLoader;
using Project.Scripts.UI.Services.WindowService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Windows.Gameplay
{
    public class GameplaySettingsWindow : BaseWindow
    {
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Button _closeButton;
        
        private ISceneLoader _sceneLoader;
        private IWindowService _windowService;

        [Inject]
        public void Construct(
            ISceneLoader sceneLoader, 
            IWindowService windowService)
        {
            _sceneLoader = sceneLoader;
            _windowService = windowService;
        }

        protected override void Initialize() => 
            Id = WindowId.GameplaySetting;

        protected override void SubscribeUpdates()
        {
            _backToMenuButton.onClick.AddListener(
                () => _sceneLoader.Load(Scenes.MAIN_MENU));
            
            _closeButton.onClick.AddListener(() => _windowService.Close(Id));
        }
    }
}