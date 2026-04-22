using System;
using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Windows.MainMenu.PlayWindow
{
    [RequireComponent(typeof(Button))]
    public class MapCard : MonoBehaviour
    {
        [SerializeField] private Image _mapImage;
        [SerializeField] private Image _cardBackgroundImage;
        [SerializeField] private TMP_Text _cardCaption;
        
        [SerializeField] private Color _selectedCaptionColor;
        [SerializeField] private Color _selectedBackgroundColor;

        public string MapName {get; private set;}
        public string SceneName {get; private set;}
        
        public event Action<MapCard> Clicked;

        private Button _button;
        private Color _defaultCaptionColor;
        private Color _defaultBackgroundColor;

        public void Construct(LevelConfig config)
        {
            MapName = config.MapName;
            SceneName = config.SceneName;

            _cardCaption.text = MapName;
            _mapImage.sprite = config.MapImage;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button?.onClick.AddListener(() => Clicked?.Invoke(this));
            
            if (_cardCaption != null)
                _defaultCaptionColor = _cardCaption.color;
            
            if (_cardBackgroundImage != null)
                _defaultBackgroundColor = _cardBackgroundImage.color;
        }
        
        public void SetSelected(bool selected)
        {
            if (_cardCaption != null)
                _cardCaption.color = selected ? _selectedCaptionColor : _defaultCaptionColor;
            
            if (_cardBackgroundImage != null)
                _cardBackgroundImage.color = selected ? _selectedBackgroundColor : _defaultBackgroundColor;
        }
    }
}