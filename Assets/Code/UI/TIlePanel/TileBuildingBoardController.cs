using Code.BuildingSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.UI
{
    public class TileBuildingBoardController
    {
        private readonly TileUIBuildingBoard _view;

        public TileBuildingBoardController(TileUIBuildingBoard view)
        {
            _view = view;
        }

        public void EnabledStartButton(bool isActive)
        {
            _view.StartButton.gameObject.SetActive(isActive);
        }

        public void SubscribeCloseMenuButton(UnityAction action)
        {
            _view.CloseMenuButton.onClick.AddListener(action);
        }
        
        public void UnSubscribeCloseMenuButton()
        {
            _view.CloseMenuButton.onClick.RemoveAllListeners();
        }

        public void SubscribeStartButton(UnityAction action)
        {
            _view.StartButton.onClick.AddListener(action);
            _view.StartButton.onClick.AddListener((() => _view.StartButton.gameObject.SetActive(false)));
        }
        public void UnSubscribeStartButton()
        {
            _view.StartButton.onClick.RemoveAllListeners();               
        }

        public GameObject GetPrefabBuildingInfo()
        {
            return _view.BuildingInfo;
        }
        public Button GetBuyPrefabButton()
        {
            return _view.BuyPrefabButton;
        }

        public Transform GetByBuildButtonsHolder()
        {
            return _view.ByBuildButtonsHolder;
        }
    }
}