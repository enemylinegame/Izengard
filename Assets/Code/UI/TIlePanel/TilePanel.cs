using Code.BuildingSystem;
using Code.TileSystem;
using UnityEngine;
using CombatSystem.Views;

namespace Code.UI
{
    public class TilePanel : MonoBehaviour
    {
        [field: SerializeField] public TileUIBuildingBoard TileMenu { get; set; }
        [field: SerializeField] public TileUIMainBoard TileUIMainBoard { get; set; }
        [field: SerializeField] public TileResourcesPanel TileResourcesPanel { get; set; }
        [field: SerializeField] public WarsPanel WarsPanel { get; set; }

    }
}