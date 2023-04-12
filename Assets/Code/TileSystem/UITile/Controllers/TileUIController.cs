using System;
using System.Collections.Generic;
using Code.TileSystem;
using Controllers.BuildBuildingsUI;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;

namespace Code.TileSystem
{
    public class TileUIController : IDisposable, IOnController, IOnUpdate
    {
        private TileList _list;
        private TileView _view;
        private TileUIView _uiView;
        private BaseCenterText _centerText;
        private BuildingsUIView _buildingsUIView;
        private BuildGenerator _generator;
        private GlobalResourceStock _stock;
        private List<BuildingConfig> _buildingConfigs;
        private Dictionary<GameObject, BuildingConfig> _destroyBuilding = new Dictionary<GameObject, BuildingConfig>();
        private BuildBuildings _buildBuildings;
        private int _currentlvl;
        private int _eightQuantity;
        private int _units;

        public TileConfig Config;
        public TileList List => _list;
        public BaseCenterText CenterText => _centerText;
        public int CurrentLVL => _currentlvl;
        public TileView View => _view;
        public BuildingsUIView BuildingsUIView => _buildingsUIView;
        public Dictionary<GameObject, BuildingConfig> DestroyBuilding => _destroyBuilding;

        public TileUIController(TileList tileList, TileUIView uiView, BaseCenterText centerText, BuildingsUIView buildingsUIView, 
            BuildGenerator buildGenerator, GlobalResourceStock stock)
        {
            _centerText = centerText;
            _list = tileList;
            _uiView = uiView;
            _generator = buildGenerator;
            _stock = stock;
            _buildingsUIView = buildingsUIView;
            
            _stock.GlobalResStock.HoldersInStock.Find(x => x.ObjectInHolder.ResourceType == ResourceType.Wood)
                .CurrentValue = 100;
            
            OpenMenu(false);
        }

        /// <summary>
        /// Загрузка информации о тайле
        /// </summary>
        public void LoadInfo(TileView view)
        {
            _uiView.Upgrade.onClick.RemoveAllListeners();
            _view = view;
            view.CreateButtonsUIBuy(this);
            _uiView.Upgrade.onClick.AddListener(() => view.LVLUp(this));
            UpdateInfo(view.TileConfig);
            ADDBuildUI(view.CurrBuildingConfigs, view);
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
            _buildingsUIView.Deinit();
            _buildingConfigs = configs;
            UpdateBuildings(view);
            foreach (var kvp in _buildingsUIView.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener((() => _view.CreateButtonsUIBuy(this)));
            }
        }
        
        public void UpdateBuildings(TileView view)
        {
            _buildingsUIView.Init(_buildingConfigs);
            
            foreach (var kvp in _buildingsUIView.ButtonsInMenu)
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

            var building = _generator.StartBuildingHouses(buildingConfig);
            _destroyBuilding.Add(building.gameObject, buildingConfig);
            _buildingsUIView.ButtonsBuy.Add(buildingConfig);
            view.FloodedBuildings.Add(building);
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
        
        public void LevelCheck()
        {
            if (CurrentLVL > _buildingsUIView.DestroyBuildingInfo.Count)
            {
                _buildingsUIView.PrefabButtonClear.gameObject.SetActive(true);
            }
            else
            {
                _buildingsUIView.PrefabButtonClear.gameObject.SetActive(false);
            }
        }

        private void OnOpenMenuButton() => OpenMenu(true);

        private void OnCloseMenuButton() => OpenMenu(false);

        public void OpenMenu(bool isOpen)
        {
            foreach (var window in _buildingsUIView.Windows) window.gameObject.SetActive(isOpen);
            _buildingsUIView.CloseMenuButton.gameObject.SetActive(isOpen);
            if (isOpen == false)
            {
                _buildingsUIView.ClearButtonsUIBuy();
            }
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
            foreach (var kvp in _buildingsUIView.ButtonsInMenu)
                kvp.Value.onClick.RemoveAllListeners();
            _buildingsUIView.CloseMenuButton.onClick.RemoveAllListeners();
            _buildingsUIView.Deinit();
            _uiView.Upgrade.onClick.RemoveAllListeners();
        }

        public void OnUpdate(float deltaTime)
        {
            LevelCheck();
        }

        #endregion
    }
}