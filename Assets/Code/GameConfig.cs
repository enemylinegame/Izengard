using UnityEngine;

namespace Izengard
{
    [CreateAssetMenu(fileName = nameof(GameConfig), menuName = "GameConfigs/" + nameof(GameConfig))]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public Vector2Int BuildingStageMapSize { get; private set; }
        [field: SerializeField] public Vector2Int BattleStageMapSize { get; private set; }
        [field: SerializeField] public LayerMask MouseLayerMask { get; private set; }
    }
}