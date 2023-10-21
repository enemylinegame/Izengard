using System.Collections.Generic;
using UnityEngine;

namespace NewBuildingSystem
{
    [CreateAssetMenu(fileName = nameof(BuildingsSettingsSO), menuName = "GameConfigs/" + nameof(BuildingsSettingsSO))]
    public class BuildingsSettingsSO : ScriptableObject
    {
        public List<ObjectData> ObjectsData;
    }
}