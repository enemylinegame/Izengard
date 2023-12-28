using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UserInputSystem;
using Object = UnityEngine.Object;

namespace NewBuildingSystem
{
    public class BuildingsController
    {
        private readonly BuildingsSettings _settings;
        private readonly List<Building> _buildings;
        private readonly GameObject _indicator;
        private readonly BuildingInfoPanelController _buildingInfoPanel;
        private Building _currentBuilding;

        public BuildingsController(BuildingsSettings settings, List<Building> buildings, GameObject indicator,
            BuildingInfoPanelController buildingInfoPanel, RayCastController controller)
        {
            _settings = settings;
            _buildings = buildings;
            _indicator = indicator;
            _buildingInfoPanel = buildingInfoPanel;
            controller.LeftClick += SelectedBuild;
        }

        public void Init(Building building)
        {
            _currentBuilding = building;

            InstallInCenterTile(building.Size);
            CreateIndicators(building.Size);

            switch (building.BuildingsType)
            {
                case EnumBuildings.Build:
                    break;
                case EnumBuildings.Res:
                    _buildingInfoPanel.ActivationErrorPanel(true);
                    ResourcesBuilding();
                    break;
                case EnumBuildings.Processing:
                    break;
                case EnumBuildings.Mining:
                    break;
            }
        }

        private void SelectedBuild(string ID)
        {
            if (_currentBuilding == null)
                return;

            if (_currentBuilding.BuildingsType == EnumBuildings.Res)
            {
                _currentBuilding.OnResources -= SearchRes;
                _currentBuilding.ColliderArea.enabled = false;
            }
            _buildingInfoPanel.PlusButton -= AddWorkers;
            _buildingInfoPanel.MinusButton -= RmWorkers;
            _buildingInfoPanel.ChangeUnitsCount(0, 0);
            _buildingInfoPanel.ActivationErrorPanel(false);
            
            var build = _buildings.Find(x => ID == x.ID);
            if(build == null) return;
            _currentBuilding = build;

            _buildingInfoPanel.EnabledPanel(true);
            _buildingInfoPanel.SetBuildingInfo(build.Name);
            _buildingInfoPanel.SetBuildingImage(build.Image);
            _buildingInfoPanel.ChangeUnitsCount(build.CurrentCountWorkers, build.MaxCountWorkers);
            _buildingInfoPanel.PlusButton += AddWorkers;
            _buildingInfoPanel.MinusButton += RmWorkers;
            switch (build.BuildingsType)
            {
                case EnumBuildings.Build:
                    break;
                case EnumBuildings.Res:
                    _buildingInfoPanel.ActivationErrorPanel(true);
                    ResourcesBuilding();
                    break;
                case EnumBuildings.Processing:
                    break;
                case EnumBuildings.Mining:
                    break;
            }
        }
        private void ResourcesBuilding()
        {
            SetSizeColliderArea(new Vector2Int(_settings.interactiveArea, _settings.interactiveArea));
            _currentBuilding.OnResources += SearchRes;
            _currentBuilding.ColliderArea.enabled = true;
            
        }
        private void AddWorkers()
        {
            if(_currentBuilding.CurrentCountWorkers >= _currentBuilding.MaxCountWorkers) return;
            _currentBuilding.CurrentCountWorkers++;
            _buildingInfoPanel.ChangeUnitsCount(_currentBuilding.CurrentCountWorkers, _currentBuilding.MaxCountWorkers);
        }
        private void RmWorkers()
        {
            if (_currentBuilding.CurrentCountWorkers <= 0) return;
            _currentBuilding.CurrentCountWorkers--;
            _buildingInfoPanel.ChangeUnitsCount(_currentBuilding.CurrentCountWorkers, _currentBuilding.MaxCountWorkers);
        }
        private void InstallInCenterTile(Vector2Int size)
        {
            _currentBuilding.Size = size;
            var sizeX = size.x / 2f;
            var sizeY = size.y / 2f;
            
            _currentBuilding.PositionBuild.position = new Vector3(sizeX, 0, sizeY);
            _currentBuilding.Collider.center = new Vector3(sizeX, .2f, sizeY);
            _currentBuilding.Collider.size = new Vector3(size.x -.5f , .2f, size.y -.5f);
        }
        private void SetSizeColliderArea(Vector2Int size)
        {
            var sizeX = _currentBuilding.Size.x + size.x;
            var sizeY = _currentBuilding.Size.y + size.y;
            
            _currentBuilding.ColliderArea.center = new Vector3(_currentBuilding.Size.x / 2f , .2f, _currentBuilding.Size.y / 2f);
            _currentBuilding.ColliderArea.size = new Vector3(sizeX - .3f, .2f, sizeY - .3f);
            _currentBuilding.ColliderArea.enabled = false;

        }
        private void CreateIndicators(Vector2Int size)
        {
            List<GameObject> cellIndicators = new();
            for (float x = 0; x < size.x; x++)
            {
                for (float y = 0; y < size.y; y++)
                {
                    if (cellIndicators.Find(ind => ind.transform.position 
                                                   != new Vector3(x, 0, y)) || cellIndicators.Count == 0)
                    {
                        var indicator = Object.Instantiate(_indicator, _currentBuilding.ObjectBuild.transform);
                        indicator.transform.position = new Vector3(x, 0, y);
                        cellIndicators.Add(indicator);
                    }
                    
                }
            }
            if(cellIndicators.Count == size.x * size.y) cellIndicators.Clear();
        }
        private void SearchRes(Building building)
        {
            _buildingInfoPanel.ActivationErrorPanel(false);
        }

        private void AddResToTheNextWave(Building building, string ResType)
        {
            // Проводим обработку ресурсов на основе загруженных рабочих
            int totalResources = building.CurrentCountWorkers * 3;

            for (int i = 0; i < totalResources; i++)
            {
                Debug.Log($"Processed resources: {ResType}");
            }
            
            // Обнуляем количество рабочих после обработки
            building.CurrentCountWorkers = 0;

        }
    }
}