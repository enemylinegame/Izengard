using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.TileSystem
{
    public class TileUIView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _LVLText;
        [SerializeField] private TMP_Text _nameTile;
        [SerializeField] private List<ButtonView> _buttonsHolder;
        [SerializeField] private TMP_Text _unitMax;

        private int _workersAccount;
        private int _maxWorkersAccount;

        public Image Icon => _icon;
        public TMP_Text LvlText => _LVLText;
        public TMP_Text NameTile
        {
            get => _nameTile;
            set => _nameTile = value;
        }

        public List<ButtonView> ButtonsHolder => _buttonsHolder;

        public int WorkersCount
        {
            set 
            {
                _workersAccount = value;
                SetWorkersAccountText();
            }
        }

        public int MaxWorkersCount
        {
            set 
            {
                _maxWorkersAccount = value;
                SetWorkersAccountText();
            }
        }

        private void SetWorkersAccountText()
        {
            _unitMax.text = $"{_workersAccount}/{_maxWorkersAccount} Units";
        }
    }
}