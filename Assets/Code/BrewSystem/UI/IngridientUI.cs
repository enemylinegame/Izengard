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
        private Button _selecttionButon;
        [SerializeField]
        private TMP_Text _name;
        [SerializeField]
        private Image _icone;
        [SerializeField]
        private GameObject _unselectedView;
        [SerializeField]
        private GameObject _selectedView;

        private int _id;
        private bool _isSelected;

        public Action<int> OnSelected;
        public Action<int> OnUnSelected;

        public void InitUI(IngridientModel model)
        {
            _id = model.Id;
            _name.text = model.Data.Name;
            _icone.sprite = model.Data.Icon;

            ChangeSelection();

            _selecttionButon.onClick.AddListener(ChangeSelection);
        }

        public void ChangeSelection()
        {
            _unselectedView.SetActive(!_isSelected);
            _selectedView.SetActive(_isSelected);

            if(_isSelected)
                OnSelected?.Invoke(_id);
            else
                OnUnSelected?.Invoke(_id);

            _isSelected = !_isSelected;
        }

        public void Dispose()
        {
            _selecttionButon.onClick.RemoveAllListeners();
        }
    }
}
