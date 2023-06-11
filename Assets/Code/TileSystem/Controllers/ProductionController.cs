using System;
using System.Collections.Generic;
using Code.BuldingsSystem;
using Code.TileSystem.Interfaces;
using ResourceSystem;

namespace Code.TileSystem
{
    public class ProductionController: IOnUpdate, IDisposable, IOnController
    {
        private WorkersTeamController _teamController;
        private TileModel _tileModel;

        private readonly int _maxWorks;

        private List<WorkDescriptor> _worksTable;
        private Dictionary<int, int> _buildingsTable;

        private GlobalStock _globalStock;

        private class WorkDescriptor
        {
            public BuildingTypes BuildingType;
            public ResourceType ResourceType;
            public int WorkId;
        }

        public ProductionController(GlobalStock globalStock, 
            WorkersTeamController teamController)
        {
            _maxWorks = _tileModel.MaxWorkers;


            _teamController = teamController;
            _globalStock = globalStock;

            _worksTable = new List<WorkDescriptor>();
            _buildingsTable = new Dictionary<int, int>();
        }

        public bool StartProduction(IbuildingCollectable buildingCollectable, 
            ICollectable building, Vector3 workPlace)
        {
            if (_tileModel.CurrentWorkersUnits >= _maxWorks) 
                return false;

            IncreaseWorksForBuilding(building.BuildingID);

            int workId = _teamController.SendWorkerToWork();

            _worksTable.Add(new WorkDescriptor
            {
                BuildingType = buildingCollectable.BuildingType,
                ResourceType = buildingCollectable.ResourceType,
                WorkId = workId
            });

            _tileModel.CurrentWorkersUnits++;

            return true;
        }

        private bool DecreaseWorksForBuilding(int buildingId)
        {
            if (!_buildingsTable.ContainsKey(buildingId))
                return false;

            
                int workersCount = --_buildingsTable[buildingId];

                if (workersCount <= 0)
                    _buildingsTable.Remove(buildingId);
                else
                    _buildingsTable[buildingId] = workersCount;

            return true;
        }

        private void IncreaseWorksForBuilding(int buildingId)
        {
            if (!_buildingsTable.ContainsKey(buildingId))
            {
                _buildingsTable.Add(buildingId, 0);
            }

            ++_buildingsTable[buildingId];
        }

        public bool StopProduction(
            IbuildingCollectable buildingCollectable, ICollectable building)
        {
            if (!DecreaseWorksForBuilding(building.BuildingID))
                return false;

            WorkDescriptor work = _worksTable.Find(x => 
                x.ResourceType == buildingCollectable.ResourceType ||
                x.BuildingType == buildingCollectable.BuildingType);

            if (null == work)
                return false;

            _teamController.CancelWork(work.WorkId);
            _worksTable.Remove(work);
            _tileModel.CurrentWorkersUnits--;

            return true;
        }
        
        public void RemoveAllWorkerAssignment(IbuildingCollectable buildingCollectable, 
            ICollectable building, TileController controller)
        {
            //var worker = _workerViews.FindAll(x => x.BuildingType == buildingCollectable.BuildingType || 
            //    x.ResourceType == buildingCollectable.ResourceType);

            //var workerassign = _workersAssigments.Exists(x => x.Building.BuildingID == building.BuildingID);

            //if (!workerassign) return;
            
            //var workersAssigments = _workersAssigments.Find(x => x.Building.BuildingID == building.BuildingID);
                
            //foreach (var kvp in worker)
            //{
            //    kvp.ResourceType = ResourceType.None;
            //    kvp.BuildingType = BuildingTypes.None;
            //}
            //_tileModel.CurrentWorkersUnits -= workersAssigments.BusyWorkersCount;
            //workersAssigments.BusyWorkersCount = 0;
            //_workersAssigments.Remove(workersAssigments);
            //workersAssigments = null;

        }
        public int GetAssignedWorkers(ICollectable building)
        {

            if (!_buildingsTable.TryGetValue(building.BuildingID, out int workersCount))
                return 0;
            return workersCount;
        }

        public void OnUpdate(float deltaTime)
        {
            _teamController.OnUpdate(deltaTime);
        }

        public void Dispose()
        {
            _teamController.Dispose();
        }
    }
}