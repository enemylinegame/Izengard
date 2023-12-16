using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DefenderSettingsPanel : UnitSettingsPanel
    {
        [SerializeField]
        private Button _spawnButton;

        public event Action OnSpawn;

        protected override void PanelAwake()
        {
            _spawnButton.onClick.AddListener(() => OnSpawn?.Invoke());
        }

        protected override void PanelDestroy()
        {
            _spawnButton.onClick.RemoveAllListeners();
        }

        protected override void OpenPanel()
        {
            base.OpenPanel();

            panelRootTransform.DOMoveX(Screen.width - 10, 0.2f, true);
        }

        protected override void ClosePanel()
        {
            base.ClosePanel();

            panelRootTransform.DOMoveX(Screen.width + 400, 0.2f, true);
        }
    }
}
