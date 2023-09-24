using System.Collections.Generic;
using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = nameof(InitialResourcesList), menuName = "Resource System/" + nameof(InitialResourcesList), order = 0)]
    public class InitialResourcesList: ScriptableObject
    {
        public List<InitialResourceData> InitialResources;
    }
}
