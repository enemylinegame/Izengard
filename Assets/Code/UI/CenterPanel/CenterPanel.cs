using Code.TileSystem;
using ResourceMarket;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class CenterPanel : MonoBehaviour
    {
        [field: Space, Header("Buildings Buy")]
        [field: SerializeField] public GameObject BuildingBuy { get; set; }
        [field: SerializeField] public Transform BuildButtonsHolder { get; set; }
        [field: SerializeField] public Button CloseBuildingsBuy { get; set; }

        [field: Space, Header("Tile")]
        [field: SerializeField] public GameObject TileByButtons { get; set; }
        [field: SerializeField] public TileSelectionView TIleSelection { get; set; }

        [field: Space, Header("Info")]
        [field: SerializeField] public BaseNotificationUI BaseNotificationUI { get; set; }
    }
}

