using System.Collections.Generic;
using Code.BuildingSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.TileSystem
{
    //[CreateAssetMenu(fileName = nameof(TileConfig), menuName = "Tile System/" + nameof(TileConfig))]
    [System.Serializable]
    public class TileConfig
    {
        [Header("Настройки тайла на данный уровень")]
        public Sprite IconTile;
        public TileLvl TileLvl;
        public int MaxWorkers;
        [Header("Здесь будет храниться список зданий на данный уровень")]
        public List<BuildingConfig> BuildingTirs;
        [FormerlySerializedAs("_repairCost")]
        [Header("Стоимость починки")]
        public List<ResourcePriceModel> PriceRepair;
        [Header("Стоимость Восстановления")]
        public List<ResourcePriceModel> PriceRecovery;
        [Header("Стоимость Апгрейда")]
        public List<ResourcePriceModel> PriceUpgrade;
    }
}