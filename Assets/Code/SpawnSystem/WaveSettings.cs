using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem
{
    [CreateAssetMenu(fileName = nameof(WaveSettings), menuName = "Spawn/" + nameof(WaveSettings))]
    public class WaveSettings : ScriptableObject
    {
        [SerializeField] private List<WaveData> _waves;

        public IReadOnlyList<WaveData> Waves => _waves;
    }
}
