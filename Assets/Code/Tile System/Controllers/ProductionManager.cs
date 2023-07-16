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

        private GlobalStock _globalStock;

        private int _mineWorkerPortionSize;
        private float _craftWorkerEfficiency;

        private sealed class WorkDescriptor
        {
            public BuildingTypes BuildingType;
            public ResourceType ResourceType;
            public int WorkId;
        }

        public ProductionManager(GlobalStock globalStock,
            WorkersTeamController teamController, WorkersTeamConfig workerConfig)
        {
            _teamController = teamController;
            _globalStock = globalStock;

            _worksTable = new List<WorkDescriptor>();

            _mineWorkerPortionSize = workerConfig.MineWorkerPortionSize;//resource config
            _craftWorkerEfficiency = workerConfig.CraftWorkerPerformance;
        }

        public bool StartFactoryProduction(Vector3 spawnPosition,
            ICollectable building,
            IWorkerPreparation preparation)
        {
            int workId = SendWorkerToManufactory(spawnPosition, 
                building.SpawnPosition,
                building.ResourceType, preparation);

            if (workId < 0)
                return false;

            _worksTable.Add(new WorkDescriptor
            {
                BuildingType = building.BuildingTypes,
                ResourceType = building.ResourceType,
                WorkId = workId
            });

            return true;
        }

        public bool StartMiningProduction(Vector3 spawnPosition,
           ICollectable building)
        {
            int workId = SendWorkerToMine(
                spawnPosition, building.SpawnPosition,
                building.ResourceType, null);

            if (workId < 0)
                return false;

            _worksTable.Add(new WorkDescriptor
            {
                BuildingType = building.BuildingTypes,
                ResourceType = building.ResourceType,
                WorkId = workId
            });

            return true;
        }


        public void StopFirstFindedWorker(ICollectable building)
        {
            WorkDescriptor work = _worksTable.Find(x =>
                x.ResourceType == building.ResourceType ||
                x.BuildingType == building.BuildingTypes);

            if (null == work)
                return;

            _teamController.CancelWork(work.WorkId);
            _worksTable.Remove(work);
        }

        public void StopAllProductions(ICollectable building)
        {
            if (null == building)
                return;

            int buildingId = building.BuildingID;
  
            var worksToStop = _worksTable.FindAll(
                x => x.BuildingType == building.BuildingTypes ||
                x.ResourceType == building.ResourceType);

            foreach (var work in worksToStop)
            {
                _teamController.CancelWork(work.WorkId);
                _worksTable.Remove(work);
            }
        }

        public void Dispose()
        {
            _teamController.Dispose();
            _teamController = null;

            _worksTable.Clear();
            _worksTable = null;
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
    }
}