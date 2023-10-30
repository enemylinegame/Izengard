using System.Collections.Generic;
using Code.UI;
using Izengard;
using NewBuildingSystem;
using SpawnSystem;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(ConfigsHolder), menuName = "GameConfigs/" + nameof(ConfigsHolder))]
    public class ConfigsHolder : ScriptableObject
    {
        [field: SerializeField] public ObjectsHolder ObjectsHolder { get; set; }
        [field: SerializeField] public UIElementsConfig UIElementsConfig { get; set; }
        [field: SerializeField] public GameConfig GameConfig { get; set; }
        [field: SerializeField] public SpawnSettings EnemySpawnSettings { get; private set; }
        [field: SerializeField] public BuildingsSettingsSO BuildingsSettingsSo { get; private set; }
        [field: SerializeField] public SpawnSettings DefendersSpawnSettings { get; private set; }
        
    }
}