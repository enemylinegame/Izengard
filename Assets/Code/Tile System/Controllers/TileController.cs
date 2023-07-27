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
    public class TileController : IDisposable, IOnController, IOnTile, ITileLoadInfo, IOnUpdate
    {
        #region Fields

        private TileList _list;
        private TileUIView _uiView;
        private TileView _tileView;


        private ITextVisualizationOnUI _playerNotificationSystem;
        private BuildingFactory _buildingFactory;
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

            _playerNotificationSystem = uiController.CenterUI.BaseNotificationUI;
            _list = tileList;
            _uiView = uiController.BottomUI.TileUIView;
            _uiController = uiController;
            _buildingFactory = buildingController;
            _inputController = inputController;

            inputController.Add(this);
        }

        #region LoadAndUnloadTile
        public void LoadInfoToTheUI(TileView tile)
        {
            TileTypeCheck(tile);
            _tileView = tile;
            if (tile.TileModel.HouseType == HouseType.None) return;

            LoadBuildings(tile.TileModel);
            _uiView.Upgrade.onClick.AddListener(LVLUp);
            LoadAllTextsFieldsAndImaged(tile.TileModel.TileConfig);
            LoadFloodedBuildings();
            LevelCheck();

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
                kvp.Value.onClick.AddListener(() => 
                    _buildingFactory.BuildBuilding(kvp.Key, model, this));
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
        public BuildingUIInfo CreateBuildingInfo(BuildingConfig config, 
            ICollectable building)
        {
            var buildingHud = Object.Instantiate(_uiController.BottomUI.BuildingMenu.
                BuildingInfo.GetComponent<BuildingUIInfo>()
                , _uiController.BottomUI.BuildingMenu.ByBuildButtonsHolder);

            buildingHud.Icon.sprite = config.Icon;
            buildingHud.Type.text = config.BuildingType.ToString();
            buildingHud.BuildingType = config.BuildingType;

            buildingHud.MaxWorkers = TileModel.MaxWorkers;
            buildingHud.WorkersСount = building.WorkersCount;


            _uiController.DestroyBuildingInfo.Add(buildingHud.gameObject, buildingHud);

            buildingHud.DestroyBuildingInfo.onClick.AddListener(() =>
                DestroyBuilding(buildingHud, building));

            buildingHud.PlusUnitButton.onClick.AddListener(() => 
                WorkerHiring(buildingHud, building));
            buildingHud.MinusUnitButton.onClick.AddListener(() => 
                WorkerDismissal(buildingHud, building));

            _uiController.IsWorkUI(UIType.Buy, false);
            LevelCheck();
            return buildingHud;
        }
        private void LoadBuildingInfo(ICollectable building)
        {
            var button = Object.Instantiate(_uiController.BottomUI.BuildingMenu.BuildingInfo
                , _uiController.BottomUI.BuildingMenu.ByBuildButtonsHolder);
            var buildingHud = button.GetComponent<BuildingUIInfo>();

            buildingHud.Icon.sprite = building.Icon;
            buildingHud.Type.text = building.BuildingTypes.ToString();
            buildingHud.BuildingType = building.BuildingTypes;
            buildingHud.BuildingID = building.BuildingID;

            buildingHud.MaxWorkers = building.MaxWorkers;
            buildingHud.WorkersСount = building.WorkersCount;

            var destroyButton = buildingHud.DestroyBuildingInfo;
            _uiController.DestroyBuildingInfo.Add(button, buildingHud);
            destroyButton.onClick.AddListener(
                () => DestroyBuilding(buildingHud, building));

            buildingHud.PlusUnitButton.onClick.AddListener(() => 
                WorkerHiring(buildingHud, building));

            buildingHud.MinusUnitButton.onClick.AddListener(() => 
                WorkerDismissal(buildingHud, building));

            LevelCheck();
        }

        private void DestroyBuilding(
            BuildingUIInfo buildingHud, ICollectable building)
        {
            _productionManager.StopAllProductions(building);

            _buildingFactory.DestroyBuilding(TileModel.FloodedBuildings, 
                buildingHud, TileModel, this);

            ResetWorkersAccount(building);
        }

        public bool IsThereFreeWorkers(ICollectable building)
        {
            if (building.WorkersCount >= building.MaxWorkers ||
                TileModel.WorkersCount >= TileModel.MaxWorkers)
                return false;

            return true;
        }

        public bool IsThereBusyWorkers(ICollectable building)
        {
            if (building.WorkersCount > 0)
                return true;

            return false;
        }

        private void WorkerHiring(BuildingUIInfo buildingUI, ICollectable building)
        {
            if (!IsThereFreeWorkers(building))
                return;

            _productionManager.StartFactoryProduction(
                _tileView.gameObject.transform.position,
                 building, building.WorkerPreparation);

            IncrementWorkersAccount(building);
            buildingUI.WorkersСount = building.WorkersCount;
        }

        private void WorkerDismissal(BuildingUIInfo buildingUI, ICollectable building)
        {
            if (!IsThereBusyWorkers(building))
                return;

            _productionManager.StopFirstFindedWorker(building);
            DecrementWorkersAccount(building);
            buildingUI.WorkersСount = building.WorkersCount;
        }

        public void IncrementWorkersAccount(ICollectable building)
        {
            ++building.WorkersCount;
            ++TileModel.WorkersCount;
            _uiView.WorkersCount = TileModel.WorkersCount;
        }

        public void DecrementWorkersAccount(ICollectable building)
        {
            --building.WorkersCount;
            --TileModel.WorkersCount;
            _uiView.WorkersCount = TileModel.WorkersCount;
        }

        public void ResetWorkersAccount(ICollectable building)
        {
            TileModel.WorkersCount -= building.WorkersCount;
            if (TileModel.WorkersCount < 0)
                Debug.LogError("TileModel.WorkersCount < 0");

            building.WorkersCount = 0;
            _uiView.WorkersCount = TileModel.WorkersCount;
        }

         private void LoadFloodedBuildings()
         {
             var buildings = TileModel.FloodedBuildings.FindAll(x => x.MineralConfig == null);

             foreach (var building in buildings)
             {
                 LoadBuildingInfo(building);
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
            _buildingFactory.PlaceCenterBuilding(tile);

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
           
            foreach (var kvp in _uiController.ButtonsInMenu)
                kvp.Value.onClick.RemoveAllListeners();
            _uiController.BottomUI.BuildingMenu.CloseMenuButton.onClick.RemoveAllListeners();
            _uiController.Deinit();
            _uiView.Upgrade.onClick.RemoveAllListeners();
        }
        public void LevelCheck()
        {
            bool levelExceedDestroyBuildingInfo = _currentLVL > 
                _uiController.DestroyBuildingInfo.Count;

            _uiController.BottomUI.BuildingMenu.PrefabButtonClear.
                gameObject.SetActive(levelExceedDestroyBuildingInfo);
        }
        public void LVLUp()
        {
            int currentLevel = TileModel.SaveTileConfig.TileLvl.GetHashCode();
            if (currentLevel == _list.LVLList.Count)
            {
                _playerNotificationSystem.BasicTemporaryUIVisualization("Max LVL", 2);
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

            _uiView.MaxWorkersCount = config.MaxUnits;
            _uiView.WorkersCount = TileModel.WorkersCount;
            
            _uiView.Icon.sprite = config.IconTile;
            _uiView.NameTile.text = TileModel.HouseType.ToString();
        }
        #endregion

        public void OnUpdate(float deltaTime)
        {
            if(_tileView != null)
                Debug.Log($"Currenthealth: {_tileView.TileModel.CurrentHealth}");
        }
    }
}