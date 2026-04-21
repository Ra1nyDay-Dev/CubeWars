using Project.Scripts.UI.Windows;

namespace Project.Scripts.UI
{
    public class MainMenuUI : SceneUI
    {
        public override void Initialize()
        {
            base.Initialize();
            _windowFactory.CreateWindow(WindowId.MainMenuNavBar);
        }
    }
}