using System;
using System.Collections.Generic;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using Code.UI.CenterPanel;
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
        private ITextVisualizationOnUI _notificationUI;
        private GlobalStock _stock;
        private GameConfig _gameConfig;
        private GeneratorLevelController _levelController;
        private readonly GlobalTileSettings _tileSettings;
        public TowerShotBehavior TowerShot;
        public Damageable MainBuilding { get; private set; }
        //TODO Это временно!!
        public DummyController DummyController;
        private TileView _tileView;

        public BuildingFactory(CenterPanelController centerPanel, GlobalStock stock, 
            GameConfig gameConfig, GeneratorLevelController levelController, GlobalTileSettings tileSettings)
        {
            _notificationUI = centerPanel;
            _stock = stock;
            _gameConfig = gameConfig;

            _levelController = levelController;
            _tileSettings = tileSettings;
            _levelController.SpawnTower += PlaceMainTower;
        }
        
        /// <summary>
        /// Проверяет на наличие ресурса если он есть ставим здание.
        /// </summary>
        public ICollectable BuildBuilding(BuildingConfig buildingConfig, TileModel model, TileController controller)
        {
            if (!IsResourcesEnough(buildingConfig))
            {
                return null;
            }
            
            buildingConfig.BuildingCost.ForEach(resourcePrice => 
                _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost));
            ICollectable building = CreateBuilding(model, buildingConfig);
            OnBuildingsChange?.Invoke(building.BuildingTypes, true);
            return building;
        }
        
        public void DestroyBuilding(ICollectable building, BuildingHUD buildingUI, TileModel model)
        {
            OnBuildingsChange?.Invoke(building.BuildingTypes, false);

            RemoveTypeDots(model, building);
            model.FloodedBuildings.Remove(building);
            
            GameObject.Destroy(building.Prefab);
            GameObject.Destroy(buildingUI.gameObject);
        }

        void AddWorkerPreparation(GameObject building, ICollectable buildingModel)
        {
            if (!building.TryGetComponent(out IWorkerPreparation preporation)) return;

            buildingModel.WorkerPreparation = preporation;
        }

        private ICollectable CreateBuilding(TileModel model, BuildingConfig config)
        {
            var dot = CheckDot(model);
            if (null == dot)
            {
                _notificationUI.BasicTemporaryUIVisualization("You have built maximum buildings", 1);
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

        public void PlaceCenterBuilding(TileView view)
        {
            var instaniatedDummy = Object.Instantiate(_gameConfig.TestBuilding, view.transform.position, Quaternion.identity);
            var dummyController = new DummyController(instaniatedDummy);
            
            _tileView = view;
            view.TileModel.CenterBuilding = dummyController.Dummy;
            DummyController = dummyController;
            
            dummyController.Dummy.OnHealthChanged += HealthChanged;
            dummyController.Spawn(_tileSettings.MaxHealthCenterBuilding);
        }

        private void PlaceMainTower(Dictionary<Vector2Int, VoxelTile> spawnedTiles, ITileSetter tileSetter, Transform pointSpawnUnits)
        {
            var config = _gameConfig.MainTowerConfig as BuildingConfig;
            var firstTile = spawnedTiles[tileSetter.FirstTileGridPosition];
            
            var mainBuilding = Object.Instantiate(config.BuildingPrefab, firstTile.transform.position, Quaternion.identity);
            
            _tileView = firstTile.TileView;
            pointSpawnUnits = mainBuilding.transform;
            
            MainBuilding = mainBuilding.GetComponent<Damageable>();
            TowerShot = mainBuilding.GetComponentInChildren<TowerShotBehavior>();
            
            _tileView.TileModel.CenterBuilding = MainBuilding;
            firstTile.TileView.TileModel.TileType = TileType.All;
            firstTile.TileView.TileModel.MaxWarriors = _tileSettings.MaxWorkersWar;
       
            MainBuilding.Init(_tileSettings.MaxHealthMainTower);
            
            MainBuilding.OnHealthChanged += HealthChanged;
            _levelController.SpawnTower -= PlaceMainTower;
        }
        
        public void RemoveTypeDots(TileModel model, ICollectable building)
        {
            var dot = model.DotSpawns.Find(x => x.Building == building);
            if (dot == null) return;
            
            dot.Building.BuildingTypes = BuildingTypes.None;
            dot.IsActive = true;
        }

        public Dot CheckDot(TileModel model)
        {
            var cleardots = model.DotSpawns.FindAll(x => x.IsActive);
            return cleardots[Random.Range(0, cleardots.Count)];
        }

        private void HealthChanged(float MaxHealh, float CurrentHealth)
        {
            Debug.Log($"<color=aqua>healthChanged: {CurrentHealth}</color>");
        }
        
        public void Dispose()
        {
            MainBuilding.OnHealthChanged -= HealthChanged;
        }

        public void OnUpdate(float deltaTime)
        {
            
        }
    }
}