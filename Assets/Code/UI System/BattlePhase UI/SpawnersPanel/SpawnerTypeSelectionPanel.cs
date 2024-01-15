using Abstraction;
using System;
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

        private Action<FactionType> _callBackAction;

        public void Enable(Action<FactionType> callBackAction)
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
            _callBackAction?.Invoke(FactionType.Enemy);
        }

        private void DefenderTypeSelected()
        {
            _callBackAction?.Invoke(FactionType.Defender);
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
