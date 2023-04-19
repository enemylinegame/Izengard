using Code.TileSystem;
using UnityEngine;
using Views.BuildBuildingsUI;

namespace Code.UI
{
    public class BottonUI : MonoBehaviour
    {
        [field: SerializeField] public BuildingsUIView BuildingMenu { get; set; }
        [field: SerializeField] public TileUIView TileUIView { get; set; }
    }
}