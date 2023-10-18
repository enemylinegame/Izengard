using System;
using System.Collections.Generic;
using Tools;

namespace SpawnSystem
{
    public class EnemySpawnHandler
    {
        private readonly EnemySpawnController _spawnController;
        private readonly IReadOnlyList<WaveData> _waves;
       
        private WaveData _currentWave;
        private int _waveIndex;
        private TimeRemaining _timer;
        private bool _isTiming;
     
 
        public WaveData CurrentWave => _currentWave;
        public event Action OnWavesEnd;

        public EnemySpawnHandler(EnemySpawnController spawnController, WaveSettings waveSettings)
        {
            _spawnController = spawnController;
            _waves = waveSettings.Waves;

            _waveIndex = 0;
            
            _currentWave = _waves[_waveIndex];

            _timer = new TimeRemaining(ExecuteWaveLogic, _currentWave.WaveDuration, true);
        }

        public void StartSpawn()
        {
            if (!_isTiming)
            {
                TimersHolder.AddTimer(_timer);
                _isTiming = true;
            }
        }

        public void StopSpawn()
        {
            if (_isTiming)
            {
                TimersHolder.RemoveTimer(_timer);
                _isTiming = false;
                _waveIndex = 0;
            }
        }

        public void PauseSpawn()
        {
            if (_isTiming)
            {
                TimersHolder.RemoveTimer(_timer);
                _isTiming = false;
            }
        }


        private void ExecuteWaveLogic()
        {
            if(_currentWave == null)
            {
                _currentWave = _waves[_waveIndex];
            }
            

            for (int i = 0; i < _currentWave.InWaveUnits.Length; i++)
            {
                _spawnController.SpawnUnit(_currentWave.InWaveUnits[i]);
            }

            _waveIndex++;

            if (_waveIndex > (_waves.Count - 1))
            {
                StopSpawn();
                OnWavesEnd?.Invoke();
                return;
            }

            var prevWaveData = _currentWave;
            _currentWave = _waves[_waveIndex];
           
            if(prevWaveData.WaveDuration != _currentWave.WaveDuration)
            {
                _timer.ChangeDuration(_currentWave.WaveDuration);
            }
        }
    }
}
