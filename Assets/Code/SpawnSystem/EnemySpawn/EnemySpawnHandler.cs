using System;
using System.Collections.Generic;
using Tools;
using UI;
using UnitSystem.Enum;
using UnitSystem;
using UnityEngine;

namespace SpawnSystem
{
    public class EnemySpawnHandler
    {
        private readonly ISpawnController _spawnController;
        private readonly IReadOnlyList<WaveData> _waves;
        private readonly BattleUIController _battleUIController;

        private WaveData _currentWave;
        private int _waveIndex;
        private TimeRemaining _timer;
        private bool _isTiming;
     
 
        public WaveData CurrentWave => _currentWave;
        public event Action OnWavesEnd;

        public EnemySpawnHandler(
            ISpawnController spawnController, 
            WaveSettings waveSettings,
            BattleUIController battleUIController)
        {
            _spawnController = spawnController;
            _waves = waveSettings.Waves;
            _battleUIController = battleUIController;
 
            _battleUIController.OnStartWave += StartWave;
            _battleUIController.OnStopWave += StopWave;
            _battleUIController.OnSpawnNewUnit += SpawnUnit;

            _waveIndex = 0;
            
            _currentWave = _waves[_waveIndex];

            _timer = new TimeRemaining(ExecuteWaveLogic, _currentWave.WaveDuration, true); 
        }

        public void SpawnUnit(IUnitData unitData)
        {
            if (unitData.Faction != UnitFactionType.Enemy)
                return;

            _spawnController.SpawnUnit(unitData);
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
