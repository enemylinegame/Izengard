using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class WorkerAssignmentsController
    {
        private TileController _tileController;

        private TileModel _tileModel => _tileController.TileModel;
        private TileConfig _saveTileConfig => _tileModel.SaveTileConfig;
        private List<WorkerView> _workerViews => _tileModel.Workers;
        private List<WorkersAssigments> _workersAssigments => _tileModel.WorkersAssigments;

        public WorkerAssignmentsController(TileController tileController)
        {
            _tileController = tileController;
        }
        
        public void FillWorkerList()
        {
            for (int i = 0; i < _saveTileConfig.MaxUnits.GetHashCode(); i++)
            {
                _workerViews.Add(new WorkerView());
                _workerViews[i].AssignedResource = ResourceType.None;
                
            }
        }
        /// <summary>
        /// Добавление юнита в найм к определенному зданию
        /// </summary>
        public bool UpdateWorkerAssigment(ResourceType resourceType, Building building)
        {
            var units = _workerViews.FindAll(x => x.AssignedResource != ResourceType.None);
            if (units.Count < _saveTileConfig.MaxUnits.GetHashCode())
            {
                if (!_workerViews.Exists(x => x.AssignedResource == ResourceType.None))
                {
                    return false;
                }

                if (!_workersAssigments.Exists(x => x.Building.BuildingID == building.BuildingID))
                {
                    var workers = new WorkersAssigments(building);
                    workers.BusyWorkersCount++;
                    _tileController.hiringUnits(1);
                    _workersAssigments.Add(workers);
                }
                else
                {
                    var workersAssigments =  _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
                    // Debug.Log(_saveTileConfig.MaxUnits.GetHashCode());
                    workersAssigments.BusyWorkersCount++;
                    _tileController.hiringUnits(1);
                    
                }
                var worker = _workerViews.Find(x => x.AssignedResource == ResourceType.None);
                worker.AssignedResource = resourceType;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Удаление юнита из найма из определенного здания
        /// </summary>
        public bool RemoveWorkerAssigment(ResourceType resourceType, Building building)
        {
            if (!_workerViews.Exists(x => x.AssignedResource == resourceType))
            {
                return false;
            }
            if (_workersAssigments.Exists(x => x.Building.BuildingID == building.BuildingID))
            {
                var workersAssigments =  _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
                workersAssigments.BusyWorkersCount--;
                _tileController.RemoveFromHiringUnits(1);
                if (workersAssigments.BusyWorkersCount < 0) workersAssigments.BusyWorkersCount = 0;
                return true;
            }
            var worker = _workerViews.Find(x => x.AssignedResource == resourceType);
            worker.AssignedResource = ResourceType.None;
            return false;
        }
        
        public void RemoveAllWorkerAssigment(ResourceType resourceType, Building building, TileController controller)
        {
            if (!_workerViews.Exists(x => x.AssignedResource == resourceType))
            {
                return;
            }
            if (_workersAssigments.Exists(x => x.Building.BuildingID == building.BuildingID))
            {
                var workersAssigments =  _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
                controller.RemoveFromHiringUnits(workersAssigments.BusyWorkersCount);
                workersAssigments.BusyWorkersCount = 0;
                if (workersAssigments.BusyWorkersCount < 0) workersAssigments.BusyWorkersCount = 0;
                
            }
            var worker = _workerViews.Find(x => x.AssignedResource == resourceType);
            worker.AssignedResource = ResourceType.None;
        }
        /// <summary>
        /// получение информации о нанятых юнитов для определенного здания
        /// </summary>
        /// <returns></returns>
        public int GetAssignedWorkers(Building building)
        {
            if (!_workersAssigments.Exists(x => x.Building.BuildingID == building.BuildingID))
            {
                return 0;
            }
            return _workersAssigments.
                Find(x => x.Building.BuildingID == building.BuildingID).BusyWorkersCount;
        }
    }
}