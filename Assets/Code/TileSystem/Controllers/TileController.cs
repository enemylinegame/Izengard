using System;
using System.Collections.Generic;
using Code.BuildingSystem;
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
    public class TileController : IDisposable, IOnController, IOnUpdate, IOnTile, ITileLoadInfo
    {
        private TileList _list;
        private TileUIView _uiView;
        private TileView _tileView;
        private BaseCenterText _centerText;
        private BuildingController _buildingController;
        private UIController _uiController;
        private WorkerAssignmentsController _workerAssignmentsController;
        private List<BuildingConfig> _buildingConfigs;
        
        private int _currentUnits;
        private int _maxUnits;
        public int CurrentLVL;
        public TileList List => _list;
        public BaseCenterText CenterText => _centerText;
        public WorkerAssignmentsController WorkerAssignmentsController => _workerAssignmentsController;
        public TileModel TileModel => _tileView.TileModel;
        public TileView View => _tileView;

        public TileController(TileList tileList, UIController uiController
            , BuildingController buildingController
            , InputController inputController, WorkersTeamController workersTeam)
        {
            _workerAssignmentsController = 
                new WorkerAssignmentsController(this, workersTeam);

            _centerText = uiController.CenterUI.BaseCenterText;
            _list = tileList;
            _uiView = uiController.BottonUI.TileUIView;
            _uiController = uiController;
            _buildingController = buildingController;
            inputController.Add(this);
        }
        public void LoadInfoToTheUI(TileView tile)
        {
            _tileView = tile;
            ADDBuildUI(tile.TileModel);
            LoadButtonsUIBuy();
            _uiView.Upgrade.onClick.AddListener(() => tile.LVLUp(this));
            UpdateInfo(tile.TileModel.TileConfig);
        }
        public void Cancel()
        {
            _tileView.TileModel.CurrentUnits = _currentUnits;
        }
        /// <summary>
        /// Loading information about the level, the maximum number of units and tile icons
        /// </summary>
        public void UpdateInfo(TileConfig config)
        {
            _uiView.LvlText.text = config.TileLvl.GetHashCode().ToString() + " LVL";
            _currentUnits = TileModel.CurrentUnits;
            _maxUnits = config.MaxUnits;
            CurrentLVL = config.TileLvl.GetHashCode();
            _uiView.UnitMax.text = _currentUnits + "/" + config.MaxUnits + " Units";
            _uiView.Icon.sprite = config.IconTile;
        }
        
         public void ADDBuildUI(TileModel model)
        {
            _uiController.Deinit();
            _buildingConfigs = model.CurrBuildingConfigs;
            Init(_buildingConfigs);

            foreach (var kvp in _uiController.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() => _buildingController.BuildBuilding(kvp.Key, model, this));
            }
        }
         
        public void LoadButtonsUIBuy()
        {
            _workerAssignmentsController.FillWorkerList();
            foreach (var building in TileModel.FloodedBuildings)
            {
                if (building.MineralConfig == null)
                {
                    LoadBuildingInfo(building, _workerAssignmentsController.GetAssignedWorkers(building));
                }
            }
        }

         /// <summary>
        /// Creating buttons from the building config
        /// </summary>
        public void Init(List<BuildingConfig> models)
        {
            foreach (var building in models)
            {
                var button = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuyPrefabButton, _uiController.CenterUI.BuildButtonsHolder);
                _uiController.ButtonsInMenu.Add(building, button);
                CreateButtonUI(building, button);
            }
        }
        /// <summary>
        /// Loading a saved block of information of a certain building and uploading it to the UI
        /// </summary>
        /// <returns></returns>
        public BuildingUIInfo LoadBuildingInfo(Building building, int Units)
        {
            var button = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuildingInfo, _uiController.BottonUI.BuildingMenu.ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            view.Icon.sprite = building.Icon.sprite;
            view.Type.text = building.BuildingTypes.ToString();
            view.Types = building.BuildingTypes;
            view.UnitsBusy.text = Units +"/5";
            view.Units = Units;
            _uiController.DestroyBuildingInfo.Add(button, view);

            view.DestroyBuildingInfo.onClick.AddListener((() => 
                _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                , view, TileModel, _workerAssignmentsController)));

            view.PlusUnit.onClick.AddListener((() => view.Hiring(true, this, building)));
            view.MinusUnit.onClick.AddListener((() => view.Hiring(false, this, building)));
            return view;
        }
        
        private void CreateButtonUI(BuildingConfig buildingConfig, Button button)
        {
            var view = button.GetComponent<BuildButtonView>();
            if (view)
            {
                view.BuildingName.text = buildingConfig.BuildingType.ToString();
                foreach (var cost in buildingConfig.BuildingCost)
                {
                    view.CostForBuildingsUI.text += cost.ResourceType + ":" + cost.Cost + " ";
                }
                view.Description.text = buildingConfig.Description;
                view.Icon.sprite = buildingConfig.Icon;
            }
            else
            {
                Debug.LogError("Button field is empty");
            }
        }
        
        /// <summary>
        /// Creating a new block of information for a specific building and uploading it to the UI
        /// </summary>
        /// <returns></returns>
        public BuildingUIInfo CreateBuildingInfo(BuildingConfig config, TileModel model, Building building)
        {
            var button = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuildingInfo, _uiController.BottonUI.BuildingMenu.ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            
            view.Icon.sprite = config.Icon;
            view.Type.text = config.BuildingType.ToString();
            view.Types = config.BuildingType;
            view.UnitsBusy.text = view.Units +"/5";
            _uiController.DestroyBuildingInfo.Add(button, view);
            
            view.DestroyBuildingInfo.onClick.AddListener((() => 
                _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                , view, TileModel, _workerAssignmentsController)));

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
        public void hiringUnits(int EightQuantity)
        {
            if (EightQuantity <= _maxUnits)
            {
                _currentUnits += EightQuantity;
                _uiView.UnitMax.text = _currentUnits.ToString() + "/" + _maxUnits.ToString() + " Units";
            }
            else
            {
                _centerText.NotificationUI("you have hired the maximum number of units", 2000);
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
                _uiView.UnitMax.text = _currentUnits.ToString() + "/" + _maxUnits.ToString() + " Units";
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
            if (CurrentLVL > _uiController.DestroyBuildingInfo.Count)
            {
                _uiController.BottonUI.BuildingMenu.PrefabButtonClear.gameObject.SetActive(true);
            }
            else
            {
                _uiController.BottonUI.BuildingMenu.PrefabButtonClear.gameObject.SetActive(false);
            }
        }


        public void OnUpdate(float deltaTime)
        {
            LevelCheck();
        }

        #endregion
    }
}