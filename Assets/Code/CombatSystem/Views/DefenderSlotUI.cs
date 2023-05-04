using UnityEngine;
using UnityEngine.UI;


namespace CombatSystem.Views
{
    public class DefenderSlotUI : MonoBehaviour
    {

        [field: SerializeField] public Image UnitIcon { get; private set; }
        [field: SerializeField] public Toggle SendToBarrackToggle { get; private set; }
        [field: SerializeField] public Button HireButton { get; private set; }
        [field: SerializeField] public Button DismissButton { get; private set; }

    }
}