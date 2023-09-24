using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem
{
    public class BuildingHUD : MonoBehaviour, IbuildingCollectable
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
                SetBusyUnitsText();
            }
        }

        public int WorkersСount
        {
            set 
            {
                _workersAccount = value;
                SetBusyUnitsText();
            }
        }

        private void SetBusyUnitsText()
        {
            _unitsBusy.text = $"{_workersAccount}/{_maxWorkers}";
        }
    }
}