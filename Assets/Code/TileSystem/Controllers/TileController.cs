using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem;
using Code.BuldingsSystem.ScriptableObjects;
using Code.QuickOutline.Scripts;
using Code.TileSystem;
using Code.TileSystem.Interfaces;
using Code.UI;
using Code.UI.LevelScene;
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
        private ITextVisualizationOnUI _textVisualization;
        private BuildingController _buildingController;
        private UIController _uiController;
        private WorkerMenager _workerMenager;
        private List<BuildingConfig> _buildingConfigs;
        private OutlineController _outlineController;
        
        private int _currentUnits;
        public int CurrentLVL;
        public TileList List => _list;
        public ITextVisualizationOnUI TextVisualization => _textVisualization;
        public WorkerMenager WorkerMenager => _workerMenager;
        public TileModel TileModel => _tileView.TileModel;
        public TileView View => _tileView;

        public TileController(TileList tileList, UIController uiController, BuildingController buildingController
            , InputController inputController, OutlineController outlineController)
        {
            _workerMenager = new WorkerMenager(this, uiController.BottonUI.TileUIView);
            _outlineController = outlineController;
            _textVisualization = uiController.CenterUI.BaseNotificationUI;
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
            _outlineController.EnableOutLine(tile.Renderer);
            if(tile.TileModel.HouseType == HouseType.None) return;

            LoadInfo(tile);
        }
        public void Cancel()
        {
            _outlineController.DisableOutLine(_tileView.Renderer);
        }
        
        private void LoadInfo(TileView tile)
        {
            LoadBuildings(tile.TileModel);
            
            _uiView.Upgrade.onClick.AddListener(() => LVLUp());
            UpdateInfo(tile.TileModel.TileConfig);
            LoadFloodedBuildings();
            _workerMenager.FillWorkerList();
        }
        public void UpdateInfo(TileConfig config)
        {
            int hashCode = config.TileLvl.GetHashCode();
            _uiView.LvlText.text = $"{hashCode} LVL";
            CurrentLVL = hashCode;
            _uiView.UnitMax.text = $"{TileModel.CurrentWorkersUnits}/{config.MaxUnits} Units";
            _uiView.Icon.sprite = config.IconTile;
            _uiView.NameTile.text = TileModel.HouseType.ToString();
        }
        
        public void LVLUp()
        {
            int currentLevel = TileModel.SaveTileConfig.TileLvl.GetHashCode();
            if (currentLevel < _list.LVLList.Count)
            {
                TileModel.SaveTileConfig = List.LVLList[currentLevel];
                TileModel.TileConfig = TileModel.SaveTileConfig;
                TileModel.CurrBuildingConfigs.AddRange(TileModel.SaveTileConfig.BuildingTirs);
                
                UpdateInfo(TileModel.SaveTileConfig);
                LoadBuildings(TileModel);
                LevelCheck();
                WorkerMenager.FillWorkerList();
            }
            else
            {
                TextVisualization.BasicTemporaryUIVisualization("Max LVL", 3);
            }
        }
        
         public void LoadBuildings(TileModel model)
         {
            _uiController.Deinit();
            _buildingConfigs = model.CurrBuildingConfigs;
            
            var buildingConfigs = _buildingConfigs.FindAll(building => building.HouseType == TileModel.HouseType);
            if (TileModel.HouseType == HouseType.All)
            {
                foreach (var building in _buildingConfigs)
                {
                    var button = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuyPrefabButton, _uiController.CenterUI.BuildButtonsHolder);
                    _uiController.ButtonsInMenu.Add(building, button);
                    CreateButtonUI(building, button);
                }
            }
            else
            {
                foreach (var building in buildingConfigs)
                {
                    var button = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuyPrefabButton, _uiController.CenterUI.BuildButtonsHolder);
                    _uiController.ButtonsInMenu.Add(building, button);
                    CreateButtonUI(building, button);
                }
            }
            
            foreach (var kvp in _uiController.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() => _buildingController.BuildBuilding(kvp.Key, model, this));
            }
         }

         private void LoadFloodedBuildings()
         {
             var buildings = TileModel.FloodedBuildings.FindAll(x => x.MineralConfig == null);
             foreach (var building in buildings)
             {
                 var assignWorkers = _workerMenager.GetAssignedWorkers(building);
                 LoadBuildingInfo(building, assignWorkers);
             }
         }
        public BuildingUIInfo LoadBuildingInfo(ICollectable building, int units)
        {
            var button = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuildingInfo
                , _uiController.BottonUI.BuildingMenu.ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            
            view.Icon.sprite = building.Icon;
            view.Type.text = building.BuildingTypes.ToString();
            view.BuildingType = building.BuildingTypes;

            view.UnitsBusy.text = $"{units}/{TileModel.MaxWorkers}";
            view.Units = units;

            var destroyButton = view.DestroyBuildingInfo;
            _uiController.DestroyBuildingInfo.Add(button, view);
            destroyButton.onClick.AddListener(() => _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                , view, TileModel, this));

            view.PlusUnit.onClick.AddListener(() => view.Hiring(true, this, building));
            view.MinusUnit.onClick.AddListener(() => view.Hiring(false, this, building));
            
            LevelCheck();

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
        public BuildingUIInfo CreateBuildingInfo(BuildingConfig config, TileModel model, ICollectable building)
        {
            var view = GameObject.Instantiate(_uiController.BottonUI.BuildingMenu.BuildingInfo.GetComponent<BuildingUIInfo>()
                , _uiController.BottonUI.BuildingMenu.ByBuildButtonsHolder);
            view.Icon.sprite = config.Icon;
            view.Type.text = config.BuildingType.ToString();
            view.BuildingType = config.BuildingType;
            view.UnitsBusy.text = $"{view.Units}/{TileModel.MaxWorkers}";
            
            _uiController.DestroyBuildingInfo.Add(view.gameObject, view);
            
            view.DestroyBuildingInfo.onClick.AddListener((() => _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                , view, TileModel, this)));
            
            view.PlusUnit.onClick.AddListener((() => view.Hiring(true, this, building)));
            view.MinusUnit.onClick.AddListener((() => view.Hiring(false, this, building)));
            
            _uiController.IsWorkUI(UIType.Buy, false);
            
            LevelCheck();

            return view;
        }
        #region Other
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