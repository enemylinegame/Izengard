using BrewSystem.Model;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrewSystem.UI
{
    public class IngridientUI : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private Button _clickedButon;
        [SerializeField]
        private TMP_Text _name;
        [SerializeField]
        private Image _icone;
        [SerializeField]
        private GameObject _selectedIcon;

        private int _id;
        public int Id => _id;

        public Action<int> OnClicked;

        public void InitUI(IngridientModel model)
        {
            _id = model.Id;
            _name.text = model.Data.Name;
            _icone.sprite = model.Data.Icon;
            _clickedButon.onClick.AddListener(() => OnClicked?.Invoke(_id));

            ChangeSelection(false);
        }

        public void ChangeSelection(bool state)
        {
            _selectedIcon.SetActive(state);
        }

        public void Dispose()
        {
            _clickedButon.onClick.RemoveAllListeners();
        }
    }
}
