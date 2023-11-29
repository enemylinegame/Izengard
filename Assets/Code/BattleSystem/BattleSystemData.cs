using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(BattleSystemData), 
        menuName = "GameConfigs/" + nameof(BattleSystemData))]
    public class BattleSystemData : ScriptableObject
    {
        [field: SerializeField] 
        public float DestinationPositionError { get; private set; } = 0.3f;
        
        [field: SerializeField] 
        public float UnitsDestroyDelay { get; private set; } = 10.0f;
    }
}