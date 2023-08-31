using System;
using Code.BuildingSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.UI
{
    public class TileBuildingBoardController
    {
        private readonly TileUIBuildingBoard _view;

        public event Action StartButton;

        public TileBuildingBoardController(TileUIBuildingBoard view)
        {
            _view = view;
            _view.StartButton.onClick.AddListener((() => StartButton?.Invoke()));
            _view.StartButton.onClick.AddListener((() => _view.StartButton.gameObject.SetActive(false)));
        }

        public void EnabledStartButton(bool isActive)
        {
            _view.StartButton.gameObject.SetActive(isActive);
        }
        public void Dispose()
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