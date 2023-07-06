using UnityEngine;
using UnityEngine.UI;

namespace Code.Units.HireDefendersSystem
{
    public class HireUnitUIView : MonoBehaviour
    {
        [field: SerializeField] public Button HireButton { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Transform ButtonContainer { get; private set; }
        [field: SerializeField] public GameObject Root { get; private set; }
    }
}