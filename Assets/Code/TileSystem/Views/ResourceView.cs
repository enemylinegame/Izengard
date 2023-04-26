using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Code.TileSystem
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resourceText;
        [SerializeField] private TMP_Text _resourceCurrentValue;
        [SerializeField] private TMP_Text _resourceMaxValue;

        [SerializeField] private Button _resourceAddButton;
        [SerializeField] private Button _resourceRemoveButton;

        public string ResourceCurrentValueString
        {
            get { return _resourceCurrentValue.text; }
            set { _resourceCurrentValue.text = value; }
        }
        public int ResourceCurrentValueInt
        {
            get { return Convert.ToInt32(_resourceCurrentValue.text); }
        }
        public string ResourceTextString
        {
            get { return _resourceText.text; }
            set { _resourceText.text = value; }
        }
        public Button ResourceAddButton
        {
            get { return _resourceAddButton; }
        }
        public Button ResourceRemoveButton
        {
            get { return _resourceRemoveButton; }
        }

        public void InitViewData()
        {
            _resourceCurrentValue.text = "0";
            _resourceMaxValue.text = "5";
        }
    }
}

