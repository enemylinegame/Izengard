using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem.MainTower
{
    public class MainTowerUI : MonoBehaviour
    {
        [SerializeField]
        private Button _fireButton;

        public event Action OnFirePressed;


        public void InitUI()
        {
            _fireButton.onClick.AddListener(() => OnFirePressed?.Invoke());

            Hide();
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
