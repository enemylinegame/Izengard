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
        [SerializeField] private int maxUnits;
        [Space, Header("Здесь будет храниться список зданий на данный уровень")]
        [SerializeField] private List<BuildingConfig> _buildingTirs;
        [Space, Header("Стоимость починки")]
        [SerializeField] private List<ResourcePriceModel> _repairCost;
        [Header("Стоимость Восстановления")]
        [SerializeField] private List<ResourcePriceModel> _recoveryCost;
        
        
        
        public List<ResourcePriceModel> RecoveryCost => _recoveryCost;
        public List<ResourcePriceModel> RepairCost => _repairCost;
        public Sprite IconTile => _icon;
        public TileLvl TileLvl => _tileLvl;
        public int MaxUnits => maxUnits;
        public List<BuildingConfig> BuildingTirs => _buildingTirs;
    }
}