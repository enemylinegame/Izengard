using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemySettingsPanel : UnitSettingsPanel
    {
        [SerializeField]
        private Button _spawnButton;

        [SerializeField]
        private Button _waveStartButton;

        [SerializeField]
        public Button _waveStopButton;

        public event Action OnSpawn;
        public event Action OnWaveStart;
        public event Action OnWaveStop;

        protected override void PanelAwake()
        {
            _waveStartButton.onClick.AddListener(() => OnWaveStart?.Invoke());
            _waveStopButton.onClick.AddListener(() => OnWaveStop?.Invoke());
            _spawnButton.onClick.AddListener(() => OnSpawn?.Invoke());
        }

        protected override void PanelDestroy()
        {
            _waveStartButton.onClick.RemoveAllListeners();
            _waveStopButton.onClick.RemoveAllListeners();
            _spawnButton.onClick.RemoveAllListeners();
        }

        protected override void OpenPanel()
        {
            base.OpenPanel();

            panelRootTransform.DOMoveX(10, 0.2f, true);
        }

        protected override void ClosePanel()
        {
            base.ClosePanel();

            panelRootTransform.DOMoveX(-300, 0.2f, true);
        }
    }
}
