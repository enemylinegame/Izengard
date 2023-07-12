using Code.BuildingSystem;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.BuildBuildingsUI
{
    public class BuildingUIInfo : MonoBehaviour, IbuildingCollectable
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _type;
        [SerializeField] private TMP_Text _unitsBusy;
        [SerializeField] private Button _destroyBuildingInfo;
        [SerializeField] private Button _plusUnit;
        [SerializeField] private Button _minusUnit;

        private int _maxWorkers;
        private int _workersAccount;

        public BuildingTypes BuildingType { get; set; }
        public ResourceType ResourceType { get; set; }
        public int BuildingID { get; set; }

        public Image Icon => _icon;
        public TMP_Text Type => _type;
        public Button DestroyBuildingInfo => _destroyBuildingInfo;
        public Button PlusUnitButton => _plusUnit;
        public Button MinusUnitButton => _minusUnit;

        public int MaxWorkers
        {
            set
            {
                _maxWorkers = value;
                SetBuisyUnitsText();
            }
        }

        public int WorkersAccount
        {
            set 
            {
                _workersAccount = value;
                SetBuisyUnitsText();
            }
        }

        private void SetBuisyUnitsText()
        {
            _unitsBusy.text = $"{_workersAccount}/{_maxWorkers}";
        }
    }
}