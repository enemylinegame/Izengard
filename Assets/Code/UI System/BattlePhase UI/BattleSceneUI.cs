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
        private Button _resetButton;

        public UnitSettingsPanel UnitSettings => _unitSettings;
        public SpawnPanelUI SpawnPanel => _spawnPanel;
        public SpawnerTypeSelectionPanel SpawnerTypeSelection => _spawnerTypeSelection;
        public UnitStatsPanel UnitStatsPanel => _unitStatsPanel;
        public Button StartButton => _startButton;
        public Button PauseButton => _pauseButton;
        public Button ResetButton => _resetButton;

        
    }
}
