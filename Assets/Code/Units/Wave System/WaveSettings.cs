using UnityEngine;

namespace WaveSystem
{
    public class WaveSettings : ScriptableObject
    {
        [field: SerializeField] public int WaveIndex { get; private set; }
    }
}
