using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem.ScriptableObjects;
using CombatSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileModel
    {
        public HouseType HouseType { get; set; }
        public TileConfig TileConfig { get; set; }
        public List<Dot> DotSpawns { get; set; }
        public List<Building> FloodedBuildings { get; set; }
        // public List<Mineral> FloodedMinerals { get; set; }
        public List<BuildingConfig> CurrBuildingConfigs { get; set; }
        public List<WorkerView> Workers { get; set; }
        public List<DefenderUnit> DefenderUnits { get; set; }
        public List<WorkersAssigments> WorkersAssigments { get; set; }
        public TileConfig SaveTileConfig { get; set; }
        public int EightQuantity { get; set; }
        

        public void Init()
        {
            SaveTileConfig = new TileConfig();
            CurrBuildingConfigs = new List<BuildingConfig>(TileConfig.BuildingTirs);
            FloodedBuildings = new List<Building>();
            // FloodedMinerals = new List<Mineral>();
            Workers = new List<WorkerView>();
            WorkersAssigments = new List<WorkersAssigments>();

            SaveTileConfig = TileConfig;
        }
    }
}