using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using SpawnSystem;

namespace UI
{
    public class SpawnerHUD : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _spawnerIndexText;
        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _selectedBackground;
        [SerializeField]
        private GameObject _unselectedBackground;

        public string Id { get; private set; }

        public event Action<string> OnSelectAction;

        public void Init(string spawnerId, string name)
        {
            Id = spawnerId;
            _spawnerIndexText.text = name;

            _button.onClick.AddListener(SpawnerHUDSelection);

            Show();
        }

        public void Deinit()
        {
            _button.onClick.RemoveListener(SpawnerHUDSelection);

            Hide();
        }

        private void SpawnerHUDSelection()
        {
            OnSelectAction?.Invoke(Id);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Select() => SetSelected(true);
        public void Unselect() => SetSelected(false);

        private void SetSelected(bool isSelected)
        {
            _selectedBackground.SetActive(isSelected);
            _unselectedBackground.SetActive(!isSelected);
        }

        private void Awake()
        {
            Hide();
        }
    }
}
