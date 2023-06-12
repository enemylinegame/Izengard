using System;
using System.Collections.Generic;
using Code.BuldingsSystem;
using Code.TileSystem.Interfaces;
using ResourceSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class ProductionManager: IOnUpdate, IDisposable, IOnController
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

        public ProductionManager(GlobalStock globalStock, 
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

            int workId = BeginWork(building.SpawnPosition, workPlace, 
                buildingCollectable.ResourceType);

            if (workId < 0)
                return false;

            _worksTable.Add(new WorkDescriptor
            {
                BuildingType = buildingCollectable.BuildingType,
                ResourceType = buildingCollectable.ResourceType,
                WorkId = workId
            });

            IncreaseWorksForBuilding(building.BuildingID);

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

        public bool StopFirstFindedProduction(
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
        
        public void StopAllFindedProductions(
            IbuildingCollectable buildingCollectable, ICollectable building)
        {

            int buildingId = building.BuildingID;
            if (!_buildingsTable.TryGetValue(buildingId, out int worksCount))
            {
                return;
            }

            _tileModel.CurrentWorkersUnits -= _buildingsTable[buildingId];
            _buildingsTable.Remove(buildingId);

            var worksToStop = _worksTable.FindAll(
                x => x.BuildingType == buildingCollectable.BuildingType || 
                x.ResourceType == buildingCollectable.ResourceType);

            foreach (var work in worksToStop)
            {
                _teamController.CancelWork(work.WorkId);
                _worksTable.Remove(work);
            }
        }
        public int GetAssignedWorkers(ICollectable building)
        {

            if (!_buildingsTable.TryGetValue(
                building.BuildingID, out int workersCount))
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


        int SendWorkerToMine(Vector3 workerInitPlace, Vector3 workPlace, 
            ResourceType resourceType)
        {
            int portionSize = 5;
            IWorkerTask workerTask = new MiningProduction(
                _globalStock, ResourceType.Iron, portionSize);

            return _teamController.SendWorkerToMine(
                workerInitPlace, workPlace, workerTask);
        }

        int SendWorkerToManufactory(Vector3 workerInitPlace, Vector3 workPlace,
            ResourceType resourceType)
        {
            float efficiency = 5.0f;
            IWorkerWork work = new ManufactoryProduction(
                _globalStock, ResourceType.Iron, efficiency);

            return _teamController.SendWorkerToWork(
                workerInitPlace, workPlace, null, work, null);
        }

        private int BeginWork(Vector3 workerInitPlace, Vector3 workPlace, 
            ResourceType resource)
        {
            switch (resource)
            {
                case ResourceType.Iron:
                {
                     return SendWorkerToMine(
                         workerInitPlace, workPlace, ResourceType.Iron);
                }
                case ResourceType.Gold:
                {
                    return SendWorkerToMine(
                        workerInitPlace, workPlace, ResourceType.Gold);
                }
                case ResourceType.Textile:
                {
                    return SendWorkerToManufactory
                        (workerInitPlace, workPlace, ResourceType.Textile);
                }
            }

            Debug.LogError("Unknown resource type");
            return -1;
        }
    }
}