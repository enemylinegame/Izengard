using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.Player;
using Code.TileSystem;
using Code.UI.CenterPanel;
using CombatSystem.Views;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;

namespace Code.UI
{
    public class TileUIController : IOnTile, ITileLoadInfo, IDisposable
    {
        private readonly CenterPanelController _centerPanel;
        private TileUI _view;
        public TileUIInfoView TileMenu => _view.TileMenu;
        public TileUIView TileUIView => _view.TileUIView;
        public ResourcesLayoutUIView ResourcesLayoutUIView => _view.ResourcesLayoutUIView;
        public WarsUIView WarsUIView => _view.WarsUIView;
        
        public List<BuildingConfig> ButtonsBuy = new List<BuildingConfig>();
        public Dictionary<BuildingConfig, Button> ButtonsInMenu = new Dictionary<BuildingConfig, Button>();
        public Dictionary<GameObject, BuildingHUD> DestroyBuildingInfo = new Dictionary<GameObject, BuildingHUD>();
        
        public TileUIController(TileUIFactory factory, InputController inputController, CenterPanelController centerPanel)
        {
            _centerPanel = centerPanel;
            _view = factory.GetView(factory.UIElementsConfig.BottonPanel);
            _view.gameObject.SetActive(false);
            TileMenu.PrefabButtonClear.onClick.AddListener((() =>
            {
                centerPanel.ActivateBuildingBuyUI();
                TileMenu.PrefabButtonClear.gameObject.SetActive(false);
            }));
            inputController.Add(this);
            centerPanel.CloseBuildingsBuy += ActivateButtonByHouse;
        }

        private void ActivateButtonByHouse()
        {
            TileMenu.PrefabButtonClear.gameObject.SetActive(true);
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
            TileUIView.ButtonsHolder.ForEach(x => x.Button.onClick.RemoveAllListeners());
        }
        /// <summary>
        /// Удаление блока информации построенного здания для загрузки другого тайла
        /// </summary>
        private void ClearButtonsUIBuy()
        {
            foreach (var kvp in DestroyBuildingInfo)
            {
                GameObject.Destroy(kvp.Key.gameObject);
            }
            DestroyBuildingInfo.Clear();
            ButtonsBuy.Clear();
        }

        public void Deinit()
        {
            foreach (var kvp in ButtonsInMenu)
            {
                GameObject.Destroy(kvp.Value.gameObject);
            }
            ButtonsInMenu.Clear();
        }

        public void Dispose()
        {
            _centerPanel.CloseBuildingsBuy += ActivateButtonByHouse;
        }
    }
}