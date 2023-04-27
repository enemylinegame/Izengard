using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem.ScriptableObjects;
using Code.UI;
using CombatSystem;
using ResourceSystem;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;

namespace Code.TileSystem
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private HouseType _type;
        [SerializeField] private TileConfig _tileConfig;
        [SerializeField] private List<Dot> _dotSpawns;
        
        private List<BuildingConfig> _curBuildingConfigs;
        private List<WorkerView> _workerViews;
        private List<DefenderUnit> _defenderUnits;
        public List<WorkersAssigments> _workersAssigmentses;
        private TileConfig _saveTileConfig;
        private Dictionary<Building, BuildingConfig> _floodedBuildings;
        private List<Mineral> _floodedMinerals;
        private int _eightQuantity;

        public TileConfig TileConfig => _tileConfig;
        public List<BuildingConfig> CurrBuildingConfigs => _curBuildingConfigs;
        public Dictionary<Building, BuildingConfig> FloodedBuildings => _floodedBuildings;
        public List<Mineral> FloodedMinerals => _floodedMinerals;
        public int EightQuantity => _eightQuantity;
        public List<Dot> DotSpawns => _dotSpawns;
        
        
        private void Awake()
        {
            _saveTileConfig = new TileConfig();
            _curBuildingConfigs = new List<BuildingConfig>(_tileConfig.BuildingTirs);
            _floodedBuildings = new Dictionary<Building, BuildingConfig>();
            _floodedMinerals = new List<Mineral>();
            _workerViews = new List<WorkerView>();
            _workersAssigmentses = new List<WorkersAssigments>();
            
            _saveTileConfig = _tileConfig;
            FillWorkerList();
        }
        private void FillWorkerList()
        {
            for (int i = 0; i < _saveTileConfig.MaxUnits.GetHashCode(); i++)
            {
                _workerViews.Add(new WorkerView());
                _workerViews[i].AssignedResource = BuildingTypes.None;
            }
        }
        /// <summary>
        /// Добавление юнита в найм к определенному зданию
        /// </summary>
        public void UpdateWorkerAssigment(BuildingTypes resourceType, Building building)
        {
            if (!_workerViews.Exists(x => x.AssignedResource == BuildingTypes.None))
            {
                return;
            }

            if (!_workersAssigmentses.Exists(x => x.Building.BuildingID == building.BuildingID))
            {
                var workers = new WorkersAssigments(building);
                workers.BusyWorkersCount++;
                _eightQuantity++;
                _workersAssigmentses.Add(workers);
            }
            else
            {
                var workersAssigments =  _workersAssigmentses.Find(x => x.Building.BuildingID == building.BuildingID);
                workersAssigments.BusyWorkersCount++;
                _eightQuantity++;
            }

                var worker = _workerViews.Find(x => x.AssignedResource == BuildingTypes.None);
            worker.AssignedResource = resourceType;
        }
        /// <summary>
        /// Удаление юнита из найма из определенного здания
        /// </summary>
        public void RemoveWorkerAssigment(BuildingTypes resourceType, Building building)
        {
            if (!_workerViews.Exists(x => x.AssignedResource == resourceType))
            {
                return;
            }
            if (_workersAssigmentses.Exists(x => x.Building.BuildingID == building.BuildingID))
            {
                var workersAssigments =  _workersAssigmentses.Find(x => x.Building.BuildingID == building.BuildingID);
                workersAssigments.BusyWorkersCount--;
                _eightQuantity--;
                if (workersAssigments.BusyWorkersCount < 0) workersAssigments.BusyWorkersCount = 0;
                
            }
            var worker = _workerViews.Find(x => x.AssignedResource == resourceType);
            worker.AssignedResource = BuildingTypes.None;
        }
        
        public void RemoveAllWorkerAssigment(BuildingTypes resourceType, Building building, TileController controller)
        {
            if (!_workerViews.Exists(x => x.AssignedResource == resourceType))
            {
                return;
            }
            if (_workersAssigmentses.Exists(x => x.Building.BuildingID == building.BuildingID))
            {
                var workersAssigments =  _workersAssigmentses.Find(x => x.Building.BuildingID == building.BuildingID);
                controller.RemoveFromHiringUnits(workersAssigments.BusyWorkersCount);
                _eightQuantity -= workersAssigments.BusyWorkersCount;
                workersAssigments.BusyWorkersCount = 0;
                if (workersAssigments.BusyWorkersCount < 0) workersAssigments.BusyWorkersCount = 0;
                
            }
            var worker = _workerViews.Find(x => x.AssignedResource == resourceType);
            worker.AssignedResource = BuildingTypes.None;
        }
        /// <summary>
        /// получение информации о нанятых юнитов для определенного здания
        /// </summary>
        /// <returns></returns>
        public int GetAssignedWorkers(Building building)
        {
            if (!_workersAssigmentses.Exists(x => x.Building.BuildingID == building.BuildingID))
            {
                return 0;
            }
            return _workersAssigmentses.
                Find(x => x.Building.BuildingID == building.BuildingID).BusyWorkersCount;
        }
        
        /// <summary>
        /// Увеличение уровня тайла
        /// </summary>
        public void LVLUp(TileController controller)
        {
            if (_saveTileConfig.TileLvl.GetHashCode() < 5)
            {
                _saveTileConfig = controller.List.LVLList[_saveTileConfig.TileLvl.GetHashCode()];
                _tileConfig = _saveTileConfig;
                _curBuildingConfigs.AddRange(_saveTileConfig.BuildingTirs);
                controller.UpdateInfo(_saveTileConfig);
                controller.ADDBuildUI(_curBuildingConfigs, this);
                FillWorkerList();
            }else controller.CenterText.NotificationUI("Max LVL", 1000);
        }
        /// <summary>
        /// Загрузка сохраненного блока информации определеного здания и загрузка иго в UI
        /// </summary>
        public void LoadButtonsUIBuy(TileController controller, UIController uiController)
        {
            foreach (var building in _floodedBuildings)
            {
                uiController.LoadBuildingInfo(building.Key, GetAssignedWorkers(building.Key), building, controller);
            }
        }
    }
    public class WorkersAssigments
    {
        public Building Building;
        public int BusyWorkersCount;

        public WorkersAssigments(Building building, int busyWorkersCount = 0)
        {
            Building = building;
            BusyWorkersCount = busyWorkersCount;
        }
    }
}