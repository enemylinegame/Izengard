using System;
using System.Collections.Generic;
using Tools;
using UI;
using UnityEngine;

namespace SpawnSystem
{
    public class EnemySpawnHandler
    {
        private readonly ISpawnController _spawnController;
        private readonly IReadOnlyList<WaveData> _waves;
        private readonly BattleSceneUI _battleUI;

        private WaveData _currentWave;
        private int _waveIndex;
        private TimeRemaining _timer;
        private bool _isTiming;
     
 
        public WaveData CurrentWave => _currentWave;
        public event Action OnWavesEnd;

        public EnemySpawnHandler(
            ISpawnController spawnController, 
            WaveSettings waveSettings, 
            BattleSceneUI battleUI)
        {
            _spawnController = spawnController;
            _waves = waveSettings.Waves;
            _battleUI = battleUI;

            _waveIndex = 0;
            
            _currentWave = _waves[_waveIndex];

            _timer = new TimeRemaining(ExecuteWaveLogic, _currentWave.WaveDuration, true);

            _battleUI.EnemyWaveStartButton.onClick.AddListener(() => StartWave());
            _battleUI.EnemyWaveStopButton.onClick.AddListener(() => StopWave());
        }

        public void StartWave()
        {
            if (!_isTiming)
            {
                TimersHolder.AddTimer(_timer);
                _isTiming = true;

                Debug.Log("Enemy wave started!");
            }          
        }

        public void StopWave()
        {
            if (_isTiming)
            {
                _isTiming = false;

                TimersHolder.RemoveTimer(_timer);
              
                _waveIndex = 0;
                _currentWave = _waves[_waveIndex];
                _timer = new TimeRemaining(ExecuteWaveLogic, _currentWave.WaveDuration, true);

                Debug.Log("Enemy wave stoped!");
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
   
            if (_waveIndex >= _waves.Count)
            {
                StopWave();
                OnWavesEnd?.Invoke();
                return;
            }

            var prevWaveData = _currentWave;
            _currentWave = _waves[_waveIndex];

            if (prevWaveData.WaveDuration != _currentWave.WaveDuration)
            {
                _timer.ChangeDuration(_currentWave.WaveDuration);
            }
        }
    }
}
