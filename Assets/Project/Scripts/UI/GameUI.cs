using System;
using Project.Scripts.UI.Elements;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public class GameUI : MonoBehaviour, IGameUI
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Transform _sceneUI;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void ShowLoadingScreen() 
            => _loadingScreen.Show();

        public void HideLoadingScreen() 
            => _loadingScreen.Hide();

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();
            sceneUI.transform.SetParent(_sceneUI, false);
            FixUiTransform(sceneUI);
        }

        public void ClearSceneUI()
        {
            var childCount = _sceneUI.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(_sceneUI.GetChild(i).gameObject);
            }
        }

        private void FixUiTransform(GameObject uiObject)
        {
            var rt = uiObject.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            uiObject.transform.localScale = Vector3.one;
        }
    }
}
