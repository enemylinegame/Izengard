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
        private Button _waveStopButton;
        [SerializeField]
        private Button _defenderSpawnButton;

        public UnitSettingsPanel UnitSettings => _unitSettings;

        public Button WaveStartButton => _waveStartButton;
        public Button WaveStopButton => _waveStopButton;
        public Button DefenderSpawnButton => _defenderSpawnButton;
    }
}
