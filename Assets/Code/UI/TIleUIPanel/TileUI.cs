using Code.BuildingSystem;
using Code.TileSystem;
using UnityEngine;
using CombatSystem.Views;

namespace Code.UI
{
    public class TileUI : MonoBehaviour
    {
        [field: SerializeField] public TileUIInfoView TileMenu { get; set; }
        [field: SerializeField] public TileUIView TileUIView { get; set; }
        [field: SerializeField] public ResourcesLayoutUIView ResourcesLayoutUIView { get; set; }
        [field: SerializeField] public WarsUIView WarsUIView { get; set; }

    }
}