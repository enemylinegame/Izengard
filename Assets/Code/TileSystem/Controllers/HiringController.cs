using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class HiringController
    {
        private TileController _tileController;

        private TileModel _TileModel => _tileController.TileModel;
        private TileConfig _saveTileConfig => _TileModel.SaveTileConfig;
        private List<WorkerView> _workerViews => _TileModel.Workers;
        private List<WorkersAssigments> _workersAssigmentses => _TileModel.WorkersAssigments;
        private int _eightQuantity
        {
            get => _TileModel.EightQuantity;
            set => _TileModel.EightQuantity = value;
        }

        public HiringController(TileController tileController)
        {
            _tileController = tileController;
        }
        
        public void FillWorkerList()
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
                
                var worker = _workerViews.Find(x => x.AssignedResource == BuildingTypes.None);
                worker.AssignedResource = resourceType;
            }
            else
            {
                var workersAssigments =  _workersAssigmentses.Find(x => x.Building.BuildingID == building.BuildingID);
                // Debug.Log(_saveTileConfig.MaxUnits.GetHashCode());
                workersAssigments.BusyWorkersCount++;
                _eightQuantity++;
                
                var worker = _workerViews.Find(x => x.AssignedResource == BuildingTypes.None);
                worker.AssignedResource = resourceType;
            }
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
    }
}