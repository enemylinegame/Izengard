using System.Collections.Generic;
using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = nameof(ResourceList), menuName = "Resource System/" + nameof(ResourceList), order = 0)]
    public class ResourceList : ScriptableObject
    {
        public List<ResourceConfig> Resources;
    }
}
