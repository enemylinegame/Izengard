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
        private Button _selectButton;
        
        public string Id { get; private set; }

        public event Action<string> OnSelectAction;

        public void Init(string spawnerId, string name) 
        {
            Id = spawnerId;
            _spawnerIndexText.text = name;

            _selectButton.onClick.AddListener(SpawnerHUDSelection);

            Show();
        }

        public void Deinit()
        {
            _selectButton.onClick.RemoveListener(SpawnerHUDSelection);

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

        private void Awake()
        {
            Hide();
        }
    }
}
