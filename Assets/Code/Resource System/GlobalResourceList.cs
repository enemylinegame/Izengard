using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = "Resources", menuName = "Resource System/ResourceList", order = 0)]
    [Serializable]
    public class GlobalResourceList : ScriptableObject
    {
        public List<ResourceConfig> GlobalResourceConfigs;
    }
}