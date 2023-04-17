using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class CenterUI : MonoBehaviour
    {
        [field: Space, Header("Buildings Buy")]
         [field: SerializeField] public GameObject BuildingBuy { get; set; }
         [field: SerializeField] public Transform BuildButtonsHolder { get; set; }
         [field: SerializeField] public Button CloseBuildingsBuy { get; set; }
         
         [field: Space, Header("Tile")]
         [field: SerializeField] public GameObject TileByButtons { get; set; }
    }
}

