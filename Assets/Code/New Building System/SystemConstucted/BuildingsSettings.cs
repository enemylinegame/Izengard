using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewBuildingSystem
{
    [CreateAssetMenu(fileName = nameof(BuildingsSettings), menuName = "GameConfigs/" + nameof(BuildingsSettings))]
    public class BuildingsSettings : ScriptableObject
    {
        [Header("Common")]
        public List<ObjectData> objectsData;

        [Header("Resources")] 
        public int interactiveArea;
    }
}