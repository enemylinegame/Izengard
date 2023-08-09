using UnityEngine;
using UnityEngine.UI;

namespace Code.Units.HireDefendersSystem
{
    public class HireUnitSlotUiView : MonoBehaviour
    {
        [field: SerializeField] public Button HireButton { get; private set; }
        [field: SerializeField] public Image Icon { get; private set; }
        [field: SerializeField] public Text UnitName { get; private set; }
        [field: SerializeField] public Text Cost { get; private set; }
    }
}