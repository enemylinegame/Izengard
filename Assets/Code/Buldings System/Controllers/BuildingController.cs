using System;
using System.Collections.Generic;
using Code.TileSystem;
using Code.UI;
using CombatSystem;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.BuildingSystem
{
    public class BuildingController: IDisposable
    {
        private UIController _uiController;
        private ITextVisualizationOnUI _notificationUI;
        private GlobalStock _stock;
        private GameConfig _gameConfig;
        
        private readonly HashSet<DummyController> _instantiatedDummys = new HashSet<DummyController>();

        public BuildingController(UIController uiController, GlobalStock stock, 
            GameConfig gameConfig, GeneratorLevelController levelController)
        {
            _uiController = uiController;
            _notificationUI = uiController.CenterUI.BaseNotificationUI;
            _stock = stock;
            _gameConfig = gameConfig;
            _stock.AddResourceToStock(ResourceType.Wood, 100);
            _stock.AddResourceToStock(ResourceType.Iron, 100);
            _stock.AddResourceToStock(ResourceType.Deer, 100);
            
            levelController.OnCombatPhaseStart += RespawnDummies;
        }
        
        /// <summary>
        /// Проверяет на наличие ресурса если он есть ставим здание.
        /// </summary>
        public void BuildBuilding(BuildingConfig buildingConfig, 
            TileModel model, TileController controller)
        {
            if (!IsResourcesEnough(buildingConfig))
            {
                return;
            }
            buildingConfig.BuildingCost.ForEach(resourcePrice => _stock.GetResourceFromStock(
                resourcePrice.ResourceType, resourcePrice.Cost));

            ICollectable building = CreateBuilding(model, buildingConfig);

            if (building == null) return;

            building.InitBuilding();

            BuildingUIInfo info = controller.CreateBuildingInfo(buildingConfig, model, building);

            building.Icon = info.Icon.sprite;
            building.BuildingTypes = info.BuildingType;
            info.BuildingID = building.BuildingID;
    
            _uiController.ButtonsBuy.Add(buildingConfig);
            model.FloodedBuildings.Add(building);
            
            controller.LevelCheck();
        }
        
        public void DestroyBuilding(List<ICollectable> buildings, BuildingUIInfo buildingUI, TileModel model, TileController tileController)
        {
            var buildingToRemove = buildings.Find(kvp => kvp.BuildingID == buildingUI.BuildingID);
            
            buildingUI.DestroyBuildingInfo.onClick.RemoveAllListeners();
            buildingUI.PlusUnit.onClick.RemoveAllListeners();
            buildingUI.MinusUnit.onClick.RemoveAllListeners();
                
            _uiController.DestroyBuildingInfo.Remove(buildingUI.gameObject);
            tileController.WorkerMenager.StopAllProductions(
                buildingToRemove);

            RemoveTypeDots(model, buildingToRemove);
                
            buildings.Remove(buildingToRemove);
            model.FloodedBuildings.Remove(buildingToRemove);
                
            GameObject.Destroy(buildingToRemove.Prefab);
            GameObject.Destroy(buildingUI.gameObject);
                
            tileController.LevelCheck();
        }
        private ICollectable CreateBuilding(TileModel model, BuildingConfig config)
        {
            var dot = CheckDot(model);
            if (dot == null)
            {
                _notificationUI.BasicTemporaryUIVisualization("You have built maximum buildings", 1);
                return null;
            }

            var buildingPrefab = config.BuildingPrefab.GetComponent<Building>();
            
            var build = Object.Instantiate(buildingPrefab, dot.transform);
            
            build.BuildingTypes = config.BuildingType;
            build.MaxWorkers = config.MaxWorkers;
            build.BuildingTypes = config.BuildingType;
            build.NameBuiding = config.Name;
            
            dot.Building = build;
            dot.IsActive = false;
            
            return build;
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
        
        private bool IsResourcesEnough(BuildingConfig buildingConfig)
        {
            foreach (ResourcePriceModel resourcePriceModel in buildingConfig.BuildingCost)
            {
                if (_stock.CheckResourceInStock(resourcePriceModel.ResourceType, resourcePriceModel.Cost))
                {
                    _notificationUI.BasicTemporaryUIVisualization("you do not have enough resources to buy", 1000);
                    return false;
                }
            }
            return true;
        }
        
        private void RespawnDummies()
        {
            foreach (var dummy in _instantiatedDummys) dummy.Spawn();
        }

        public void PlaceCenterBuilding(TileView view)
        {
            var instaniatedDummy = Object.Instantiate(_gameConfig.TestBuilding, view.transform.position, Quaternion.identity);
            var dummyController = new DummyController(instaniatedDummy);
            _instantiatedDummys.Add(dummyController);
            foreach (var dummy in _instantiatedDummys) dummy.Spawn();
        }
        
        public void Dispose()
        {
            // _levelGenerator.SpawnResources -= OnNewTile;
            foreach (var dummyController in _instantiatedDummys) dummyController.Dispose();
        }
    }
}