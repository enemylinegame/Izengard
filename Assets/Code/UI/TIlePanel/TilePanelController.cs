using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.Player;
using Code.TileSystem;
using Code.UI;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;

namespace Code.UI
{
    public class TilePanelController : IOnTile, ITileLoadInfo, IDisposable
    {
        private readonly TilePanel _view;
        private readonly CenterPanelController _centerPanel;
        
        public readonly TileBuildingBoardController TileMenu;
        public readonly TileMainBoardController TileMainBoard;
        public readonly TileResourcesPanelController TileResourcesPanel;
        public readonly WarsPanelController WarsPanel;
        public readonly Dictionary<BuildingConfig, Button> ButtonsInMenu;
        public readonly Dictionary<GameObject, BuildingHUD> DestroyBuildingInfo;
        
        public TilePanelController(TilePanelFactory factory, InputController inputController, CenterPanelController centerPanel)
        {
            _view = factory.GetView(factory.UIElementsConfig.BottonPanel);

            ButtonsInMenu = new Dictionary<BuildingConfig, Button>();
            DestroyBuildingInfo = new Dictionary<GameObject, BuildingHUD>();
            TileMenu = new TileBuildingBoardController(_view.TileMenu);
            TileResourcesPanel = new TileResourcesPanelController(_view.TileResourcesPanel);
            TileMainBoard = new TileMainBoardController(_view.TileUIMainBoard);
            WarsPanel = new WarsPanelController(_view.WarsPanel);

            TileMenu.StartButton += centerPanel.ActivateBuildingBuyUI;
            centerPanel.CloseBuildingsBuy += TileMenu.EnabledStartButton;
            
            inputController.Add(this);
            
            _view.gameObject.SetActive(false);
            _centerPanel = centerPanel;
        }
        public void LoadInfoToTheUI(TileView tile)
        {
            if(tile.TileModel.TileType == TileType.None) return;
            _view.gameObject.SetActive(true);
        }

        public void Cancel()
        {
            ClearButtonsUIBuy();
            _view.gameObject.SetActive(false);
            TileMainBoard.DisposeButtons();
        }
        /// <summary>
        /// Удаление блока информации построенного здания для загрузки другого тайла
        /// </summary>
        private void ClearButtonsUIBuy()
        {
            foreach (var kvp in DestroyBuildingInfo)
            {
                Object.Destroy(kvp.Key.gameObject);
            }
            DestroyBuildingInfo.Clear();
        }

        public void Deinit()
        {
            foreach (var kvp in ButtonsInMenu)
            {
                Object.Destroy(kvp.Value.gameObject);
            }

            ButtonsInMenu.Clear();
        }

        public void Dispose()
        {
            _centerPanel.CloseBuildingsBuy -= TileMenu.EnabledStartButton;
            TileMenu.StartButton -= _centerPanel.ActivateBuildingBuyUI;
        }
    }
}