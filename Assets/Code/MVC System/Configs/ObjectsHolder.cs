using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(ObjectsHolder), menuName = "GameConfigs/" + nameof(ObjectsHolder))]
    public class ObjectsHolder : ScriptableObject
    {
        [field: Header("Prefabs")]
        [field: SerializeField] public GameObject CellIndicator { get; private set; }

        [field: Header("Materials"), Space] 
        [field: SerializeField] public Material PreveiwMaterial { get; private set; }
    }
}