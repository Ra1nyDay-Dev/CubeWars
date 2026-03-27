using Project.Scripts.UI.Elements;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class GameUI : MonoBehaviour, IGameUI
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Transform _sceneUI;

        public void ShowLoadingScreen() => _loadingScreen.Show();
        public void HideLoadingScreen() => _loadingScreen.Hide();

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();
            sceneUI.transform.SetParent(_sceneUI, false);
            FixUiTransform(sceneUI);
        }

        private void ClearSceneUI()
        {
            var childCount = _sceneUI.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(_sceneUI.GetChild(i).gameObject);
            }
        }

        private void FixUiTransform(GameObject UIObject)
        {
            var rt = UIObject.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            UIObject.transform.localScale = Vector3.one;
        }
    }
}
