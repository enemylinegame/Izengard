using System.Collections.Generic;
using EnemyUnit;
using WaveSystem.Interfaces;

namespace WaveSystem.Model
{
    public class WaveModel : IWave
    {
        private readonly Queue<EnemyType> _waveUnits;
        public Queue<EnemyType> WaveUnits => _waveUnits;

        public WaveModel() 
        {

        }
    }
}
