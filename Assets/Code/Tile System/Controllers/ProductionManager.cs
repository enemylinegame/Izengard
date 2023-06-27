using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using ResourceSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public sealed class ProductionManager : IDisposable
    {
        private WorkersTeamController _teamController;

        private List<WorkDescriptor> _worksTable;
        private Dictionary<int, int> _buildingsTable;

        private GlobalStock _globalStock;

        private int _worksAccount;

        private int _mineWorkerPortionSize;
        private float _craftWorkerEfficiency;

        private sealed class WorkDescriptor
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

            _worksAccount = 0;

            _mineWorkerPortionSize = workerConfig.MineWorkerPortionSize;//resource config
            _craftWorkerEfficiency = workerConfig.CraftWorkerPerformance;
        }

        public bool IsThereFreeWorkers(ICollectable building)
        {
            if (0 == building.MaxWorkers)
                return false;

            if (!_buildingsTable.TryGetValue(building.BuildingID,
                out int workersAccount))
                return true;

            if (building.MaxWorkers > workersAccount)
                return true;

            return false;
        }

        public bool StartProduction(Vector3 spawnPosition,
            ICollectable building,
            IWorkerPreparation preparation)
        {

            int workId = BeginWork(spawnPosition, building.SpawnPosition,
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

        private bool DecreaseWorksForBuilding(int buildingId)
        {
            if (!_buildingsTable.ContainsKey(buildingId))
                return false;

            if (--_buildingsTable[buildingId] <= 0)
                _buildingsTable.Remove(buildingId);

            return true;
        }

        public bool IsThereBuisyWorkers(ICollectable building)
        {
            if (!_buildingsTable.TryGetValue(
                building.BuildingID, out int worksCount))
                return false;

            return worksCount > 0 ? true : false;
        }

        private void IncreaseWorksForBuilding(int buildingId)
        {
            if (!_buildingsTable.ContainsKey(buildingId))
            {
                _buildingsTable.Add(buildingId, 0);
            }

            ++_buildingsTable[buildingId];
        }

        public void StopFirstFindedWorker(ICollectable building)
        {
            if (!DecreaseWorksForBuilding(building.BuildingID))
                return;

            WorkDescriptor work = _worksTable.Find(x =>
                x.ResourceType == building.ResourceType ||
                x.BuildingType == building.BuildingTypes);

            if (null == work)
                return;

            _teamController.CancelWork(work.WorkId);
            _worksTable.Remove(work);
            OnWorksCountChanged.Invoke(--_worksAccount);
        }

        public void StopAllProductions(ICollectable building)
        {
            if (null == building)
                return;

            int buildingId = building.BuildingID;
            if (!_buildingsTable.TryGetValue(buildingId, out int worksCount))
                return;

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

        public void Dispose()
        {
            _teamController.Dispose();
            _teamController = null;

            _worksTable.Clear();
            _worksTable = null;
            _buildingsTable.Clear();
            _buildingsTable = null;
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
                case ResourceType.Wood:
                    {
                        return SendWorkerToMine(workerInitPlace, workPlace,
                            resource, preparation);
                    }
                case ResourceType.Iron:
                    {
                        return SendWorkerToMine(workerInitPlace, workPlace,
                            resource, preparation);
                    }
                case ResourceType.Deer:
                    {
                        return SendWorkerToMine(workerInitPlace, workPlace,
                            resource, preparation);
                    }
                case ResourceType.MagicStones:
                    {
                        return SendWorkerToMine(workerInitPlace, workPlace,
                            resource, preparation);
                    }
                case ResourceType.Gold:
                    {
                        return SendWorkerToMine(workerInitPlace, workPlace,
                            resource, preparation);
                    }
                case ResourceType.Horse:
                    {
                        return SendWorkerToManufactory(workerInitPlace, workPlace,
                            resource, preparation);
                    }
                case ResourceType.Textile:
                    {
                        return SendWorkerToManufactory(workerInitPlace, workPlace,
                            resource, preparation);
                    }
                case ResourceType.Steel:
                    {
                        return SendWorkerToManufactory(workerInitPlace, workPlace,
                            resource, preparation);
                    }
            }

            Debug.LogError("Unknown resource type");
            return -1;
        }
    }
}