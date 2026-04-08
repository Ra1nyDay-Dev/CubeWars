using UnityEngine;

namespace Project.Scripts.UI
{
    public interface IGameUI
    {
        void AttachSceneUI(GameObject sceneUI);
        void ClearSceneUI();
        void ShowLoadingScreen();
        void HideLoadingScreen();
    }
}