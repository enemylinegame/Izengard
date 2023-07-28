using System;
using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = nameof(GlobalResourceData), menuName = "Resource System/" + nameof(GlobalResourceData), order = 0)]
    [Serializable]
    public class GlobalResourceData : ScriptableObject
    {
        public InitialResourcesList InitialResourceData;
     
        public ResourceList ResourcesData;
    }
}