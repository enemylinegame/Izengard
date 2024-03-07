using AudioSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BattlePanelUI : MonoBehaviour
    {
        [SerializeField]
        private UnitSettingsPanel _unitSettingsPanel;
        [SerializeField]
        private SpawnPanelUI _spawnPanel;
        [SerializeField]
        private SpawnerTypeSelectionPanel _spawnerTypeSelection;
        [SerializeField]
        private UnitStatsPanel _unitStatsPanel;
        [SerializeField]
        private Button _startButton;
        [SerializeField]
        private Button _pauseButton;
        [SerializeField]
        private Button _resumeButton;
        [SerializeField]
        private Button _resetButton;
        [SerializeField] 
        private AudioSourceComponent _uIAudioSource;
        
        public UnitSettingsPanel UnitSettingsPanel => _unitSettingsPanel;
        public SpawnPanelUI SpawnPanel => _spawnPanel;
        public SpawnerTypeSelectionPanel SpawnerTypeSelection => _spawnerTypeSelection;
        public UnitStatsPanel UnitStatsPanel => _unitStatsPanel;
        public Button StartButton => _startButton;
        public Button PauseButton => _pauseButton;
        public Button ResumeButton => _resumeButton;
        public Button ResetButton => _resetButton;
        public IAudioSource UIAudioSource => _uIAudioSource;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}
