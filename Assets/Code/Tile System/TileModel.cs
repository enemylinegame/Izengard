using System.Collections.Generic;
using Code.BuildingSystem;
using CombatSystem;

namespace Code.TileSystem
{
    public class TileModel
    {
        private const int MAX_WORKERS = 5;
        private const int MAX_WARRIORS = 8;
        
        public HouseType HouseType { get; set; }
        public TileConfig TileConfig { get; set; }
        public List<Dot> DotSpawns { get; set; }
        public List<ICollectable> FloodedBuildings { get; set; }
        public List<BuildingConfig> CurrBuildingConfigs { get; set; }
        public List<DefenderUnit> DefenderUnits { get; set; }
        public TileConfig SaveTileConfig { get; set; }
        public int MaxWorkers => MAX_WORKERS;
        public int MaxWarriors => MAX_WARRIORS;
        public int CurrentWorkersUnits { get; set; }
        
        
        public void Init()
        {
            SaveTileConfig = new TileConfig();
            CurrBuildingConfigs = new List<BuildingConfig>(TileConfig.BuildingTirs);
            FloodedBuildings = new List<ICollectable>();
            DefenderUnits = new List<DefenderUnit>();
            SaveTileConfig = TileConfig;
        }
    }
}