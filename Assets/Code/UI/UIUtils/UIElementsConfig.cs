using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = nameof(UIElementsConfig), menuName = "UI/" + nameof(UIElementsConfig))]
    public class UIElementsConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject RightPanel { get; private set; }
        [field: SerializeField] public GameObject TopPanel { get; private set; }
        [field: SerializeField] public GameObject LeftPanel { get; private set; }
        [field: SerializeField] public GameObject BottonPanel { get; private set; }
        [field: SerializeField] public GameObject CenterPanel { get; private set; }
        [field: SerializeField] public GameObject MarketPanel { get; private set; }
    }
}