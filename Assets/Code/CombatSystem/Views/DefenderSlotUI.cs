using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace CombatSystem.Views
{
    public class DefenderSlotUI : MonoBehaviour
    {

        [field: SerializeField] public Image UnitIcon { get; private set; }
        [field: SerializeField] public Button UnitIconButton { get; private set; }
        [field: SerializeField] public Toggle SendToBarrackToggle { get; private set; }
        [field: SerializeField] public Button HireButton { get; private set; }
        [field: SerializeField] public Button DismissButton { get; private set; }
        [field: SerializeField] public GameObject SelectBoard { get; private set; }

    }
}