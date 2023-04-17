using UnityEngine;
using Views.BuildBuildingsUI;

namespace Code.UI
{
    public class BottonUI : MonoBehaviour
    {
        [field: SerializeField] public BuildingsUIView BuildingMenu { get; private set; }
    }
}