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
        private InputController _inputController;
        private UIController _uiController;
        private ProductionManager _productionManager;
        private List<BuildingConfig> _buildingConfigs;
        private OutlineController _outlineController;
        
        private int _currentUnits;
        public int CurrentLVL;
        public TileList List => _list;
        public ITextVisualizationOnUI TextVisualization => _textVisualization;
        public ProductionManager WorkerMenager => _productionManager;
        public TileModel TileModel => _tileView.TileModel;
        public TileView View => _tileView;

        public TileController(TileList tileList, UIController uiController, 
            BuildingController buildingController, 
            InputController inputController, OutlineController outlineController,
            ProductionManager productionManager)
        {
            _productionManager = productionManager;
            _productionManager.SeMaxWorks(TileModel.MaxWorkers);
            _productionManager.OnWorksCountChanged += OnWorksCountChanged;

            _outlineController = outlineController;
            _textVisualization = uiController.CenterUI.BaseNotificationUI;
            _list = tileList;
            _uiView = uiController.BottonUI.TileUIView;
            _uiController = uiController;
            _buildingController = buildingController;
            _inputController = inputController;
            inputController.Add(this);
        }

        private void OnWorksCountChanged(int workksCount)
        {
            TileModel.CurrentWorkersUnits = workksCount;
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
            if (currentLevel == _list.LVLList.Count)
            {
                TextVisualization.BasicTemporaryUIVisualization("Max LVL", 2);
                return;
            }
            
            TileModel.SaveTileConfig = List.LVLList[currentLevel];
            TileModel.TileConfig = TileModel.SaveTileConfig;
            TileModel.CurrBuildingConfigs.AddRange(TileModel.SaveTileConfig.BuildingTirs);
                
            UpdateInfo(TileModel.SaveTileConfig);
            LoadBuildings(TileModel);
            LevelCheck();
        }
        
         public void LoadBuildings(TileModel model)
         { 
             List<BuildingConfig> buildingConfigs;
            _uiController.Deinit();
            _buildingConfigs = model.CurrBuildingConfigs;
            if (TileModel.HouseType == HouseType.All)
            {
                buildingConfigs = _buildingConfigs;
            }
            else
            {
                buildingConfigs = _buildingConfigs.FindAll(building => building.HouseType == TileModel.HouseType);
            }
            
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
         }

         private void LoadFloodedBuildings()
         {
             var buildings = TileModel.FloodedBuildings.FindAll(x => x.MineralConfig == null);
             foreach (var building in buildings)
             {
                 var assignWorkers = _productionManager.GetAssignedWorkers(building);
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
            view.CurrentUnits = units;

            var destroyButton = view.DestroyBuildingInfo;
            _uiController.DestroyBuildingInfo.Add(button, view);
            destroyButton.onClick.AddListener(() => _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                , view, TileModel, this));

            view.PlusUnit.onClick.AddListener(() => Hiring(true, view, building));
            view.MinusUnit.onClick.AddListener(() => Hiring(false, view, building));
            
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
            view.UnitsBusy.text = $"{view.CurrentUnits}/{TileModel.MaxWorkers}";
            
            _uiController.DestroyBuildingInfo.Add(view.gameObject, view);
            
            view.DestroyBuildingInfo.onClick.AddListener((() => _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                , view, TileModel, this)));
            
            view.PlusUnit.onClick.AddListener(() => Hiring(true, view, building));
            view.MinusUnit.onClick.AddListener(() => Hiring(false, view, building));
            
            _uiController.IsWorkUI(UIType.Buy, false);
            
            LevelCheck();

            return view;
        }
        #region Other
        public void Dispose()
        {
            _productionManager.OnWorksCountChanged -= OnWorksCountChanged;

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
        
        /// <summary>
        /// найм юнитов для определеного типа здания
        /// </summary>
        public void Hiring(bool isOn, BuildingUIInfo buildingUI, ICollectable building)
        {
            Vector3 workPlace = Vector3.forward * 10.0f;
            Debug.LogWarning("workPlace is only for test.");

            var hire = isOn 
                ? _productionManager.StartProduction(
                    buildingUI, building, workPlace, null) 
                : _productionManager.StopFirstFindedProduction(
                    buildingUI, building);

            if (!hire) return;
            
            buildingUI.CurrentUnits += isOn ? 1 : -1;
            if(buildingUI.CurrentUnits <=0) buildingUI.CurrentUnits = 0;
            buildingUI.UnitsBusy.text = $"{buildingUI.CurrentUnits}/{TileModel.MaxWorkers}";
        }

        private void TileTypeCheck(TileView view)
        {
            if(view.TileModel.HouseType != HouseType.None) return;
            
            _uiController.IsWorkUI(UIType.TileSel, true);
            
            _uiController.CenterUI.TIleSelection.TileEco.onClick.AddListener(() => TileType(HouseType.Eco, view));
            _uiController.CenterUI.TIleSelection.TileWar.onClick.AddListener(() => TileType(HouseType.war, view));
            _uiController.CenterUI.TIleSelection.Back.onClick.AddListener(() => RemoveListenersTileSelection(true));
        }
        
        private void TileType(HouseType type, TileView tile)
        {
            tile.TileModel.HouseType = type;

            RemoveListenersTileSelection(false);

            _uiController.IsWorkUI(UIType.TileSel, false);
            _uiController.IsWorkUI(UIType.Tile, true);
            LoadInfo(tile);
        }

        private void RemoveListenersTileSelection(bool IsBack)
        {
            _uiController.CenterUI.TIleSelection.TileEco.onClick.RemoveAllListeners();
            _uiController.CenterUI.TIleSelection.TileWar.onClick.RemoveAllListeners();
            if (IsBack)
            {
                _inputController.IsOnTile = true;
                Cancel();
            }
            _uiController.CenterUI.TIleSelection.Back.onClick.RemoveListener(() => RemoveListenersTileSelection(true));
        }

        #endregion
    }
}