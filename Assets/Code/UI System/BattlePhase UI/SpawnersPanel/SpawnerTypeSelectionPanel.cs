using System;
using UnitSystem.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SpawnerTypeSelectionPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _enemyTypeSelectButton;
        [SerializeField]
        private Button _defenderTypeSelectButton;

        private Action<UnitFactionType> _callBackAction;

        public void Enable(Action<UnitFactionType> callBackAction)
        {
            _callBackAction = callBackAction;
            _enemyTypeSelectButton.onClick.AddListener(EnemyTypeSelected);
            _defenderTypeSelectButton.onClick.AddListener(DefenderTypeSelected);

            Show();
        }

        public void Disable() 
        {
            _callBackAction = null;

            _enemyTypeSelectButton.onClick.RemoveListener(EnemyTypeSelected);
            _defenderTypeSelectButton.onClick.RemoveListener(DefenderTypeSelected);

            Hide();
        }

        private void EnemyTypeSelected()
        {
            _callBackAction?.Invoke(UnitFactionType.Enemy);
        }

        private void DefenderTypeSelected()
        {
            _callBackAction?.Invoke(UnitFactionType.Defender);
        }

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
