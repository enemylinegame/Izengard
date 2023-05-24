﻿using System;
using System.Collections.Generic;
using Code.BuldingsSystem;
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
        private BaseNotificationUI _notificationUI;
        private GlobalStock _stock;
        public BuildingController(UIController uiController, GlobalStock stock)
        {
            _uiController = uiController;
            _notificationUI = uiController.CenterUI.BaseNotificationUI;
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
            buildingConfig.BuildingCost.ForEach(resourcePrice => _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost));

            ICollectable building = StartBuilding(model, buildingConfig);

            if (building == null) return;

            building.InitBuilding();

            BuildingUIInfo info = controller.CreateBuildingInfo(buildingConfig, model, building);

            building.Icon = info.Icon.sprite;
            building.BuildingTypes = info.BuildingType;
    
            _uiController.ButtonsBuy.Add(buildingConfig);
            model.FloodedBuildings.Add(building);
            
            controller.LevelCheck();
        }
        
        public void DestroyBuilding(List<ICollectable> buildings, BuildingUIInfo buildingUI, TileModel model, TileController tileController)
        {
            var buildingToRemove = buildings.Find(kvp => kvp.BuildingTypes == buildingUI.BuildingType);
            
            buildingUI.DestroyBuildingInfo.onClick.RemoveAllListeners();
            buildingUI.PlusUnit.onClick.RemoveAllListeners();
            buildingUI.MinusUnit.onClick.RemoveAllListeners();
                
            _uiController.DestroyBuildingInfo.Remove(buildingUI.gameObject);
            tileController.WorkerMenager.RemoveAllWorkerAssignment(buildingUI, buildingToRemove, tileController);
            RemoveTypeDots(model, buildingToRemove);
                
            buildings.Remove(buildingToRemove);
            model.FloodedBuildings.Remove(buildingToRemove);
                
            GameObject.Destroy(buildingToRemove.Prefab);
            GameObject.Destroy(buildingUI.gameObject);
                
            tileController.LevelCheck();
        }
        private ICollectable StartBuilding(TileModel model, BuildingConfig config)
        {
            var dot = CheckDot(model);
            if (dot == null)
            {
                _notificationUI.BasicTemporaryUIVisualization("You have built maximum buildings", 1000);
                return null;
            }

            var buildingPrefab = config.BuildingPrefab.GetComponent<Building>();
            
            var build = Object.Instantiate(buildingPrefab, dot.transform);
            
            build.BuildingTypes = config.BuildingType;
            
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
            return cleardots[UnityEngine.Random.Range(0, cleardots.Count)];
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
    }
}