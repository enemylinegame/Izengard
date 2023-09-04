using System.Collections.Generic;
using EnemyUnit;

namespace WaveSystem.Interfaces
{
    public interface IWave
    {
        Queue<EnemyType> WaveUnits { get; }
    }
}
