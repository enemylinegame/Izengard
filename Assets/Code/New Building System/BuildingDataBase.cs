using System.Collections.Generic;
using UnityEngine;

namespace NewBuildingSystem
{
    [CreateAssetMenu(fileName = nameof(BuildingDataBase), menuName = "GameConfigs/" + nameof(BuildingDataBase))]
    public class BuildingDataBase : ScriptableObject
    {
        public List<ObjectData> ObjectsData;
    }
}