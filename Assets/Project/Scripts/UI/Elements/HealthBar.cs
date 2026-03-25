using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Elements
{
    public class HealthBar : MonoBehaviour, IHealthView
    {
        [SerializeField] private Image _imageCurrent;
        
        public void SetValue(float current, float max) =>
            _imageCurrent.fillAmount = current / max;
    }
}