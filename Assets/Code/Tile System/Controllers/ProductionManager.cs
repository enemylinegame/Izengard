using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using ResourceSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class ProductionManager : IOnUpdate, IDisposable, IOnController
    {
        private WorkersTeamController _teamController;

        private List<WorkDescriptor> _worksTable;
        private Dictionary<int, int> _buildingsTable;

        private GlobalStock _globalStock;

        private int _maxWorks;
        private int _worksAccount;

        private int _mineWorkerPortionSize;
        private float _craftWorkerEfficiency;

        private class WorkDescriptor
        {
            public BuildingTypes BuildingType;
            public ResourceType ResourceType;
            public int WorkId;
        }

        public Action<int> OnWorksCountChanged = delegate { };

        public ProductionManager(GlobalStock globalStock,
            WorkersTeamController teamController, WorkersTeamConfig workerConfig)
        {
            _teamController = teamController;
            _globalStock = globalStock;

            _worksTable = new List<WorkDescriptor>();
            _buildingsTable = new Dictionary<int, int>();

            _maxWorks = 0;
            _worksAccount = 0;

            _mineWorkerPortionSize = workerConfig.MineWorkerPortionSize;
            _craftWorkerEfficiency = workerConfig.CraftWorkerPerformance;
        }

        public bool StartProduction(
            ICollectable building, Vector3 workPlace,
            IWorkerPreparation preparation)
        {
            if (_worksAccount >= _maxWorks)
                return false;

            int workId = BeginWork(building.SpawnPosition, workPlace,
                building.ResourceType, preparation);

            if (workId < 0)
                return false;

            _worksTable.Add(new WorkDescriptor
            {
                BuildingType = building.BuildingTypes,
                ResourceType = building.ResourceType,
                WorkId = workId
            });

            IncreaseWorksForBuilding(building.BuildingID);

            OnWorksCountChanged.Invoke(++_worksAccount);
            return true;
        }

        public void SeMaxWorks(int maxWorks)
        {
            _maxWorks = maxWorks;
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

        public bool StopFirstFindedProduction(ICollectable building)
        {
            if (!DecreaseWorksForBuilding(building.BuildingID))
                return false;

            WorkDescriptor work = _worksTable.Find(x =>
                x.ResourceType == building.ResourceType ||
                x.BuildingType == building.BuildingTypes);

            if (null == work)
                return false;

            _teamController.CancelWork(work.WorkId);
            _worksTable.Remove(work);
            OnWorksCountChanged.Invoke(--_worksAccount);

            return true;
        }

        public void StopAllFindedProductions(ICollectable building)
        {
            int buildingId = building.BuildingID;
            if (!_buildingsTable.TryGetValue(buildingId, out int worksCount))
            {
                return;
            }

            _worksAccount -= _buildingsTable[buildingId];
            OnWorksCountChanged.Invoke(_worksAccount);

            _buildingsTable.Remove(buildingId);

            var worksToStop = _worksTable.FindAll(
                x => x.BuildingType == building.BuildingTypes ||
                x.ResourceType == building.ResourceType);

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

        private int SendWorkerToMine(Vector3 workerInitPlace, Vector3 workPlace,
            ResourceType resourceType, IWorkerPreparation preparation)
        {
            
            IWorkerTask workerTask = new MiningProduction(
                _globalStock, resourceType, _mineWorkerPortionSize);

            return _teamController.SendWorkerToMine(
                workerInitPlace, workPlace, preparation, workerTask);
        }

        private int SendWorkerToManufactory(Vector3 workerInitPlace, Vector3 workPlace,
            ResourceType resourceType, IWorkerPreparation preparation)
        {
            
            IWorkerWork work = new ManufactoryProduction(
                _globalStock, resourceType, _craftWorkerEfficiency);

            return _teamController.SendWorkerToWork(
                workerInitPlace, workPlace, preparation, work);
        }

        private int BeginWork(Vector3 workerInitPlace, Vector3 workPlace,
            ResourceType resource, IWorkerPreparation preparation)
        {
            switch (resource)
            {
                case ResourceType.Iron:
                    {
                        return SendWorkerToMine(workerInitPlace, workPlace,
                            ResourceType.Iron, preparation);
                    }
                case ResourceType.Gold:
                    {
                        return SendWorkerToMine(workerInitPlace, workPlace,
                            ResourceType.Gold, preparation);
                    }
                case ResourceType.Textile:
                    {
                        return SendWorkerToManufactory(workerInitPlace, workPlace,
                            ResourceType.Textile, preparation);
                    }
            }

            Debug.LogError("Unknown resource type");
            return -1;
        }
    }
}