using Code.TileSystem;
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
        private int _currentCurrentUnits;
        public BuildingTypes BuildingType { get; set; }
        public ResourceType ResourceType { get; set; }
        public int BuildingID { get; set; }

        public Image Icon => _icon;
        public TMP_Text Type => _type;
        public TMP_Text UnitsBusy => _unitsBusy;
        public Button DestroyBuildingInfo => _destroyBuildingInfo;
        public Button PlusUnit => _plusUnit;
        public Button MinusUnit => _minusUnit;
        public int CurrentUnits { get => _currentCurrentUnits; set => _currentCurrentUnits = value; }
    }
}