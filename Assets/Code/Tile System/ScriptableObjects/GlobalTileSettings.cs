using System.Collections.Generic;
using Code.BuildingSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.TileSystem
{
    [CreateAssetMenu(fileName = nameof(GlobalTileSettings), menuName = "Tile System/" + nameof(GlobalTileSettings))]
    public class GlobalTileSettings : ScriptableObject
    {
        [Header("Юниты")]
        public int MaxWorkersWar;
        public int MaxWarriorsEco;
        [Space, Header("Уровень Здоровья Башен")]
        public int MaxHealthMainTower;
        public int MaxHealthCenterBuilding;
        [Space, Header("Разные списки для настроки тайла")]
        public List<TileConfig> Levels;
    }
}