using System.Collections.Generic;
using Code.TileSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Code.BuildingSystem
{
    public class TileUIInfoView : MonoBehaviour
    {
        [field: Header("TileUI")]
        [field: SerializeField] public Button CloseMenuButton { get; private set; }
        [field: SerializeField] public Button PrefabButtonClear { get; private set; }
        [field: SerializeField] public GameObject BuildingInfo { get; private set; }
        [field: SerializeField] public Transform[] Windows { get; private set; }
        [field: SerializeField] public Transform ByBuildButtonsHolder { get; set; }
        [field: SerializeField] public Button BuyPrefabButton { get; set; }
    }
}