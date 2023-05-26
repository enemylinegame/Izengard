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
        //private List<WorkerWork> _workerViews => _tileModel.Workers;
        private List<WorkersAssigments> _workersAssigments => 
            _tileModel.WorkersAssigments;

        WorkersTeamController _workersTeamController;
        Dictionary<int, int> _workerTasks;

        public WorkerMenager(TileController tileController,
            WorkersTeamController workersController)
        {
            _tileController = tileController;
            _workersTeamController = workersController;

            _workersTeamController.OnMissionCompleted += 
                OnWorkerMissionCompleted;

            _workerTasks = new Dictionary<int, int>();
        }

        private void OnWorkerMissionCompleted(int workerId)
        {
            if (_workerTasks.TryGetValue(workerId, out int value))
            {
                lock (_workerTasks)
                {
                    _workerTasks.Remove(workerId);
                }
            }
        }

        public bool UpdateWorkerAssignment(IbuildingCollectable buildingCollectable, 
            ICollectable building)
        {
            //if (_workerViews.Count >= _saveTileConfig.MaxUnits)
            //{
            //    return false;
            //}

            int workTaskNumber = _workersTeamController.SendWorkerToWork(
                building.SpawnPosition, 
                building.SpawnPosition + new Vector3(100, 0, 100));

            lock (_workerTasks)
            {
                _workerTasks.Add(workTaskNumber, 50);
            }

            var workersAssigments =  _workersAssigments.Find(
                x => x.Building.BuildingID == building.BuildingID);

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

            //var workers = _workerViews.Find(x => x.BuildingType == BuildingTypes.None && 
            //    x.ResourceType == ResourceType.None);

            //if (buildingCollectable is ICollectable buildingType)
            //{
            //    workers.BuildingType = buildingType.BuildingTypes;
            //}
            //else if (buildingCollectable is ICollectable resource)
            //{
            //    workers.ResourceType = resource.ResourceType;
            //}
            return true;
        }
        public bool RemoveWorkerAssignment(
            IbuildingCollectable buildingCollectable, ICollectable building)
        {
            //var worker = _workerViews.Find(x => 
            //x.ResourceType == buildingCollectable.ResourceType || 
            //x.BuildingType == buildingCollectable.BuildingType);

            var workersAssigment = _workersAssigments.Find(
                x => x.Building.BuildingID == building.BuildingID);
            
            if (workersAssigment != null)
            {
                workersAssigment.BusyWorkersCount--;
                _tileController.RemoveFromHiringUnits(1);
                //_workerViews.Remove(worker);
                //Object.Destroy(worker);
                _tileModel.CurrentUnits --;
                return true;
            }

            return false;
        }
        
        public void RemoveAllWorkerAssignment(IbuildingCollectable buildingCollectable, 
            ICollectable building, TileController controller)
        {
            //var worker = _workerViews.FindAll(x => 
            //    x.BuildingType == buildingCollectable.BuildingType || 
            //    x.ResourceType == buildingCollectable.ResourceType);

            var workerassign = _workersAssigments.Exists(
                x => x.Building.BuildingID == building.BuildingID);

            if (workerassign)
            {
                var workersAssigments = _workersAssigments.Find(
                    x => x.Building.BuildingID == building.BuildingID);

                controller.RemoveFromHiringUnits(workersAssigments.BusyWorkersCount);
                workersAssigments.BusyWorkersCount = 0;
                if (workersAssigments.BusyWorkersCount < 0) workersAssigments.BusyWorkersCount = 0;
                //foreach (var kvp in worker)
                //{
                //    _workerViews.Remove(kvp);
                //    Object.Destroy(kvp);
                //}
                _tileModel.CurrentUnits --;
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
            worker.BuildingType = BuildingTypes.None;
            //_workerViews.Add(worker);
            _tileModel.CurrentUnits ++;
        }
    }
}