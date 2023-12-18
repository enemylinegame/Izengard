using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BattleSceneUI : MonoBehaviour
    {
        [SerializeField]
        private UnitSettingsPanel _unitSettings;
        [SerializeField]
        private Button _waveStartButton;
        [SerializeField]
        public Button _waveStopButton;


        public event Action OnWaveStart;
        public event Action OnWaveStop;

        public UnitSettingsPanel UnitSettings => _unitSettings;

        private void Awake()
        {
            _waveStartButton.onClick.AddListener(() => OnWaveStart?.Invoke());
            _waveStopButton.onClick.AddListener(() => OnWaveStop?.Invoke());
        }

        private void OnDestroy()
        {
            _waveStartButton.onClick.RemoveAllListeners();
            _waveStopButton.onClick.RemoveAllListeners();
        }

    }
}
