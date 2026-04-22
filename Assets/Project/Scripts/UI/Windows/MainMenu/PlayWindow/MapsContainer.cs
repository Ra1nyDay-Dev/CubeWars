using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Infrastructure.Services.AssetManagement;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI.Windows.MainMenu.PlayWindow
{
    public class MapsContainer : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        
        public MapCard Current { get;  private set; }
        
        private const string MAP_CARD_PATH = "UI/MapCard";
        
        private IConfigProvider _configProvider;
        private IAssetProvider _assetProvider;
        private IInstantiator _instantiator;
        
        private readonly List<MapCard> _mapCards = new();
        private List<LevelConfig> _mapsConfigs;

        [Inject]
        public void Construct(
            IConfigProvider configProvider,
            IAssetProvider assetProvider,
            IInstantiator instantiator)
        {
            _configProvider = configProvider;
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }
        
        public void Initialize()
        {
            GetMapsConfigs();
            RefreshMaps();
            SubscribeUpdates();
            Select(_mapCards.First());
        }

        private void OnDestroy() => 
            UnsubscribeUpdates();

        private void SubscribeUpdates()
        {
            foreach (MapCard mapCard in _mapCards)
                mapCard.Clicked += OnMapCardClicked;
        }

        private void UnsubscribeUpdates()
        {
            foreach (MapCard mapCard in _mapCards)
                mapCard.Clicked -= OnMapCardClicked;
        }

        private void OnMapCardClicked(MapCard mapCard)
        {
            if (Current == mapCard)
                return;

            Select(mapCard);
        }

        private void Select(MapCard mapCard)
        {
            foreach (MapCard card in _mapCards)
                card.SetSelected(card == mapCard);
            
            Current = mapCard;
        }

        private void GetMapsConfigs() => 
            _mapsConfigs = _configProvider.GetAllLevelsConfigs();

        private void RefreshMaps()
        {
            ClearMapsContainer();
            FillMapsContainer();
        }

        private void FillMapsContainer()
        {
            foreach (LevelConfig mapConfig in _mapsConfigs)
            {
                GameObject mapCardPrefab = _assetProvider.LoadAsset(MAP_CARD_PATH);
                MapCard mapCard = _instantiator.InstantiatePrefabForComponent<MapCard>(mapCardPrefab, _content);
                mapCard.Construct(mapConfig);
                _mapCards.Add(mapCard);
            }
        }

        private void ClearMapsContainer()
        {
            foreach (MapCard mapCard in _mapCards)
                Destroy(mapCard.gameObject);
            
            _mapCards.Clear();
        }
    }
}