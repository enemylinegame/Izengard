using Interfaces;
using System;
using System.Collections.Generic;


namespace Wave
{
    public interface IWaveGathering : IDisposable
    {
        List<IPoolController<IEnemyController>> GetEnemysList(int waveNumber, bool isDowntime);
    }
}