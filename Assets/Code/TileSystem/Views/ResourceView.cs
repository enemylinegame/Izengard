using System;
using Code.BuldingsSystem;
using Code.TileSystem.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ResourceSystem;

namespace Code.TileSystem
{
    public class ResourceView : MonoBehaviour,IbuildingCollectable
    {
        [SerializeField] private TMP_Text _resourceText;
        [SerializeField] private TMP_Text _resourceCurrentValue;
        [SerializeField] private TMP_Text _resourceMaxValue;

        [SerializeField] private Button _resourceAddButton;
        [SerializeField] private Button _resourceRemoveButton;
        
        public BuildingTypes BuildingType { get; set; }
        public ResourceType ResourceType { get; set; }
        
        public ICollectable Building { get; set; }

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


        public void InitViewData(string resourceName, int currentValue, ICollectable mineralConfig)
        {
            _resourceCurrentValue.text = $"{currentValue}";
            _resourceText.text = resourceName;
            _resourceMaxValue.text = "5";

            ResourceType = mineralConfig.ResourceType;
            Building = mineralConfig;
        }
    }
}

