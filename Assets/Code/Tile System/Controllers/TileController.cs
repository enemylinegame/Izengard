using System;
using System.Collections.Generic;
using System.Diagnostics;
using Code.BuildingSystem;
using Code.UI;
using Code.Player;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Code.TileSystem
{
    public class TileController : IDisposable, IOnController, IOnTile, ITileLoadInfo, IOnUpdate
    {
        #region Fields
        
        private TileUIView _uiView;
        private TileView _tileView;
        private ITextVisualizationOnUI _playerNotificationSystem;
        private BuildingFactory _buildingFactory;
        private InputController _inputController;
        private UIController _uiController;
        private ProductionManager _productionManager;
        private List<BuildingConfig> _buildingConfigs;
        private ButtonsControllerOnTile _buttonsController;
        private LevelOfLifeButtonsCustomizer _level;
        private readonly GlobalStock _stock;
        private readonly GlobalTileSettings _tileSettings;
        private int _currentLVL;
        public ProductionManager WorkerMenager => _productionManager;
        public TileModel TileModel => _tileView.TileModel;
        public TileView View => _tileView;

        #endregion
        public TileController(UIController uiController, BuildingFactory buildingController, 
            InputController inputController, ProductionManager productionManager, 
            LevelOfLifeButtonsCustomizer level, GlobalStock stock, GlobalTileSettings tileSettings)
        {
            _buttonsController = new ButtonsControllerOnTile(uiController, inputController);
            
            _productionManager = productionManager;
            _playerNotificationSystem = uiController.CenterUI.BaseNotificationUI;
            _uiView = uiController.BottomUI.TileUIView;
            _uiController = uiController;
            _buildingFactory = buildingController;
            _inputController = inputController;
            _level = level;
            _stock = stock;
            _tileSettings = tileSettings;

            inputController.Add(this);
        }

        #region LoadAndUnloadTile
        public void LoadInfoToTheUI(TileView tile)
        {
            if (tile.TileModel.TileType == BuildingSystem.TileType.None)
            {
                TileTypeCheck(tile);
                return;
            } 
            _tileView = tile;

            LoadBuildings(tile.TileModel, tile.TileModel.TileType);
            _buttonsController.ButtonAddListener(tile.TileModel, _level);
            _buttonsController.HolderButton(ButtonTypes.Upgrade).onClick.AddListener(LVLUp);
            LoadAllTextsFieldsAndImaged(tile.TileModel.TileConfig);
            LoadFloodedBuildings();
            LevelCheck();

        }

        public void Cancel()
        {
            _buttonsController.RemoveListeners(ButtonTypes.All, TileModel);
        }
        #endregion
        #region BuildingBuy

        private void LoadBuildings(TileModel model, TileType type)
        {
            List<BuildingConfig> buildingConfigs = null;
            _uiController.Deinit();
            _buildingConfigs = model.CurrBuildingConfigs;

            switch (type)
            {
                case BuildingSystem.TileType.All:
                    buildingConfigs = _buildingConfigs;
                    break;
                case BuildingSystem.TileType.war:
                    buildingConfigs = _buildingConfigs.FindAll(building => building.TileType.Exists(x => x == BuildingSystem.TileType.war));
                    break;
                case BuildingSystem.TileType.Eco:
                    buildingConfigs = _buildingConfigs.FindAll(building => building.TileType.Exists(x => x == BuildingSystem.TileType.Eco));
                    break;
            }

            buildingConfigs.ForEach(building =>
            {
                var button = Object.Instantiate(_uiController.BottomUI.BuildingMenu.BuyPrefabButton, _uiController.CenterUI.BuildButtonsHolder);
                _uiController.ButtonsInMenu.Add(building, button);
                CreateButtonUI(building, button);
            });
            
            foreach (var kvp in _uiController.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() =>
                {
                    _buildingFactory.BuildBuilding(kvp.Key, model, this);
                });
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
            var buildingHud = Object.Instantiate(_uiController.BottomUI.BuildingMenu.BuildingInfo.GetComponent<BuildingUIInfo>(), 
                _uiController.BottomUI.BuildingMenu.ByBuildButtonsHolder);

            buildingHud.Icon.sprite = config.Icon;
            buildingHud.Type.text = config.BuildingType.ToString();
            buildingHud.BuildingType = config.BuildingType;

            buildingHud.MaxWorkers = TileModel.MaxWorkers;
            buildingHud.WorkersСount = building.WorkersCount;


            _uiController.DestroyBuildingInfo.Add(buildingHud.gameObject, buildingHud);

            buildingHud.DestroyBuildingInfo.onClick.AddListener(() => DestroyBuilding(buildingHud, building));

            buildingHud.PlusUnitButton.onClick.AddListener(() => WorkerHiring(buildingHud, building));
            buildingHud.MinusUnitButton.onClick.AddListener(() => WorkerDismissal(buildingHud, building));

            _uiController.IsWorkUI(UIType.Buy, false);
            LevelCheck();
            return buildingHud;
        }
        private void LoadBuildingInfo(ICollectable building)
        {
            var button = Object.Instantiate(_uiController.BottomUI.BuildingMenu.BuildingInfo, 
                _uiController.BottomUI.BuildingMenu.ByBuildButtonsHolder);
            var buildingHud = button.GetComponent<BuildingUIInfo>();

            buildingHud.Icon.sprite = building.Icon;
            buildingHud.Type.text = building.BuildingTypes.ToString();
            buildingHud.BuildingType = building.BuildingTypes;
            buildingHud.BuildingID = building.BuildingID;

            buildingHud.MaxWorkers = building.MaxWorkers;
            buildingHud.WorkersСount = building.WorkersCount;

            var destroyButton = buildingHud.DestroyBuildingInfo;
            _uiController.DestroyBuildingInfo.Add(button, buildingHud);
            destroyButton.onClick.AddListener(() => DestroyBuilding(buildingHud, building));

            buildingHud.PlusUnitButton.onClick.AddListener(() => WorkerHiring(buildingHud, building));

            buildingHud.MinusUnitButton.onClick.AddListener(() => WorkerDismissal(buildingHud, building));

            LevelCheck();
        }

        private void DestroyBuilding(BuildingUIInfo buildingHud, ICollectable building)
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
            if(view.TileModel.TileType != BuildingSystem.TileType.None && view.TileModel.TileType == BuildingSystem.TileType.All) return;
            
            _uiController.IsWorkUI(UIType.TileSel, true);

            _uiController.CenterUI.TIleSelection.TileEco.onClick.AddListener(() => TileType(BuildingSystem.TileType.Eco, view));
            _uiController.CenterUI.TIleSelection.TileWar.onClick.AddListener(() => TileType(BuildingSystem.TileType.war, view));
            _uiController.CenterUI.TIleSelection.Back.onClick.AddListener(() => RemoveListenersTileSelection(true));
        }
        private void TileType(TileType type, TileView tile)
        {
            switch (type)
            {
                case BuildingSystem.TileType.Eco:
                    tile.TileModel.MaxWarriors = _tileSettings.MaxWarriorsEco;
                    break;
                case BuildingSystem.TileType.war:
                    tile.TileModel.MaxWarriors = _tileSettings.MaxWorkersWar;
                    break;
            }
            
            tile.TileModel.TileType = type;
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
                _inputController.LockLeftClick = true;
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
        }
        public void LevelCheck()
        {
            bool levelExceedDestroyBuildingInfo = _currentLVL > 
                _uiController.DestroyBuildingInfo.Count;

            _uiController.BottomUI.BuildingMenu.PrefabButtonClear.
                gameObject.SetActive(levelExceedDestroyBuildingInfo);
        }
        private void LVLUp()
        {
            if (!IsResourcesEnough(TileModel.TileConfig)) return;
            
            TileModel.TileConfig.PriceUpgrade.ForEach(resourcePrice =>
            {
                _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost);
            });
            
            int currentLevel = TileModel.SaveTileConfig.TileLvl.GetHashCode();
            if (currentLevel == _tileSettings.Levels.Count)
            {
                _playerNotificationSystem.BasicTemporaryUIVisualization("Max LVL", 2);
                return;
            }
            
            TileModel.SaveTileConfig = _tileSettings.Levels[currentLevel];
            TileModel.TileConfig = TileModel.SaveTileConfig;
            TileModel.CurrBuildingConfigs.AddRange(TileModel.SaveTileConfig.BuildingTirs);
                
            LoadAllTextsFieldsAndImaged(TileModel.SaveTileConfig);
            LoadBuildings(TileModel, TileModel.TileType);
            LevelCheck();
        }
        
        private void LoadAllTextsFieldsAndImaged(TileConfig config)
        {
            int hashCode = config.TileLvl.GetHashCode();
            _uiView.LvlText.text = $"{hashCode} LVL";
            _currentLVL = hashCode;

            _uiView.MaxWorkersCount = config.MaxWorkers;
            _uiView.WorkersCount = TileModel.WorkersCount;
            
            _uiView.Icon.sprite = config.IconTile;
            _uiView.NameTile.text = TileModel.TileType.ToString();
        }
        
        private bool IsResourcesEnough(TileConfig tileConfig)
        {
            foreach (ResourcePriceModel resourcePriceModel in tileConfig.PriceUpgrade)
            {
                if (!_stock.CheckResourceInStock(resourcePriceModel.ResourceType, resourcePriceModel.Cost))
                {
                    _playerNotificationSystem.BasicTemporaryUIVisualization("you do not have enough resources to buy", 1);
                    return false;
                }
            }
            return true;
        }
        #endregion

        public void OnUpdate(float deltaTime)
        {
            if (_tileView != null)
            {
                _buttonsController.ButtonsChecker(TileModel);
            }
            
        }
    }
}