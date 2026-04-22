using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Windows.MainMenu.NavBar
{
    [RequireComponent(typeof(Button))]
    public class NavButton : MonoBehaviour
    {
        [field: SerializeField] public WindowId WindowId { get; private set; }

        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _buttonText;
        
        [SerializeField] private Color _selectedBackgroundColor;
        [SerializeField] private Color _selectedTextColor;

        public event Action<WindowId> Clicked;
        
        private Button _button;
        private Color _defaultTextColor;
        private Color _defaultBackgroundColor;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => Clicked?.Invoke(WindowId));
            
            if (_buttonText != null)
                _defaultTextColor = _buttonText.color;
            
            if (_background != null)
                _defaultBackgroundColor = _background.color;
        }
        

        public void SetSelected(bool selected)
        {
            if (_buttonText != null)
                _buttonText.color = selected ? _selectedTextColor : _defaultTextColor;
            
            if (_background != null)
                _background.color = selected ? _selectedBackgroundColor : _defaultBackgroundColor;
        }
    }
}