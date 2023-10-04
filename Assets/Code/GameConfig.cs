using UnityEngine;

namespace Izengard
{
    [CreateAssetMenu(fileName = nameof(GameConfig), menuName = "GameConfigs/"+nameof(GameConfig))]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public LayerMask MouseLayerMask { get; private set; }
    }
}