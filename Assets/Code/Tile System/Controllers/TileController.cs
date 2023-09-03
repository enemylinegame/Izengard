using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.UI;
using Code.Player;
using ResourceSystem;
using ResourceSystem.SupportClases;
using Views.BuildBuildingsUI;
using Debug = UnityEngine.Debug;

namespace Code.TileSystem
{
    public class TileController : IDisposable, IOnController, IOnTile, ITileLoadInfo, IOnUpdate
    {
        #region Fields

        public event Action<TileView, TileModel> TileTypeChange;
        
        private TileView _tileView;
        private readonly ButtonsControllerOnTile _buttonsController;
        private readonly ProductionManager _productionManager;
        private readonly LevelOfLifeButtonsCustomizer _level;
        private readonly buildingsPanelController _buildingsPanel;
        private readonly BuildingFactory _buildingFactory;
        private readonly GlobalTileSettings _tileSettings;
        private readonly TileGenerator _tileGenerator;
        private readonly GlobalStock _stock;
        private int _currentLVL;
        
        private readonly CenterPanelController _centerPanelController;
        private readonly TilePanelController _tilePanelController;
        private readonly TileMainBoardController _tileMainBoard;
        private readonly NotificationPanelController _notificationPanel;
        public ProductionManager WorkerMenager => _productionManager;
        public TileModel TileModel;
        public TileView View => _tileView;

        #endregion
        public TileController(UIPanelsInitialization uiPanelsInitialization, BuildingFactory buildingController, 
            InputController inputController, ProductionManager productionManager, 
            LevelOfLifeButtonsCustomizer level, GlobalStock stock, GlobalTileSettings tileSettings, TileGenerator tileGenerator)
        {
            _buttonsController = new ButtonsControllerOnTile(uiPanelsInitialization.TileMainBoard, inputController);
            _buildingsPanel = new buildingsPanelController(buildingController, productionManager, uiPanelsInitialization);
            
            
            _productionManager = productionManager;
            _centerPanelController = uiPanelsInitialization.CenterPanelController;
            _tilePanelController = uiPanelsInitialization.TilePanelController;
            _tileMainBoard = uiPanelsInitialization.TileMainBoard;
            _notificationPanel = uiPanelsInitialization.NotificationPanel;
            _buildingFactory = buildingController;
            _level = level;
            _stock = stock;
            _tileSettings = tileSettings;
            _tileGenerator = tileGenerator;

            inputController.Add(this);
        }

        #region LoadAndUnloadTile
        public void LoadInfoToTheUI(TileView tile, TileModel model)
        {
            if (model.TileType == TileType.None)
            {
                TileTypeCheck(model, tile);
                return;
            } 
            _tileView = tile;
            TileModel = model;
            //TODO: 
            _buildingsPanel.LoadBuildings(model, WorkerHiring, WorkerDismissal, LevelCheck, ResetWorkersAccount);
            _buttonsController.ButtonAddListener(model, _level);
            _tileMainBoard.HolderButton(ButtonTypes.Upgrade).onClick.AddListener(LVLUp);
            _tileMainBoard.LoadAllTextsFieldsAndImaged(model.TileConfig, TileModel, out _currentLVL);
            _buildingsPanel.LoadFloodedBuildings(model, WorkerHiring, WorkerDismissal, LevelCheck, ResetWorkersAccount);
            LevelCheck();

        }

        public void Cancel()
        {
            _tileMainBoard.RemoveListeners(ButtonTypes.All, TileModel);
        }
        #endregion
        #region BuildingBuy
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
        #endregion
        #region TileTypeChange
        
        private void TileTypeCheck(TileModel model, TileView view)
        {
            if(model.TileType != TileType.None && model.TileType == TileType.All) return;
            
            _centerPanelController.ActivateTileTypeSelection((() => SetTileType(TileType.Eco, model, view)),
                () => SetTileType(TileType.war, model, view));
        }
        private void SetTileType(TileType type, TileModel model, TileView tile)
        {
            switch (type)
            {
                case TileType.Eco:
                    model.MaxWarriors = _tileSettings.MaxWarriorsEco;
                    break;
                case TileType.war:
                    model.MaxWarriors = _tileSettings.MaxWorkersWar;
                    break;
            }
            
            model.TileType = type;
            _buildingFactory.PlaceCenterBuilding(tile, model);

            _centerPanelController.DeactivateTileTypeSelection();
            _tilePanelController.LoadInfoToTheUI(tile, model);
            TileTypeChange?.Invoke(tile, model);
            LoadInfoToTheUI(tile, model);
        }

        #endregion
        #region Other
        public void Dispose() { }

        private void LevelCheck()
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
                
            
            _buildingsPanel.LoadBuildings(TileModel, WorkerHiring, WorkerDismissal, LevelCheck, ResetWorkersAccount);
            LoadInfoToTheUI(_tileView, TileModel);
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