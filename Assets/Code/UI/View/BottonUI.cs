using Code.BuildingSystem;
using Code.TileSystem;
using UnityEngine;
using CombatSystem.Views;

namespace Code.UI
{
    public class BottonUI : MonoBehaviour
    {
        [field: SerializeField] public BuildingsUIView BuildingMenu { get; set; }
        [field: SerializeField] public TileUIView TileUIView { get; set; }
        [field: SerializeField] public ResourcesLayoutUIView ResourcesLayoutUIView { get; set; }
        [field: SerializeField] public WarsUIView WarsUIView { get; set; }

    }
}