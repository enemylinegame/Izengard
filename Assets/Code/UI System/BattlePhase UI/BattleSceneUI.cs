using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BattleSceneUI : MonoBehaviour
    {
        [SerializeField]
        private Button _enemyWaveStartButton;

        [SerializeField]
        public Button _enemyWaveStopButton;

        [SerializeField]
        private Button _defenderSpawnButton;

        public event Action OnEnemyWaveStartClick;
        public event Action OnEnemyWaveStopClick;
        public event Action OnDefenderSpawnClick;

        private void Awake()
        {
            _enemyWaveStartButton.onClick.AddListener(() => OnEnemyWaveStartClick?.Invoke());
            _enemyWaveStopButton.onClick.AddListener(() => OnEnemyWaveStopClick?.Invoke());
            _defenderSpawnButton.onClick.AddListener(() => OnDefenderSpawnClick?.Invoke());
        }

        private void OnDestroy()
        {
            _enemyWaveStartButton.onClick.RemoveAllListeners();
            _enemyWaveStopButton.onClick.RemoveAllListeners();
            _defenderSpawnButton.onClick.RemoveAllListeners();
        }

    }
}
