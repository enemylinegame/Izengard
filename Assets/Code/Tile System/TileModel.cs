using System.Collections.Generic;
using Code.BuildingSystem;
using CombatSystem;
using CombatSystem.Interfaces;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileModel
    {
        private const int MAX_WARRIORS = 8;
        
        public Damageable CenterBuilding { get; set; }
        public TileType TileType { get; set; }
        public TileConfig TileConfig { get; set; }
        public List<Dot> DotSpawns { get; set; }
        public List<ICollectable> FloodedBuildings { get; set; }
        public List<BuildingConfig> CurrBuildingConfigs { get; set; }
        public List<DefenderPreview> DefenderUnits { get; private set; }
        public List<IDamageable> EnemiesInTile { get; private set; }
        public TileConfig SaveTileConfig { get; set; }
        public int MaxWorkers => TileConfig.MaxWorkers;
        public int MaxWarriors;
        public int WorkersCount { get; set; }
        public Vector3 TilePosition { get; set; } // TODO: after merging, redo this ...
        
        public void Init()
        {
            SaveTileConfig = new TileConfig();
            if(TileConfig != null) CurrBuildingConfigs = new List<BuildingConfig>(TileConfig.BuildingTirs);
            FloodedBuildings = new List<ICollectable>();
            DefenderUnits = new List<DefenderPreview>();
            EnemiesInTile = new List<IDamageable>();
            SaveTileConfig = TileConfig;
        }
    }
}