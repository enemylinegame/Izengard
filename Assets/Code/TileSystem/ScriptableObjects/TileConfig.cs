using System.Collections.Generic;
using Code.BuildingSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.TileSystem
{
    [CreateAssetMenu(fileName = nameof(TileConfig), menuName = "Tile System/" + nameof(TileConfig))]
    public class TileConfig : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private TileLvl _tileLvl;
        [FormerlySerializedAs("addUnits")] [SerializeField] private int maxUnits;
        [Header("Здесь будет храниться список зданий на данный уровень")]
        [SerializeField] private List<BuildingConfig> _buildingTirs;

        public Sprite IconTile => _icon;
        public TileLvl TileLvl => _tileLvl;
        public int MaxUnits => maxUnits;
        public List<BuildingConfig> BuildingTirs => _buildingTirs;
    }
}