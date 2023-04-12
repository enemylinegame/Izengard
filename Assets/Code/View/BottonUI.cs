using UnityEngine;
using Views.BuildBuildingsUI;

namespace Code.View
{
    public class BottonUI : MonoBehaviour
    {
        [field: SerializeField] public BuildingsUIView BuildingMenu { get; private set; }
    }
}