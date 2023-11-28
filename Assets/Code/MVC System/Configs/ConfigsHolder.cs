using BattleSystem.Buildings.Configs;
using SpawnSystem;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(ConfigsHolder), menuName = "GameConfigs/" + nameof(ConfigsHolder))]
    public class ConfigsHolder : ScriptableObject
    {
        [field: SerializeField] public PrefabsHolder PrefabsHolder { get; set; }
        [field: SerializeField] public WaveSettings EnemyWaveSettings { get; set; }
        [field: SerializeField] public WarBuildingConfig MainTowerSettings { get; private set; }
        [field: SerializeField] public BattleSystemData BattleSystemConst { get; private set; }
    }
}