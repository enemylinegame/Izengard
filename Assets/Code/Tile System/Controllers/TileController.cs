using System;
using System.Collections.Generic;
using System.Diagnostics;
using Code.BuildingSystem;
using Code.UI;
using Code.Player;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Code.TileSystem
{
    public class TileController : IDisposable, IOnController, IOnTile, ITileLoadInfo, IOnUpdate
    {
        #region Fields

        public event Action<TileView> TileTypeChange;
        
        private TileView _tileView;
        private BuildingFactory _buildingFactory;
        private ProductionManager _productionManager;
        private List<BuildingConfig> _buildingConfigs;
        private ButtonsControllerOnTile _buttonsController;
        private LevelOfLifeButtonsCustomizer _level;
        private readonly GlobalStock _stock;
        private readonly GlobalTileSettings _tileSettings;
        private int _currentLVL;
        
        private CenterPanelController _centerPanelController;
        private TilePanelController _tilePanelController;
        private TileMainBoardController _tileMainBoard;
        private NotificationPanelController _notificationPanel;
        public ProductionManager WorkerMenager => _productionManager;
        public TileModel TileModel => _tileView.TileModel;
        public TileView View => _tileView;

        #endregion
        public TileController(UIPanelsInitialization uiPanelsInitialization, BuildingFactory buildingController, 
            InputController inputController, ProductionManager productionManager, 
            LevelOfLifeButtonsCustomizer level, GlobalStock stock, GlobalTileSettings tileSettings)
        {
            _buttonsController = new ButtonsControllerOnTile(uiPanelsInitialization.TileMainBoard, inputController);
            
            _productionManager = productionManager;
            _centerPanelController = uiPanelsInitialization.CenterPanelController;
            _tilePanelController = uiPanelsInitialization.TilePanelController;
            _tileMainBoard = uiPanelsInitialization.TileMainBoard;
            _notificationPanel = uiPanelsInitialization.NotificationPanel;
            _buildingFactory = buildingController;
            _level = level;
            _stock = stock;
            _tileSettings = tileSettings;

            inputController.Add(this);
        }

        #region LoadAndUnloadTile
        public void LoadInfoToTheUI(TileView tile)
        {
            if (tile.TileModel.TileType == TileType.None)
            {
                TileTypeCheck(tile);
                return;
            } 
            _tileView = tile;

            LoadBuildings(tile.TileModel, tile.TileModel.TileType);
            _buttonsController.ButtonAddListener(tile.TileModel, _level);
            _tileMainBoard.HolderButton(ButtonTypes.Upgrade).onClick.AddListener(LVLUp);
            _tileMainBoard.LoadAllTextsFieldsAndImaged(tile.TileModel.TileConfig, TileModel, out _currentLVL);
            LoadFloodedBuildings();
            LevelCheck();

        }

        public void Cancel()
        {
            _tileMainBoard.RemoveListeners(ButtonTypes.All, TileModel);
        }
        #endregion
        #region BuildingBuy

        private void LoadBuildings(TileModel model, TileType type)
        {
            List<BuildingConfig> buildingConfigs = null;
            _tilePanelController.Deinit();
            _buildingConfigs = model.CurrBuildingConfigs;

            switch (type)
            {
                case TileType.All:
                    buildingConfigs = _buildingConfigs;
                    break;
                case TileType.war:
                    buildingConfigs = _buildingConfigs.FindAll(building => building.TileType.Exists(x => x == TileType.war));
                    break;
                case TileType.Eco:
                    buildingConfigs = _buildingConfigs.FindAll(building => building.TileType.Exists(x => x == TileType.Eco));
                    break;
            }

            buildingConfigs.ForEach(building =>
            {
                var button = Object.Instantiate(_tilePanelController.TileMenu.GetBuyPrefabButton(), 
                    _centerPanelController.TransformBuildButtonsHolder());
                _tilePanelController.ButtonsInMenu.Add(building, button);
                
                CreateButtonUI(building, button);
            });
            
            foreach (var kvp in _tilePanelController.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() =>
                {
                    ICollectable building = _buildingFactory.BuildBuilding(kvp.Key, model);
                    if (building == null)
                    {
                        _centerPanelController.DeactivateBuildingBuyUI();
                        return; 
                    }
                    BuildingHUD info = CreateBuildingInfo(kvp.Key, building);
                    LoadBuildingHUD(info, building, kvp.Key);
                    model.FloodedBuildings.Add(building);
                    building.InitBuilding();
                    LevelCheck();
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
        public BuildingHUD CreateBuildingInfo(BuildingConfig config, ICollectable building)
        {
            var buildingHud = Object.Instantiate(_tilePanelController.TileMenu.GetPrefabBuildingInfo().GetComponent<BuildingHUD>(), 
                _tilePanelController.TileMenu.GetByBuildButtonsHolder());

            buildingHud.Icon.sprite = config.Icon;
            buildingHud.Type.text = config.BuildingType.ToString();
            buildingHud.BuildingType = config.BuildingType;

            buildingHud.MaxWorkers = TileModel.MaxWorkers;
            buildingHud.WorkersСount = building.WorkersCount;


            _tilePanelController.DestroyBuildingInfo.Add(buildingHud.gameObject, buildingHud);

            buildingHud.DestroyBuildingInfo.onClick.AddListener(() => DestroyBuilding(buildingHud, building));

            buildingHud.PlusUnitButton.onClick.AddListener(() => WorkerHiring(buildingHud, building));
            buildingHud.MinusUnitButton.onClick.AddListener(() => WorkerDismissal(buildingHud, building));

            _centerPanelController.DeactivateBuildingBuyUI();
            LevelCheck();
            return buildingHud;
        }
        private void LoadBuildingInfo(ICollectable building)
        {
            var button = Object.Instantiate(_tilePanelController.TileMenu.GetPrefabBuildingInfo(), 
                _tilePanelController.TileMenu.GetByBuildButtonsHolder());
            var buildingHud = button.GetComponent<BuildingHUD>();

            buildingHud.Icon.sprite = building.Icon;
            buildingHud.Type.text = building.BuildingTypes.ToString();
            buildingHud.BuildingType = building.BuildingTypes;
            buildingHud.BuildingID = building.BuildingID;

            buildingHud.MaxWorkers = building.MaxWorkers;
            buildingHud.WorkersСount = building.WorkersCount;

            var destroyButton = buildingHud.DestroyBuildingInfo;
            _tilePanelController.DestroyBuildingInfo.Add(button, buildingHud);
            destroyButton.onClick.AddListener(() => DestroyBuilding(buildingHud, building));

            buildingHud.PlusUnitButton.onClick.AddListener(() => WorkerHiring(buildingHud, building));

            buildingHud.MinusUnitButton.onClick.AddListener(() => WorkerDismissal(buildingHud, building));

            LevelCheck();
        }

        private void DestroyBuilding(BuildingHUD buildingHud, ICollectable building)
        {
            _productionManager.StopAllProductions(building);
            UnLoadBuildingHUD(buildingHud);
            _buildingFactory.DestroyBuilding(building, buildingHud, TileModel);
            WorkerMenager.StopAllProductions(building);
            ResetWorkersAccount(building);
            LevelCheck();
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

        private void WorkerHiring(BuildingHUD buildingUI, ICollectable building)
        {
            if (!IsThereFreeWorkers(building))
                return;

            _productionManager.StartFactoryProduction(_tileView.gameObject.transform.position, building, building.WorkerPreparation);

            IncrementWorkersAccount(building);
            buildingUI.WorkersСount = building.WorkersCount;
        }
        
        private void WorkerDismissal(BuildingHUD buildingUI, ICollectable building)
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
            _tileMainBoard.ChangeWorkersCount(TileModel.WorkersCount);
        }

        public void DecrementWorkersAccount(ICollectable building)
        {
            --building.WorkersCount;
            --TileModel.WorkersCount;
            _tileMainBoard.ChangeWorkersCount(TileModel.WorkersCount);
        }

        public void ResetWorkersAccount(ICollectable building)
        {
            TileModel.WorkersCount -= building.WorkersCount;
            if (TileModel.WorkersCount < 0)
                Debug.LogError("TileModel.WorkersCount < 0");

            building.WorkersCount = 0;
            _tileMainBoard.ChangeWorkersCount(TileModel.WorkersCount);
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
            if(view.TileModel.TileType != TileType.None && view.TileModel.TileType == TileType.All) return;
            
            _centerPanelController.ActivateTileTypeSelection(SetTileType, view);
        }
        private void SetTileType(TileType type, TileView tile)
        {
            switch (type)
            {
                case TileType.Eco:
                    tile.TileModel.MaxWarriors = _tileSettings.MaxWarriorsEco;
                    break;
                case TileType.war:
                    tile.TileModel.MaxWarriors = _tileSettings.MaxWorkersWar;
                    break;
            }
            
            tile.TileModel.TileType = type;
            _buildingFactory.PlaceCenterBuilding(tile);

            _centerPanelController.DeactivateTileTypeSelection();
            _tilePanelController.LoadInfoToTheUI(tile);
            LoadInfoToTheUI(tile);
            TileTypeChange?.Invoke(tile);
        }

        #endregion
        #region Other
        public void Dispose() { }
        public void LevelCheck()
        {
            bool levelExceedDestroyBuildingInfo = _currentLVL > _tilePanelController.DestroyBuildingInfo.Count;

            _tilePanelController.TileMenu.EnabledStartButton(levelExceedDestroyBuildingInfo);
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
                _notificationPanel.BasicTemporaryUIVisualization("Max LVL", 2);
                return;
            }
            
            TileModel.SaveTileConfig = _tileSettings.Levels[currentLevel];
            TileModel.TileConfig = TileModel.SaveTileConfig;
            TileModel.CurrBuildingConfigs.AddRange(TileModel.SaveTileConfig.BuildingTirs);
                
            
            LoadBuildings(TileModel, TileModel.TileType);
            LevelCheck();
        }
        
        
        
        private bool IsResourcesEnough(TileConfig tileConfig)
        {
            foreach (ResourcePriceModel resourcePriceModel in tileConfig.PriceUpgrade)
            {
                if (!_stock.CheckResourceInStock(resourcePriceModel.ResourceType, resourcePriceModel.Cost))
                {
                    _notificationPanel.BasicTemporaryUIVisualization("you do not have enough resources to buy", 1);
                    return false;
                }
            }
            return true;
        }

        private void LoadBuildingHUD(BuildingHUD info, ICollectable building, BuildingConfig buildingConfig)
        {
            building.Icon = info.Icon.sprite;
            building.BuildingTypes = info.BuildingType;
            info.BuildingID = building.BuildingID;
            info.MaxWorkers = building.MaxWorkers;
        }

        private void UnLoadBuildingHUD(BuildingHUD info)
        {
            info.DestroyBuildingInfo.onClick.RemoveAllListeners();
            info.PlusUnitButton.onClick.RemoveAllListeners();
            info.MinusUnitButton.onClick.RemoveAllListeners();
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