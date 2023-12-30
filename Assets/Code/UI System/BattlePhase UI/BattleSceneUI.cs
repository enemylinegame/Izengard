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
        private Button _waveStartButton;
        [SerializeField]
        private Button _waveStopButton;
        [SerializeField]
        private Button _defenderSpawnButton;

        public UnitSettingsPanel UnitSettings => _unitSettings;
        public SpawnPanelUI SpawnPanel => _spawnPanel;
        public SpawnerTypeSelectionPanel SpawnerTypeSelection => _spawnerTypeSelection;
        public UnitStatsPanel UnitStatsPanel => _unitStatsPanel;
        public Button WaveStartButton => _waveStartButton;
        public Button WaveStopButton => _waveStopButton;
        public Button DefenderSpawnButton => _defenderSpawnButton;

        
    }
}
