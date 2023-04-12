using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.TileSystem
{
    [CreateAssetMenu(fileName = nameof(TileList), menuName = "Tile System/" + nameof(TileList))]
    public class TileList : ScriptableObject
    {
        [SerializeField] private List<TileConfig> _LVLList;

        public List<TileConfig> LVLList => _LVLList;
    }
}