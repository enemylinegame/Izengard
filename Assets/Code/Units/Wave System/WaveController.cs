using System;
using System.Collections.Generic;
using WaveSystem.Interfaces;
using WaveSystem.Model;

namespace WaveSystem
{
    public class WaveController : IOnController, IOnUpdate, IOnFixedUpdate, IDisposable
    {
        private readonly Dictionary<int, IWave> _wavesCollection 
            = new Dictionary<int, IWave>();

        private readonly EnemySpawnController _spawnController;

        private IWave _currentWave;
        public IWave CurrentWave => _currentWave;

        public WaveController(
            List<WaveSettings> waveSettings, 
            EnemySpawnController spawnController)
        {
            foreach(var waveData in waveSettings)
            {
                var waveModel = new WaveModel(waveData);
                _wavesCollection[waveData.WaveIndex] = waveModel;
            }

            _spawnController = spawnController;
        }

        public void StartWave(int index)
        {
            var wave = _wavesCollection[index];

            for(int i =0; i < wave.WaveUnits.Count; i++)
            {
                _spawnController.SpawnEnemy(wave.WaveUnits.Peek());
            }

            _currentWave = wave;
        }

        public void OnUpdate(float deltaTime)
        {
            _spawnController.OnUpdate(deltaTime);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            _spawnController.OnFixedUpdate(fixedDeltaTime);
        }

        public void Dispose()
        {
            _spawnController?.Dispose();
        }
    }
}
