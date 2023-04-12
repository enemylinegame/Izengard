using Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wave
{
    public interface IWaveGathering : IDisposable
    {
        List<IPoolController<IEnemyController>> GetEnemysList(int waveNumber, bool isDowntime);
    }
}