using Code.UI;
using Izengard;
using NewBuildingSystem;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(ConfigsHolder), menuName = "GameConfigs/" + nameof(ConfigsHolder))]
    public class ConfigsHolder : ScriptableObject
    {
        [field: SerializeField] public ObjectsHolder ObjectsHolder { get; private set; }
        [field: SerializeField] public UIElementsConfig UIElementsConfig { get; private set; }
        [field: SerializeField] public GameConfig GameConfig { get; private set; }
        [field: SerializeField] public BuildingsSettingsSO BuildingsSettingsSo { get; private set; }
    }
}