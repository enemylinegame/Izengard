using System;
using System.Collections.Generic;
using Controllers.BuildBuildingsUI;
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
        [SerializeField] private DotSpawns _dotSpawns;
        [SerializeField] private List<BuildingConfig> _curBuildingConfigs;
        [SerializeField] private List<Building> _floodedBuildings = new List<Building>();
        private List<WorkerView> _workerViews;
        public List<WorkersAssigments> _workersAssigmentses;
        private int _eightQuantity;
        private TileConfig _saveTileConfig;

        public TileConfig TileConfig => _tileConfig;
        public List<BuildingConfig> CurrBuildingConfigs => _curBuildingConfigs;
        public List<Building> FloodedBuildings => _floodedBuildings;
        public int EightQuantity
        {
            get => _eightQuantity;
            set => _eightQuantity = value;
        }

        public DotSpawns DotSpawns => _dotSpawns;

        private void Start()
        {
            _saveTileConfig = new TileConfig();
            _saveTileConfig = _tileConfig;
            _curBuildingConfigs = new List<BuildingConfig>(_tileConfig.BuildingTirs);
            _workerViews = new List<WorkerView>();
            
            _workersAssigmentses = new List<WorkersAssigments>();
            FillWorkerList();
        }
        
        private void FillWorkerList()
        {
            for (int i = 0; i < _saveTileConfig.MaxUnits.GetHashCode(); i++) //TODO: read from config
            {
                _workerViews.Add(new WorkerView());
                _workerViews[i].AssignedResource = BuildingTypes.None;
            }
        }

        public void UpdateWorkerAssigment(BuildingTypes resourceType, Building building)
        {
            if (!_workerViews.Exists(x => x.AssignedResource == BuildingTypes.None))
            {
                return;
            }

            if (!_workersAssigmentses.Exists(x => x.Building.CurrentBuildingID == building.CurrentBuildingID))
            {
                var workers = new WorkersAssigments(building);
                workers.BusyWorkersCount++;
                _eightQuantity++;
                _workersAssigmentses.Add(workers);
                Debug.Log(_workersAssigmentses.Count);
            }
            else
            {
                var workersAssigments =  _workersAssigmentses.Find(x => x.Building.CurrentBuildingID == building.CurrentBuildingID);
                workersAssigments.BusyWorkersCount++;
                _eightQuantity++;
            }

                var worker = _workerViews.Find(x => x.AssignedResource == BuildingTypes.None);
            worker.AssignedResource = resourceType;
        }

        public void RemoveWorkerAssigment(BuildingTypes resourceType, Building building)
        {
            if (!_workerViews.Exists(x => x.AssignedResource == resourceType))
            {
                return;
            }
            if (_workersAssigmentses.Exists(x => x.Building.CurrentBuildingID == building.CurrentBuildingID))
            {
                var workersAssigments =  _workersAssigmentses.Find(x => x.Building.CurrentBuildingID == building.CurrentBuildingID);
                workersAssigments.BusyWorkersCount--;
                _eightQuantity--;
                if (workersAssigments.BusyWorkersCount < 0) workersAssigments.BusyWorkersCount = 0;
                
            }
            var worker = _workerViews.Find(x => x.AssignedResource == resourceType);
            worker.AssignedResource = BuildingTypes.None;
        }

        public int GetAssignedWorkers(Building building)
        {
            if (!_workersAssigmentses.Exists(x => x.Building.CurrentBuildingID == building.CurrentBuildingID))
            {
                return 0;
            }
            return _workersAssigmentses.
                Find(x => x.Building.CurrentBuildingID == building.CurrentBuildingID).BusyWorkersCount;
        }
        
        
        public void LVLUp(TileUIController controller)
        {
            if (_saveTileConfig.TileLvl.GetHashCode() < 5)
            {
                _saveTileConfig = controller.List.LVLList[_saveTileConfig.TileLvl.GetHashCode()];
                _tileConfig = _saveTileConfig;
                _curBuildingConfigs.AddRange(_saveTileConfig.BuildingTirs);
                controller.UpdateInfo(_saveTileConfig);
                controller.ADDBuildUI(_curBuildingConfigs, this);
            }else controller.CenterText.NotificationUI("Max LVL", 1000);
        }

        public void LoadButtonsUIBuy(TileUIController controller)
        {
            foreach (var building in _floodedBuildings)
            {
                controller.BuildingsUIView.LoadBuildingInfo(building, GetAssignedWorkers(building), controller.DestroyBuilding, controller);
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