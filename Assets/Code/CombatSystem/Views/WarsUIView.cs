using UnityEngine;
using UnityEngine.UI;


namespace CombatSystem.Views
{
    public sealed class WarsUIView : MonoBehaviour
    {
        [field: SerializeField] public Button EnterToBarracks { get; private set; }
        [field: SerializeField] public Transform[] Slots { get; private set; }
        [field: SerializeField] public Sprite UnitDefenderSprite { get; private set; }
        [field: SerializeField] public Sprite NoUnitSprite { get; private set; }
    }
}