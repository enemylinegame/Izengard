using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.UI;
using Code.Player;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;

namespace Code.TileSystem
{
    public class TileController : IDisposable, IOnController, IOnTile, ITileLoadInfo
    {
        #region Fields
        
        private TileList _list;
        private TileUIView _uiView;
        private TileView _tileView;
        private ITextVisualizationOnUI _textVisualization;
        private BuildingFactory _buildingController;
        private InputController _inputController;
        private UIController _uiController;
        private ProductionManager _productionManager;
        private List<BuildingConfig> _buildingConfigs;
        private int _currentLVL;
        public ProductionManager WorkerMenager => _productionManager;
        public TileModel TileModel => _tileView.TileModel;
        public TileView View => _tileView;
        
        #endregion
        public TileController(TileList tileList, 
            UIController uiController, 
            BuildingFactory buildingController, 
            InputController inputController,
            ProductionManager productionManager)
        {
            _productionManager = productionManager;
            
            _textVisualization = uiController.CenterUI.BaseNotificationUI;
            _list = tileList;
            _uiView = uiController.BottomUI.TileUIView;
            _uiController = uiController;
            _buildingController = buildingController;
            _inputController = inputController;
            
            inputController.Add(this);
        }

        private void OnWorksCountChanged(int workersCount)
        {
            if (null != TileModel)
            {
                TileModel.CurrentWorkersUnits = workersCount;
                _uiView.UnitMax.text = $"{TileModel.CurrentWorkersUnits}/{TileModel.MaxWorkers} Units";
            }
        }
        #region LoadAndUnloadTile
        public void LoadInfoToTheUI(TileView tile)
        {
            TileTypeCheck(tile);
            _tileView = tile;
            if(tile.TileModel.HouseType == HouseType.None) return;

            LoadBuildings(tile.TileModel);
            _uiView.Upgrade.onClick.AddListener(LVLUp);
            LoadAllTextsFieldsAndImaged(tile.TileModel.TileConfig);
            LoadFloodedBuildings();
            LevelCheck();

            _productionManager.OnWorksCountChanged += OnWorksCountChanged;
        }
        public void Cancel() { }
        #endregion
        #region BuildingBuy

        private void LoadBuildings(TileModel model)
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
                var button = Object.Instantiate(_uiController.BottomUI.BuildingMenu.BuyPrefabButton, _uiController.CenterUI.BuildButtonsHolder);
                _uiController.ButtonsInMenu.Add(building, button);
                CreateButtonUI(building, button);
            }

            foreach (var kvp in _uiController.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() => _buildingController.BuildBuilding(kvp.Key, model, this));
            }
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

        #endregion
        #region BuildingInfoAndHiring
        public BuildingUIInfo CreateBuildingInfo(BuildingConfig config, ICollectable building)
         {
             var view = Object.Instantiate(_uiController.BottomUI.BuildingMenu.BuildingInfo.GetComponent<BuildingUIInfo>()
                 , _uiController.BottomUI.BuildingMenu.ByBuildButtonsHolder);
             view.Icon.sprite = config.Icon;
             view.Type.text = config.BuildingType.ToString();
             view.BuildingType = config.BuildingType;
             view.UnitsBusy.text = $"{view.CurrentUnits}/{building.MaxWorkers}";
            
             _uiController.DestroyBuildingInfo.Add(view.gameObject, view);
            
             view.DestroyBuildingInfo.onClick.AddListener(() => _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                 , view, TileModel, this));
            
             view.PlusUnit.onClick.AddListener(() => Hiring(true, view, building));
             view.MinusUnit.onClick.AddListener(() => Hiring(false, view, building));
            
             _uiController.IsWorkUI(UIType.Buy, false);
             LevelCheck();
             return view;
         }
         private void LoadBuildingInfo(ICollectable building, int units)
         {
             var button = Object.Instantiate(_uiController.BottomUI.BuildingMenu.BuildingInfo
                 , _uiController.BottomUI.BuildingMenu.ByBuildButtonsHolder);
             var view = button.GetComponent<BuildingUIInfo>();
            
             view.Icon.sprite = building.Icon;
             view.Type.text = building.BuildingTypes.ToString();
             view.BuildingType = building.BuildingTypes;

             view.UnitsBusy.text = $"{units}/{building.MaxWorkers}";
             view.CurrentUnits = units;

             var destroyButton = view.DestroyBuildingInfo;
             _uiController.DestroyBuildingInfo.Add(button, view);
             destroyButton.onClick.AddListener(() => _buildingController.DestroyBuilding(TileModel.FloodedBuildings
                 , view, TileModel, this));

             view.PlusUnit.onClick.AddListener(() => Hiring(true, view, building));
             view.MinusUnit.onClick.AddListener(() => Hiring(false, view, building));
            
             LevelCheck();
         }

        private void Hiring(bool isOn, BuildingUIInfo buildingUI, ICollectable building)
         {
            if (isOn)
            {
                if(buildingUI.CurrentUnits >= building.MaxWorkers ||
                    TileModel.CurrentWorkersUnits >= TileModel.MaxWorkers) 
                    return;


                if (!_productionManager.IsThereFreeWorkers(building))
                    return;

                _productionManager.StartFactoryProduction(
                    _tileView.gameObject.transform.position,
                     building, building.WorkerPreparation);

                buildingUI.CurrentUnits += 1;
            }
            else
            {
                if (!_productionManager.IsThereBusyWorkers(building))
                    return;

                _productionManager.StopFirstFindedWorker(building);
                buildingUI.CurrentUnits -= 1;
            }

             buildingUI.UnitsBusy.text = $"{buildingUI.CurrentUnits}/{building.MaxWorkers}";
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

        #endregion
        #region TileTypeChange
        private void TileTypeCheck(TileView view)
        {
            if(view.TileModel.HouseType != HouseType.None) return;
            
            _uiController.IsWorkUI(UIType.TileSel, true);

            _uiController.CenterUI.TIleSelection.TileEco.onClick.AddListener(() => TileType(HouseType.Eco, view));
            _uiController.CenterUI.TIleSelection.TileWar.onClick.AddListener(() => TileType(HouseType.war, view));
            _uiController.CenterUI.TIleSelection.Back.onClick.AddListener(() => RemoveListenersTileSelection(true));
            Debug.Log($"Блокировка UI: {_inputController.LockRightClick}");
        }
        private void TileType(HouseType type, TileView tile)
        {
            tile.TileModel.HouseType = type;

            RemoveListenersTileSelection(false);
            _buildingController.PlaceCenterBuilding(tile);

            _uiController.IsWorkUI(UIType.TileSel, false);
            _uiController.IsWorkUI(UIType.Tile, true);
            LoadInfoToTheUI(tile);
        }

        private void RemoveListenersTileSelection(bool IsBack)
        {
            _uiController.CenterUI.TIleSelection.TileEco.onClick.RemoveAllListeners();
            _uiController.CenterUI.TIleSelection.TileWar.onClick.RemoveAllListeners();
            if (IsBack)
            {
                _inputController.IsOnTile = true;
                _inputController.HardOffTile();
            }
            _uiController.CenterUI.TIleSelection.Back.onClick.RemoveListener(() => RemoveListenersTileSelection(true));
        }

        #endregion
        #region Other
        public void Dispose()
        {
            _productionManager.OnWorksCountChanged -= OnWorksCountChanged;

            foreach (var kvp in _uiController.ButtonsInMenu)
                kvp.Value.onClick.RemoveAllListeners();
            _uiController.BottomUI.BuildingMenu.CloseMenuButton.onClick.RemoveAllListeners();
            _uiController.Deinit();
            _uiView.Upgrade.onClick.RemoveAllListeners();
        }
        public void LevelCheck()
        {
            bool levelExceedDestroyBuildingInfo = _currentLVL > _uiController.DestroyBuildingInfo.Count;
            _uiController.BottomUI.BuildingMenu.PrefabButtonClear.gameObject.SetActive(levelExceedDestroyBuildingInfo);
        }
        public void LVLUp()
        {
            int currentLevel = TileModel.SaveTileConfig.TileLvl.GetHashCode();
            if (currentLevel == _list.LVLList.Count)
            {
                _textVisualization.BasicTemporaryUIVisualization("Max LVL", 2);
                return;
            }
            
            TileModel.SaveTileConfig = _list.LVLList[currentLevel];
            TileModel.TileConfig = TileModel.SaveTileConfig;
            TileModel.CurrBuildingConfigs.AddRange(TileModel.SaveTileConfig.BuildingTirs);
                
            LoadAllTextsFieldsAndImaged(TileModel.SaveTileConfig);
            LoadBuildings(TileModel);
            LevelCheck();
        }
        
        private void LoadAllTextsFieldsAndImaged(TileConfig config)
        {
            int hashCode = config.TileLvl.GetHashCode();
            _uiView.LvlText.text = $"{hashCode} LVL";
            _currentLVL = hashCode;
            _uiView.UnitMax.text = $"{TileModel.CurrentWorkersUnits}/{config.MaxUnits} Units";
            _uiView.Icon.sprite = config.IconTile;
            _uiView.NameTile.text = TileModel.HouseType.ToString();
        }
        #endregion

    }
}