using System.Collections.Generic;
using Code.BuldingsSystem;
using Code.TileSystem.Interfaces;
using ResourceSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class WorkerMenager
    {
        private TileController _tileController;
        private TileModel _tileModel => _tileController.TileModel;
        private TileConfig _saveTileConfig => _tileModel.SaveTileConfig;
        private List<WorkerView> _workerViews => _tileModel.Workers;
        private List<WorkersAssigments> _workersAssigments => _tileModel.WorkersAssigments;

        public WorkerMenager(TileController tileController) => _tileController = tileController;
        public bool UpdateWorkerAssignment(IbuildingCollectable buildingCollectable, ICollectable building)
        {
            if (_workerViews.Count >= _saveTileConfig.MaxUnits)
            {
                return false;
            }
            var workersAssigments =  _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
            if (workersAssigments == null)
            {
                NewWorker();
                var workersAssign = new WorkersAssigments(building);
                workersAssign.BusyWorkersCount++;
                _tileController.HiringUnits(1);
                _workersAssigments.Add(workersAssign);
            }
            else
            {
                NewWorker();
                workersAssigments.BusyWorkersCount++;
                _tileController.HiringUnits(1);                 
            }

            var workers = _workerViews.Find(x => x.BuildingTypes == BuildingTypes.None && x.ResourceType == ResourceType.None);
            if (buildingCollectable is ICollectable buildingType)
            {
                workers.BuildingTypes = buildingType.BuildingTypes;
            }
            else if (buildingCollectable is ICollectable resource)
            {
                workers.ResourceType = resource.ResourceType;
            }
            return true;
        }
        public bool RemoveWorkerAssignment(IbuildingCollectable buildingCollectable, ICollectable building)
        {
            var worker = _workerViews.Find(x => x.ResourceType == buildingCollectable.ResourceType || x.BuildingTypes == buildingCollectable.BuildingType);
            var workersAssigment = _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
            
            if (workersAssigment != null)
            {
                workersAssigment.BusyWorkersCount--;
                _tileController.RemoveFromHiringUnits(1);
                _workerViews.Remove(worker);
                Object.Destroy(worker);
                _tileModel.CurrentUnits = _workerViews.Count;
                return true;
            }

            return false;
        }
        
        public void RemoveAllWorkerAssignment(IbuildingCollectable buildingCollectable, ICollectable building, TileController controller)
        {
            var worker = _workerViews.FindAll(x => x.BuildingTypes == buildingCollectable.BuildingType || x.ResourceType == buildingCollectable.ResourceType);
            var workerassign = _workersAssigments.Exists(x => x.Building.BuildingID == building.BuildingID);
            if (workerassign)
            {
                var workersAssigments = _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
                controller.RemoveFromHiringUnits(workersAssigments.BusyWorkersCount);
                workersAssigments.BusyWorkersCount = 0;
                if (workersAssigments.BusyWorkersCount < 0) workersAssigments.BusyWorkersCount = 0;
                foreach (var kvp in worker)
                {
                    _workerViews.Remove(kvp);
                    Object.Destroy(kvp);
                }
                _tileModel.CurrentUnits = _workerViews.Count;
            }
        }
        public int GetAssignedWorkers(ICollectable building)
        {
            if (!_workersAssigments.Exists(x => x.Building.BuildingID == building.BuildingID))
            {
                return 0;
            }
            return _workersAssigments.
                Find(x => x.Building.BuildingID == building.BuildingID).BusyWorkersCount;
        }

        private void NewWorker()
        {
            var worker = new WorkerView();
            worker.ResourceType = ResourceType.None;
            worker.BuildingTypes = BuildingTypes.None;
            _workerViews.Add(worker);
            _tileModel.CurrentUnits = _workerViews.Count;
        }
    }
}