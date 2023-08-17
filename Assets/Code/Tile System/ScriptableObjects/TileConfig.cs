using System.Collections.Generic;
using Code.BuildingSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.TileSystem
{
    [CreateAssetMenu(fileName = nameof(TileConfig), menuName = "Tile System/" + nameof(TileConfig))]
    public class TileConfig : ScriptableObject
    {
        [Header("Настройки тайла на данный уровень")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private TileLvl _tileLvl;
        [SerializeField] private int _maxWorkers;
        [Space, Header("Здесь будет храниться список зданий на данный уровень")]
        [SerializeField] private List<BuildingConfig> _buildingTirs;
        [FormerlySerializedAs("_repairCost")]
        [Space, Header("Стоимость починки")]
        [SerializeField] private List<ResourcePriceModel> _priceRepair;
        [Header("Стоимость Восстановления")]
        [SerializeField] private List<ResourcePriceModel> _priceRecovery;
        [Header("Стоимость Апгрейда")]
        [SerializeField] private List<ResourcePriceModel> _priceUpgrade;
        
        
        
        public List<ResourcePriceModel> PriceRecovery => _priceRecovery;
        public List<ResourcePriceModel> PriceRepair => _priceRepair;
        public List<ResourcePriceModel> PriceUpgrade => _priceUpgrade;
        public Sprite IconTile => _icon;
        public TileLvl TileLvl => _tileLvl;
        public int MaxWorkers => _maxWorkers;
        public List<BuildingConfig> BuildingTirs => _buildingTirs;
    }
}