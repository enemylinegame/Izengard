using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem;
using Code.BuldingsSystem.ScriptableObjects;
using Code.TileSystem;
using Code.TileSystem.Interfaces;
using Code.UI;
using Controllers;
using Interfaces;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;

namespace Code.TileSystem
{
    public class TileController : IDisposable, IOnController, IOnTile, ITileLoadInfo
    {
        private TileList _list;
        private TileUIView _uiView;
        private TileView _tileView;
        private BaseCenterText _centerText;
        private BuildingController _buildingController;
        private UIController _uiController;
        private WorkerMenager _workerMenager;
        private List<BuildingConfig> _buildingConfigs;
        
        private int _currentUnits;
        private int _maxUnits;
        public int CurrentLVL;
        public TileList List => _list;
        public BaseCenterText CenterText => _centerText;
        public WorkerMenager WorkerMenager => _workerMenager;
        public TileModel TileModel => _tileView.TileModel;
        public TileView View => _tileView;

        public TileController(TileList tileList, UIController uiController, BuildingController buildingController
            , InputController inputController)
        {
            _workerMenager = new WorkerMenager(this);
            _centerText = uiController.CenterUI.BaseCenterText;
            _list = tileList;
            _uiView = uiController.BottonUI.TileUIView;
            _uiController = uiController;
            _buildingController = buildingController;
            inputController.Add(this);
        }
        public void LoadInfoToTheUI(TileView tile)
        {
            TileTypeCheck(tile);
            _tileView = tile;
            
            if(tile.TileModel.HouseType == HouseType.None) return;

            LoadInfo(tile);
        }
        public void Cancel()
        {
            _tileView.TileModel.CurrentUnits = _currentUnits;
        }
        
        private void LoadInfo(TileView tile)
        {
            LoadBuildings(tile.TileModel);
            
            _uiView.Upgrade.onClick.AddListener(() => tile.LVLUp(this));
            UpdateInfo(tile.TileModel.TileConfig);
        }
        /// <summary>
        /// Loading information about the level, the maximum number of units and tile icons
        /// </summary>
        public void UpdateInfo(TileConfig config)
        {
            int hashCode = config.TileLvl.GetHashCode();
            _uiView.LvlText.text = $"{hashCode} LVL";
            _currentUnits = TileModel.CurrentUnits;
            _maxUnits = config.MaxUnits;
            CurrentLVL = hashCode;
            _uiView.UnitMax.text = $"{_currentUnits}/{_maxUnits} Units";
            _uiView.Icon.sprite = config.IconTile;
            _uiView.NameTile.text = TileModel.HouseType.ToString();
        }
        
         public void LoadBuildings(TileModel model)
         {
            _uiController.Deinit();
            _buildingConfigs = model.CurrBuildingConfigs;

            var buildingConfigs = _buildingConfigs.FindAll(building => building.HouseType == TileModel.HouseType);
            foreach (var building in buildingConfigs)
            {
                var button = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuyPrefabButton, _uiController.CenterUI.BuildButtonsHolder);
                _uiController.ButtonsInMenu.Add(building, button);
                CreateButtonUI(building, button);
            }
            
            foreach (var kvp in _uiController.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() => _buildingController.BuildBuilding(kvp.Key, model, this));
            }
            
            var buildings = TileModel.FloodedBuildings.FindAll(x => x.MineralConfig == null);
            foreach (var building in buildings)
            {
                var assignWorkers = _workerMenager.GetAssignedWorkers(building);
                LoadBuildingInfo(building, assignWorkers);
            }
         }
        /// <summary>
        /// Loading a saved block of information of a certain building and uploading it to the UI
        /// </summary>
        /// <returns></returns>
        public BuildingUIInfo LoadBuildingInfo(ICollectable building, int units)
        {
            var button = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuildingInfo
                , _uiController.BottonUI.BuildingMenu.ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            
            view.Icon.sprite = building.Icon;
            view.Type.text = building.BuildingTypes.ToString();
            view.BuildingType = building.BuildingTypes;

            view.UnitsBusy.text = $"{units}/5";
            view.Units = units;

            var destroyButton = view.DestroyBuildingInfo;
            _uiController.DestroyBuildingInfo.Add(button, view);
            destroyButton.onClick.AddListener(() => _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                , view, TileModel, this));

            view.PlusUnit.onClick.AddListener(() => view.Hiring(true, this, building));
            view.MinusUnit.onClick.AddListener(() => view.Hiring(false, this, building));

            return view;
        }
        
        private void CreateButtonUI(BuildingConfig buildingConfig, Button button)
        {
            if (!button.TryGetComponent(out BuildButtonView view))
            {
                Debug.LogError("Button field is empty");
                return;
            }

            view.BuildingName.text = buildingConfig.BuildingType.ToString();

            string costText = "";
            foreach (var cost in buildingConfig.BuildingCost)
            {
                costText += $"{cost.ResourceType}:{cost.Cost} ";
            }
            view.CostForBuildingsUI.text = costText;

            view.Description.text = buildingConfig.Description;
            view.Icon.sprite = buildingConfig.Icon;
        }
        
        /// <summary>
        /// Creating a new block of information for a specific building and uploading it to the UI
        /// </summary>
        /// <returns></returns>
        public BuildingUIInfo CreateBuildingInfo(BuildingConfig config, TileModel model, ICollectable building)
        {
            var button = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuildingInfo
                , _uiController.BottonUI.BuildingMenu.ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            view.Icon.sprite = config.Icon;
            view.Type.text = config.BuildingType.ToString();
            view.BuildingType = config.BuildingType;
            view.UnitsBusy.text = $"{view.Units}/5";
            
            _uiController.DestroyBuildingInfo.Add(button, view);
            
            view.DestroyBuildingInfo.onClick.AddListener((() => _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                , view, TileModel, this)));
            
            view.PlusUnit.onClick.AddListener((() => view.Hiring(true, this, building)));
            view.MinusUnit.onClick.AddListener((() => view.Hiring(false, this, building)));
            
            _uiController.IsWorkUI(UIType.Buy, false);
            
            return view;
        }
        #region Other

        /// <summary>
        /// This method is for taking a unit from Tile
        /// </summary>
        /// <param name="EightQuantity">кол юнитов</param>
        public void HiringUnits(int quantityToAdd)
        {
            if (_currentUnits  <= _maxUnits)
            {
                _currentUnits += quantityToAdd;
                _uiView.UnitMax.text = $"{_currentUnits}/{_maxUnits} Units";
            }
            else
            {
                _centerText.NotificationUI("You have hired the maximum number of units", 2000);
            }
        }


        /// <summary>
        /// Method to return a unit for hiring
        /// </summary>
        /// <param name="EightQuantity">кол юнитов</param>
        public void RemoveFromHiringUnits(int EightQuantity)
        {
            if (_currentUnits > 0)
            {
                _currentUnits -= EightQuantity;
                _uiView.UnitMax.text = $"{_currentUnits}/{_maxUnits} Units";
            }
        }

        public void Dispose()
        {
            foreach (var kvp in _uiController.ButtonsInMenu)
                kvp.Value.onClick.RemoveAllListeners();
            _uiController.BottonUI.BuildingMenu.CloseMenuButton.onClick.RemoveAllListeners();
            _uiController.Deinit();
            _uiView.Upgrade.onClick.RemoveAllListeners();
        }
        
        public void LevelCheck()
        {
            bool levelExceedDestroyBuildingInfo = CurrentLVL > _uiController.DestroyBuildingInfo.Count;
            _uiController.BottonUI.BuildingMenu.PrefabButtonClear.gameObject.SetActive(levelExceedDestroyBuildingInfo);
        }

        private void TileTypeCheck(TileView view)
        {
            if(view.TileModel.HouseType != HouseType.None) return;
            
            _uiController.IsWorkUI(UIType.TileSel, true);
            
            _uiController.CenterUI.TIleSelection.TileEco.onClick.AddListener(() => TileType(HouseType.Eco, view));
            _uiController.CenterUI.TIleSelection.TileWar.onClick.AddListener(() => TileType(HouseType.war, view));
        }


        private void TileType(HouseType type, TileView tile)
        {
            tile.TileModel.HouseType = type;
            
            _uiController.CenterUI.TIleSelection.TileEco.onClick.RemoveAllListeners();
            _uiController.CenterUI.TIleSelection.TileWar.onClick.RemoveAllListeners();

            _uiController.IsWorkUI(UIType.TileSel, false);
            _uiController.IsWorkUI(UIType.Tile, true);
            LoadInfo(tile);
        }

        
        #endregion
    }
}