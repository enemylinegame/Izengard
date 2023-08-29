using UnityEngine;

namespace Code.Game
{
    [CreateAssetMenu(fileName = nameof(PrefabsHolder), menuName = "GameConfigs/" + nameof(PrefabsHolder))]
    public class PrefabsHolder : ScriptableObject
    {
        [field: SerializeField] public GameObject Res { get; set; }
        [field: SerializeField] public GameObject Bullet { get; set; }
        [field: SerializeField] public GameObject TestBuilding { get; set; }
    }
}