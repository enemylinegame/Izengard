using System;
using System.Collections.Generic;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using CombatSystem;
using LevelGenerator.Interfaces;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.BuildingSystem
{
    public class BuildingFactory: IDisposable, IOnController, IOnUpdate
    {
        public event Action<BuildingTypes, bool> OnBuildingsChange;
        
        private UIController _uiController;
        private ITextVisualizationOnUI _notificationUI;
        private GlobalStock _stock;
        private GameConfig _gameConfig;
        private GeneratorLevelController _levelController;
        private readonly GlobalTileSettings _tileSettings;
        public TowerShotBehavior TowerShot;
        
        
        public Damageable MainBuilding { get; private set; }
        private readonly HashSet<DummyController> _instantiatedDummys = 
            new HashSet<DummyController>();

        
        //TODO Это временно!!
        public DummyController DummyController;
        private TileView _tileView;

        public BuildingFactory(UIController uiController, GlobalStock stock, 
            GameConfig gameConfig, GeneratorLevelController levelController, GlobalTileSettings tileSettings)
        {
            _uiController = uiController;
            _notificationUI = uiController.CenterUI.BaseNotificationUI;
            _stock = stock;
            _gameConfig = gameConfig;

            //_stock.AddResourceToStock(ResourceType.Wood, 100);
            //_stock.AddResourceToStock(ResourceType.Iron, 100);
            //_stock.AddResourceToStock(ResourceType.Deer, 100);

            _levelController = levelController;
            _tileSettings = tileSettings;
            //_levelController.OnCombatPhaseStart += RespawnDummies;
            _levelController.SpawnTower += PlaceMainTower;
        }
        
        /// <summary>
        /// Проверяет на наличие ресурса если он есть ставим здание.
        /// </summary>
        public void BuildBuilding(BuildingConfig buildingConfig, TileModel model, TileController controller)
        {
            if (!IsResourcesEnough(buildingConfig))
            {
                return;
            }
            buildingConfig.BuildingCost.ForEach(resourcePrice => 
                _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost));

            ICollectable building = CreateBuilding(model, buildingConfig);

            if (building == null) return;

            building.InitBuilding();

            BuildingUIInfo info = controller.CreateBuildingInfo(buildingConfig, building);

            building.Icon = info.Icon.sprite;
            building.BuildingTypes = info.BuildingType;
            info.BuildingID = building.BuildingID;
            info.MaxWorkers = building.MaxWorkers;
    
            _uiController.ButtonsBuy.Add(buildingConfig);
            model.FloodedBuildings.Add(building);
            
            OnBuildingsChange?.Invoke(building.BuildingTypes, true);

            controller.LevelCheck();
        }
        
        public void DestroyBuilding(List<ICollectable> buildings, BuildingUIInfo buildingUI, TileModel model, 
            TileController tileController)
        {
            var buildingToRemove = buildings.Find(kvp => kvp.BuildingID == buildingUI.BuildingID);
            
            buildingUI.DestroyBuildingInfo.onClick.RemoveAllListeners();
            buildingUI.PlusUnitButton.onClick.RemoveAllListeners();
            buildingUI.MinusUnitButton.onClick.RemoveAllListeners();
                
            _uiController.DestroyBuildingInfo.Remove(buildingUI.gameObject);
            tileController.WorkerMenager.StopAllProductions(
                buildingToRemove);

            OnBuildingsChange?.Invoke(buildingToRemove.BuildingTypes, false);

            RemoveTypeDots(model, buildingToRemove);
                
            buildings.Remove(buildingToRemove);
            model.FloodedBuildings.Remove(buildingToRemove);
            
            GameObject.Destroy(buildingToRemove.Prefab);
            GameObject.Destroy(buildingUI.gameObject);
                
            tileController.LevelCheck();
        }

        void AddWorkerPreparation(GameObject building, ICollectable buildingModel)
        {
            if (!building.TryGetComponent(out IWorkerPreparation preporation))
                return;

            buildingModel.WorkerPreparation = preporation;
        }

        private ICollectable CreateBuilding(TileModel model, BuildingConfig config)
        {
            var dot = CheckDot(model);
            if (null == dot)
            {
                _notificationUI.BasicTemporaryUIVisualization(
                    "You have built maximum buildings", 1);
                return null;
            }
            
            var building = Object.Instantiate(config.BuildingPrefab, dot.transform);
            if (!building.TryGetComponent(out ICollectable buildingModel))
            {
                Debug.LogError("Building does't has component ICollectable");
                return null;
            }

            buildingModel.BuildingTypes = config.BuildingType;
            buildingModel.MaxWorkers = config.MaxWorkers;
            buildingModel.BuildingTypes = config.BuildingType;
            buildingModel.Name = config.Name;
            buildingModel.VisibleName = config.Name;
            buildingModel.ResourceType = config.Resource;
            buildingModel.SpawnPosition = dot.transform.position;

            AddWorkerPreparation(building, buildingModel);

            dot.Building = buildingModel;
            dot.IsActive = false;
            
            return buildingModel;
        }

        public void RemoveTypeDots(TileModel model, ICollectable building)
        {
            var dot = model.DotSpawns.Find(x => x.Building == building);
            if (dot == null) return;
            
            dot.Building.BuildingTypes = BuildingTypes.None;
            dot.IsActive = true;
        }

        public PlaceOfProduction CheckDot(TileModel model)
        {
            var cleardots = model.DotSpawns.FindAll(x => x.IsActive);
            return cleardots[Random.Range(0, cleardots.Count)];
        }
        
        private bool IsResourcesEnough(BuildingConfig buildingConfig)
        {
            foreach (ResourcePriceModel resourcePriceModel in buildingConfig.BuildingCost)
            {
                if (!_stock.CheckResourceInStock(resourcePriceModel.ResourceType, resourcePriceModel.Cost))
                {
                    _notificationUI.BasicTemporaryUIVisualization("you do not have enough resources to buy", 1);
                    return false;
                }
            }
            return true;
        }
        
        // private void RespawnDummies()
        // {
        //     foreach (var dummy in _instantiatedDummys) dummy.Spawn();
        // }

        public void PlaceCenterBuilding(TileView view)
        {
            var instaniatedDummy = Object.Instantiate(_gameConfig.TestBuilding, view.transform.position, Quaternion.identity);
            var dummyController = new DummyController(instaniatedDummy);
            _instantiatedDummys.Add(dummyController);
            _tileView = view;
            view.TileModel.CenterBuilding = dummyController.Dummy;
            DummyController = dummyController;
            dummyController.Dummy.OnHealthChanged += HealthChanged;
            foreach (var dummy in _instantiatedDummys) dummy.Spawn(_tileSettings.MaxHealthCenterBuilding);
        }

        public void PlaceMainTower(Dictionary<Vector2Int, VoxelTile> spawnedTiles, ITileSetter tileSetter, Transform pointSpawnUnits)
        {
            var config = _gameConfig.MainTowerConfig as BuildingConfig;

            var firstTile = spawnedTiles[tileSetter.FirstTileGridPosition];
            var mainBuilding = Object.Instantiate(config.BuildingPrefab, firstTile.transform.position, Quaternion.identity);
            _tileView = firstTile.TileView;
            pointSpawnUnits = mainBuilding.transform;
            MainBuilding = mainBuilding.GetComponent<Damageable>();
            _tileView.TileModel.CenterBuilding = MainBuilding;
            if (mainBuilding != null)
            {
                TowerShot = mainBuilding.GetComponentInChildren<TowerShotBehavior>();
                firstTile.TileView.TileModel.TileType = TileType.All;
                firstTile.TileView.TileModel.MaxWarriors = _tileSettings.MaxWorkersWar;
                MainBuilding.OnHealthChanged += HealthChanged;
            }
       
            MainBuilding.Init(_tileSettings.MaxHealthMainTower);
            _levelController.SpawnTower -= PlaceMainTower;
        }

        private void HealthChanged(float MaxHealh, float CurrentHealth)
        {
            Debug.Log($"<color=aqua>healthChanged: {CurrentHealth}</color>");
        }
        
        public void Dispose()
        {
            //_levelController.OnCombatPhaseStart -= RespawnDummies;
            // _levelGenerator.SpawnResources -= OnNewTile;
            foreach (var dummyController in _instantiatedDummys) dummyController.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            
        }
    }
}