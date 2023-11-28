using System.Collections.Generic;
using BattleSystem.Buildings.Configs;
using SpawnSystem;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(ConfigsHolder), menuName = "GameConfigs/" + nameof(ConfigsHolder))]
    public class ConfigsHolder : ScriptableObject
    {
        [field: SerializeField] public PrefabsHolder PrefabsHolder { get; set; }
        [field: SerializeField] public SpawnSettings EnemySpawnSettings { get; private set; }
        [field: SerializeField] public SpawnSettings DefendersSpawnSettings { get; private set; }
        [field: SerializeField] public WarBuildingConfig MainTowerSettings { get; private set; }
        [field: SerializeField] public BattleSystemConstants BattleSystemConst { get; private set; }
    }
}