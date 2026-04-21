using Project.Scripts.UI.Windows;
using UnityEngine;

namespace Project.Scripts.UI.Services.WindowFactory
{
    public interface IWindowFactory
    {
        void AttachSceneUI(SceneUI sceneUI);
        void ClearSceneUI();
        BaseWindow CreateWindow(WindowId windowId, WindowType windowType = WindowType.Scene);
    }
}