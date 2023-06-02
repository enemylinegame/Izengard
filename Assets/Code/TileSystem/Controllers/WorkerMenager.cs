using System.Collections.Generic;
using Code.BuldingsSystem;
using Code.TileSystem.Interfaces;
using Code.UI;
using ResourceSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class WorkerMenager
    {
        private TileUIView _uiView;
        private TileController _tileController;
        private TileModel _tileModel => _tileController.TileModel;
        private TileConfig _saveTileConfig => _tileModel.SaveTileConfig;
        private List<WorkerView> _workerViews => _tileModel.Workers;
        private List<WorkersAssigments> _workersAssigments => _tileModel.WorkersAssigments;

        public WorkerMenager(TileController tileController, TileUIView uiController)
        {
            _uiView = uiController;
            _tileController = tileController;
        }

        public void FillWorkerList()
        {
            _uiView.UnitMax.text = $"{_tileModel.CurrentWorkersUnits}/{_saveTileConfig.MaxUnits} Units";
            for (int i = 0; i < _saveTileConfig.MaxUnits; i++)
            {
                if(_workerViews.Count >= _saveTileConfig.MaxUnits) return;
                
                _workerViews.Add(new WorkerView());
                _workerViews[i].BuildingType = BuildingTypes.None;
                _workerViews[i].ResourceType = ResourceType.None;
                
            }
        }
        public bool UpdateWorkerAssignment(IbuildingCollectable buildingCollectable, ICollectable building)
        {
            if (_tileModel.CurrentWorkersUnits >= _saveTileConfig.MaxUnits) return false;
            
            var workersAssigments =  _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
            if (workersAssigments == null)
            {
                var workersAssign = new WorkersAssigments(building);
                workersAssign.BusyWorkersCount++;
                _workersAssigments.Add(workersAssign);
            }
            else workersAssigments.BusyWorkersCount++;

            var worker = _workerViews.Find(x => x.BuildingType == BuildingTypes.None 
                && x.ResourceType == ResourceType.None || x.BuildingType == 0 && x.ResourceType == ResourceType.None);
            worker.BuildingType = building.BuildingTypes;
            worker.ResourceType = building.ResourceType;
            
            _tileModel.CurrentWorkersUnits++;
            FillWorkerList();
            return true;
        }
        public bool RemoveWorkerAssignment(IbuildingCollectable buildingCollectable, ICollectable building)
        {
            var worker = _workerViews.Find(x => x.ResourceType == buildingCollectable.ResourceType && x.BuildingType == buildingCollectable.BuildingType);
            var workersAssigment = _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
            
            if (workersAssigment == null || workersAssigment.BusyWorkersCount == 0) return false;
            
            worker.ResourceType = ResourceType.None;
            worker.BuildingType = BuildingTypes.None;
            
            workersAssigment.BusyWorkersCount--;
            _tileModel.CurrentWorkersUnits--;
            FillWorkerList();
            return true;
        }
        
        public void RemoveAllWorkerAssignment(IbuildingCollectable buildingCollectable, ICollectable building, TileController controller)
        {
            var workers = _workerViews.FindAll(x => x.BuildingType == buildingCollectable.BuildingType || x.ResourceType == buildingCollectable.ResourceType);
            var workerassign = _workersAssigments.Exists(x => x.Building.BuildingID == building.BuildingID);
            if (!workerassign) return;
            
            var workersAssigments = _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
                
            foreach (var worker in workers)
            {
                worker.ResourceType = ResourceType.None;
                worker.BuildingType = BuildingTypes.None;
            }
            _tileModel.CurrentWorkersUnits -= workersAssigments.BusyWorkersCount;
            workersAssigments.BusyWorkersCount = 0;
            _workersAssigments.Remove(workersAssigments);
            workersAssigments = null;
            FillWorkerList();
        }
        public int GetAssignedWorkers(ICollectable building)
        {
            if (!_workersAssigments.Exists(x => x.Building.BuildingID == building.BuildingID)) return 0;
            return _workersAssigments.
                Find(x => x.Building.BuildingID == building.BuildingID).BusyWorkersCount;
        }
    }
}