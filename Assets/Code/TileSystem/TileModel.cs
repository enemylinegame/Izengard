using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem.ScriptableObjects;
using CombatSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileModel
    {
        private const int MAX_WORKERS = 5;
        private const int MAX_WARRIORS = 8;
        
        public HouseType HouseType { get; set; }
        public TileConfig TileConfig { get; set; }
        public List<Dot> DotSpawns { get; set; }
        public List<Building> FloodedBuildings { get; set; }
        public List<BuildingConfig> CurrBuildingConfigs { get; set; }
        public List<WorkerView> Workers { get; set; }
        public List<DefenderUnit> DefenderUnits { get; set; }
        public List<WorkersAssigments> WorkersAssigments { get; set; }
        public TileConfig SaveTileConfig { get; set; }
        public int MaxWorkers => MAX_WORKERS;
        public int MaxWarriors => MAX_WARRIORS;
        public int CurrentUnits { get; set; }
        
        
        public void Init()
        {
            SaveTileConfig = new TileConfig();
            CurrBuildingConfigs = new List<BuildingConfig>(TileConfig.BuildingTirs);
            FloodedBuildings = new List<Building>();
            Workers = new List<WorkerView>();
            WorkersAssigments = new List<WorkersAssigments>();
            DefenderUnits = new List<DefenderUnit>();
            SaveTileConfig = TileConfig;
        }
    }
}