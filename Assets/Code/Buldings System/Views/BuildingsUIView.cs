using UnityEngine;
using UnityEngine.UI;


namespace Code.BuildingSystem
{
    public class BuildingsUIView : MonoBehaviour
    {
        [field: SerializeField] public Button CloseMenuButton { get; private set; }
        [field: SerializeField] public Button PrefabButtonClear { get; private set; }
        [field: SerializeField] public GameObject BuildingInfo { get; private set; }
        [field: SerializeField] public Transform[] Windows { get; private set; }
        [field: SerializeField] public Transform ByBuildButtonsHolder { get; set; }
        [field: SerializeField] public Button BuyPrefabButton { get; set; }
    }
}