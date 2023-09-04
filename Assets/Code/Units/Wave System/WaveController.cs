using System;
using System.Collections.Generic;
using WaveSystem.Interfaces;

namespace WaveSystem
{
    public class WaveController : IOnController, IOnUpdate, IOnFixedUpdate, IDisposable
    {
        private readonly Dictionary<int, IWave> _wavesCollection 
            = new Dictionary<int, IWave>();

       // private readonly IWaveGathering _waveGathering;
        private readonly EnemySpawnController _enemySpawnController;

        private IWave _currentWave;
        public IWave CurrentWave => _currentWave;

        public WaveController()
        {
           
        }

        public void StartWave(int index)
        {
            var wave = _wavesCollection[index];

            for(int i =0; i < wave.WaveUnits.Count; i++)
            {
                _enemySpawnController.SpawnEnemy(wave.WaveUnits.Peek());
            }

            _currentWave = wave;
        }

        public void OnUpdate(float deltaTime)
        {
            _enemySpawnController.OnUpdate(deltaTime);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            _enemySpawnController.OnFixedUpdate(fixedDeltaTime);
        }

        public void Dispose()
        {
            _enemySpawnController?.Dispose();
        }
    }
}
