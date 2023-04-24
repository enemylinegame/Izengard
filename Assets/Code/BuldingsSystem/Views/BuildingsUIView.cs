using BuildingSystem;
using System.Collections.Generic;
using Code;
using Code.BuildingSystem;
using Code.TileSystem;
using ResourceSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Views.BuildBuildingsUI
{
    public class BuildingsUIView : MonoBehaviour
    {
        [field: SerializeField] public Button CloseMenuButton { get; private set; }
        [field: SerializeField] public Button PrefabButtonClear { get; private set; }
        [field: SerializeField] public GameObject BuildingInfo { get; private set; }
        [field: SerializeField] public Transform[] Windows { get; private set; }
        [field: SerializeField] public Transform ByBuildButtonsHolder { get; set; }
        [field: SerializeField] public Button BuyPrefabButton { get; set; }
        [field: SerializeField] public Button BuyDefender { get; private set; } // add Nikolay Vasilev
        [field: SerializeField] public Button EnterToBarracks { get; private set; } // add Anton

    }
}