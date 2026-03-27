using System.Collections;
using UnityEngine;

namespace Project.Scripts.UI.Elements
{
    public class LoadingScreen :  MonoBehaviour
    {
        private CanvasGroup _loadingScreen;
        [SerializeField] private float _fadeSpeed = 0.03f;
        
        private void Awake() => 
            _loadingScreen = this.gameObject.GetComponent<CanvasGroup>();

        public void Show()
        {
            gameObject.SetActive(true);
            _loadingScreen.alpha = 1;
        }

        public void Hide() => 
            StartCoroutine(FadeIn());

        private IEnumerator FadeIn()
        {
            while (_loadingScreen.alpha > 0)
            {
                _loadingScreen.alpha = Mathf.Clamp(_loadingScreen.alpha - _fadeSpeed, 0f, 1f);
                yield return new WaitForSeconds(_fadeSpeed);
            }

            gameObject.SetActive(false);
        }
    }
}