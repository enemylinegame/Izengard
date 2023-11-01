using UnityEngine;

namespace ResourceSystem
{
    [System.Serializable]
    public class InitialResourceData
    {
        [field: SerializeField] public ResourceType ResourceType { get; private set; }
        [field: SerializeField] public int Amount { get; private set; } = 0;
    }

}
