using System;
using Code.BuildingSystem;
using Code.TileSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.UI
{
    public class CenterPanelController
    {
        public readonly NotificationPanelController NotificationPanel;
        private CenterPanel _view;
        private TileSelectionView _tileSelection => _view.TIleSelection;
        public event Action<bool> CloseBuildingsBuy;
        
        public CenterPanelController(CenterPanelFactory factory)
        {
            _view = factory.GetView(factory.UIElementsConfig.CenterPanel);

            NotificationPanel = new NotificationPanelController(_view.NotificationPanel);
            
            _view.CloseBuildingsBuy.onClick.AddListener((() =>
            {
                _view.BuildingBuy.gameObject.SetActive(false);
                CloseBuildingsBuy?.Invoke(true);
            }));
        }

        public void ActivateBuildingBuyUI()
        {
            _view.BuildingBuy.gameObject.SetActive(true);
            
        }
        public void DeactivateBuildingBuyUI()
        {
            _view.BuildingBuy.gameObject.SetActive(false);
            CloseBuildingsBuy?.Invoke(true);
            
        }
        

        public void ActivateTileTypeSelection(UnityAction actionEco, UnityAction actionWar)
        {
            _tileSelection.gameObject.SetActive(true);
            _tileSelection.TileEco.onClick.AddListener(actionEco);
            _tileSelection.TileWar.onClick.AddListener(actionWar);
            _tileSelection.Back.onClick.AddListener(DeactivateTileTypeSelection);
            
        }
        
        public void DeactivateTileTypeSelection()
        {
            _tileSelection.gameObject.SetActive(false);
            _tileSelection.TileEco.onClick.RemoveAllListeners();
            _tileSelection.TileWar.onClick.RemoveAllListeners();
            _tileSelection.Back.onClick.RemoveAllListeners();
            
        }

        public Transform TransformBuildButtonsHolder()
        {
            return _view.BuildButtonsHolder;
        }
    }
}