using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = nameof(UIElementsConfig), menuName = "UI/" + nameof(UIElementsConfig))]
    public class UIElementsConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject ResPanel { get; private set; }
        [field: SerializeField] public GameObject InfoPanel { get; private set; }
        [field: SerializeField] public GameObject BuildingPanel { get; private set; }
        [field: SerializeField] public GameObject BuildingInfoPanel { get; private set; }
        [field: SerializeField] public GameObject FunctionPanel { get; private set; }
        [field: SerializeField] public GameObject CenterPanel { get; private set; }
        [field: SerializeField] public GameObject EndGameScreenPanel { get; private set; }
        [field: SerializeField] public GameObject InGameMenu { get; private set; }
        [field: SerializeField] public GameObject BattlePanel { get; private set; }
        [field: SerializeField] public GameObject BrewSystemUI { get; private set; }

    }
}