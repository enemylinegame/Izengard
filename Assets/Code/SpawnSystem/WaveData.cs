using System;
using UnitSystem.Enum;

namespace SpawnSystem
{
    [Serializable]
    public class WaveData
    {
        public string WaveName;
        public float WaveDuration = 10f;
        public UnitRoleType[] InWaveUnits;
    }
}
