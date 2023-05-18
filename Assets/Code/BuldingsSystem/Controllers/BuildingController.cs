using System;
using System.Collections.Generic;
using Code.BuldingsSystem.ScriptableObjects;
using Code.TileSystem;
using Code.TileSystem.Interfaces;
using Code.UI;
using Controllers;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Code.BuildingSystem
{
    public class BuildingController
    {
        private UIController _uiController;
        private BaseCenterText _centerText;
        private GlobalStock _stock;
        public BuildingController(UIController uiController, GlobalStock stock)
        {
            _uiController = uiController;
            _centerText = uiController.CenterUI.BaseCenterText;
            _stock = stock;
            _stock.AddResourceToStock(ResourceType.Wood, 100);
            _stock.AddResourceToStock(ResourceType.Iron, 100);
            _stock.AddResourceToStock(ResourceType.Deer, 100);
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

            foreach (var resourcePrice in buildingConfig.BuildingCost)
            {
                _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost);
            }
            var building = StartBuilding(model, buildingConfig);
            if (building)
            {
                var info = controller.CreateBuildingInfo(buildingConfig, model, building);
                building.Icon = info.Icon;
                building.BuildingTypes = info.Types;
                building.InitBuilding();
                
                _uiController.ButtonsBuy.Add(buildingConfig);
                model.FloodedBuildings.Add(building);
            }
        }

        public void DestroyBuilding(List<Building> buildingConfigs, BuildingUIInfo Button, TileModel model, WorkerAssignmentsController workerAssignmentsController)
        {
            foreach (var kvp in buildingConfigs)
            {
                if (kvp.BuildingTypes == Button.Types)
                {
                    Button.DestroyBuildingInfo.onClick.RemoveAllListeners();
                    Button.PlusUnit.onClick.RemoveAllListeners();
                    Button.MinusUnit.onClick.RemoveAllListeners();
                    buildingConfigs.Remove(kvp);
                    
                    _uiController.DestroyBuildingInfo.Remove(Button.gameObject);
                    model.FloodedBuildings.Remove(kvp);
                    // workerAssignmentsController.RemoveAllWorkerAssigment(Button.Types, kvp);
                    RemoveTypeDots(model, kvp);
                    GameObject.Destroy(kvp.gameObject);
                    GameObject.Destroy(Button.gameObject);
                    break;
                }

            }
        }
        private Building StartBuilding(TileModel model, BuildingConfig config)
        {
            if (CheckDot(model))
            {
                var dot = CheckDot(model);
                var build = Object.Instantiate(config.BuildingPrefab.GetComponent<Building>(), dot.transform);
                build.BuildingTypes = config.BuildingType;
                dot.IsActive = false;
                return build;
            }
            _centerText.NotificationUI("You have built maximum buildings", 1000);
            return null;
        }
        public void RemoveTypeDots(TileModel model, Building building)
        {
            if (model.DotSpawns.Exists(x => x.Building == building))
            {
                var dot = model.DotSpawns.Find(x => x.Building == building);
                dot.Building.BuildingTypes = BuildingTypes.None;
                dot.IsActive = true;
            }
        }
        public Dot CheckDot(TileModel model)
        {
            var cleardots = model.DotSpawns.FindAll(x => x.IsActive);
            return cleardots[UnityEngine.Random.Range(0, cleardots.Count)];
        }
        
        private bool IsResourcesEnough(BuildingConfig buildingConfig)
        {
            foreach (ResourcePriceModel resourcePriceModel in buildingConfig.BuildingCost)
            {
                if (_stock.CheckResourceInStock(resourcePriceModel.ResourceType, resourcePriceModel.Cost))
                {
                    _centerText.NotificationUI("you do not have enough resources to buy", 1000);
                    return false;
                }
            }
            return true;
        }
    }
}