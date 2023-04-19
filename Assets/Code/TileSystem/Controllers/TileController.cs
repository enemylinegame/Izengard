using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.TileSystem;
using Code.UI;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;

namespace Code.TileSystem
{
    public class TileController : IDisposable, IOnController, IOnUpdate
    {
        private TileList _list;
        private TileView _view;
        private TileUIView _uiView;
        private BaseCenterText _centerText;
        private BuildGenerator _generator;
        private GlobalResourceStock _stock;
        private List<BuildingConfig> _buildingConfigs;
        private BuildBuildings _buildBuildings;
        private BuildingController _buildingController;
        private UIController _uiController;
        private int _currentlvl;
        private int _eightQuantity;
        private int _units;

        public TileConfig Config;
        public TileList List => _list;
        public BaseCenterText CenterText => _centerText;
        public int CurrentLVL => _currentlvl;
        public TileView View => _view;

        public TileController(TileList tileList, TileUIView uiView, BaseCenterText centerText, UIController uiController, 
            BuildGenerator buildGenerator, GlobalResourceStock stock, BuildingController buildingController)
        {
            _centerText = centerText;
            _list = tileList;
            _uiView = uiView;
            _generator = buildGenerator;
            _stock = stock;
            _uiController = uiController;
            
            _stock.GlobalResStock.HoldersInStock.Find(x => x.ObjectInHolder.ResourceType == ResourceType.Wood)
                .CurrentValue = 100;
            _buildingController = buildingController;
        }

        /// <summary>
        /// Загрузка информации о тайле
        /// </summary>
        public void LoadInfo(TileView view)
        {
            _uiView.Upgrade.onClick.RemoveAllListeners();
            _view = view;
            ADDBuildUI(view.CurrBuildingConfigs, view);
            view.LoadButtonsUIBuy(this, _uiController);
            _uiView.Upgrade.onClick.AddListener(() => view.LVLUp(this));
            UpdateInfo(view.TileConfig);
        }
        /// <summary>
        /// Загрузка всей информации на тайл
        /// </summary>
        public void UpdateInfo(TileConfig config)
        {
            _uiView.LvlText.text = config.TileLvl.GetHashCode().ToString() + " LVL";
            _eightQuantity = _view.EightQuantity;
            _units = config.MaxUnits;
            _currentlvl = config.TileLvl.GetHashCode();
            _uiView.UnitMax.text = _eightQuantity + "/"+ config.MaxUnits + " Units";
            _uiView.Icon.sprite = config.IconTile;
        }
        public void ADDBuildUI(List<BuildingConfig> configs, TileView view)
        {
            _uiController.Deinit();
            _buildingConfigs = configs;
            UpdateBuildings(view);
        }
        
        public void UpdateBuildings(TileView view)
        {
            _uiController.Init(_buildingConfigs);
            
            foreach (var kvp in _uiController.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() => BuildBuilding(kvp.Key, view));
            }
        }
        /// <summary>
        /// Проверяет на наличие ресурса если он есть ставим здание.
        /// </summary>
        private void BuildBuilding(BuildingConfig buildingConfig, TileView view)
        {
            if (!IsResourcesEnough(buildingConfig))
            {
                return;
            }
            
            foreach (var resourcePrice in buildingConfig.BuildingCost)
            {
                var resourceHolder = _stock.GlobalResStock.HoldersInStock.Find(x => 
                    x.ObjectInHolder.ResourceType == resourcePrice.ResourceType);
                resourceHolder.CurrentValue -= resourcePrice.Cost;
            }
            var building = _buildingController.StartBuilding(view, buildingConfig);
            if (building)
            {
                var info = _uiController.CreateBuildingInfo(buildingConfig, this, building);
                building.Icon = info.Icon;
                building.Type = info.Types;
                _uiController.ButtonsBuy.Add(buildingConfig);
                view.FloodedBuildings.Add(building, buildingConfig);
            }
        }
        
        public void DestroyBuilding(Dictionary<Building, BuildingConfig> buildingConfigs, BuildingUIInfo Button, TileView view)
        {
            foreach (var kvp in buildingConfigs)
            {
                if (kvp.Value.BuildingType == Button.Types)
                {
                    Button.DestroyBuildingInfo.onClick.RemoveAllListeners();
                    Button.PlusUnit.onClick.RemoveAllListeners();
                    Button.MinusUnit.onClick.RemoveAllListeners();
                    buildingConfigs.Remove(kvp.Key);
                    _uiController.DestroyBuildingInfo.Remove(Button.gameObject);
                    view.FloodedBuildings.Remove(kvp.Key);
                    _buildingController.RemoveTypeDots(view, kvp.Key);
                    GameObject.Destroy(kvp.Key.gameObject);
                    GameObject.Destroy(Button.gameObject);
                    break;
                }
                
            }
        }
        
        private bool IsResourcesEnough(BuildingConfig buildingConfig)
        {
            foreach (ResourcePriceModel resourcePriceModel in buildingConfig.BuildingCost)
            {
                if (_stock.GlobalResStock.GetResursesCount(resourcePriceModel.ResourceType) < resourcePriceModel.Cost)
                {
                    _centerText.NotificationUI("you do not have enough resources to buy", 1000);
                    return false;
                }
            }
            return true;
        }
        #region Other

        /// <summary>
        /// Этот метод для того чтобы взять юнита из тайла
        /// </summary>
        /// <param name="EightQuantity">кол юнитов</param>
        public void hiringUnits(int EightQuantity)
        {
            if (EightQuantity <= _units)
            {
                _eightQuantity += EightQuantity;
                _uiView.UnitMax.text = _eightQuantity.ToString() + "/"+ _units.ToString() + " Units";
            }
            else
            {
                _centerText.NotificationUI("you have hired the maximum number of units", 2000);
            }
            
        }

        /// <summary>
        /// Метод для того чтобы вернуть юнита для найма
        /// </summary>
        /// <param name="EightQuantity">кол юнитов</param>
        public void RemoveFromHiringUnits(int EightQuantity)
        {
            if (_eightQuantity > 0)
            {
                _eightQuantity -= EightQuantity;
                _uiView.UnitMax.text = _eightQuantity.ToString() + "/"+ _units.ToString() + " Units";
            }
        }

        public void Dispose()
        {
            foreach (var kvp in _uiController.ButtonsInMenu)
                kvp.Value.onClick.RemoveAllListeners();
            _uiController.BuildingsUIView.CloseMenuButton.onClick.RemoveAllListeners();
            _uiController.Deinit();
            _uiView.Upgrade.onClick.RemoveAllListeners();
        }
        public void LevelCheck()
        {
            if (CurrentLVL > _uiController.DestroyBuildingInfo.Count)
            {
                _uiController.BuildingsUIView.PrefabButtonClear.gameObject.SetActive(true);
            }
            else
            {
                _uiController.BuildingsUIView.PrefabButtonClear.gameObject.SetActive(false);
            }
        }
        public void OnUpdate(float deltaTime)
        {
            LevelCheck();
        }

        #endregion
    }
}