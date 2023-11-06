using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(BattleSystemConstants), menuName = "GameConfigs/" + nameof(BattleSystemConstants))]
    public class BattleSystemConstants : ScriptableObject
    {
        [field: SerializeField] public float DestinationPositionError { get; private set; } = 0.3f;
        [field: SerializeField] public float DeadUnitsDestroyDelay { get; private set; } = 10.0f;
    }
}