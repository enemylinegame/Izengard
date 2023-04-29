using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem.ScriptableObjects;
using CombatSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileModel
    {
        [field: SerializeField] public HouseType HouseType { get; set; }
        [field: SerializeField] public TileConfig TileConfig { get; set; }
        [field: SerializeField] public List<Dot> DotSpawns { get; set; }
        [field: SerializeField] public List<Building> FloodedBuildings { get; set; }
        [field: SerializeField] public List<Mineral> FloodedMinerals { get; set; }
        [field: SerializeField] public List<BuildingConfig> CurrBuildingConfigs { get; set; }
        [field: SerializeField] public List<WorkerView> Workers { get; set; }
        [field: SerializeField] public List<DefenderUnit> DefenderUnits { get; set; }
        [field: SerializeField] public List<WorkersAssigments> WorkersAssigments { get; set; }
        [field: SerializeField] public TileConfig SaveTileConfig { get; set; }
        [field: SerializeField] public int EightQuantity { get; set; }

        public void Init()
        {
            SaveTileConfig = new TileConfig();
            CurrBuildingConfigs = new List<BuildingConfig>(TileConfig.BuildingTirs);
            FloodedBuildings = new List<Building>();
            FloodedMinerals = new List<Mineral>();
            Workers = new List<WorkerView>();
            WorkersAssigments = new List<WorkersAssigments>();

            SaveTileConfig = TileConfig;
        }
    }
}