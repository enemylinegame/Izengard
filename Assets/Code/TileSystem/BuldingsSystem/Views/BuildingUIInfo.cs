using Code.TileSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.BuildBuildingsUI
{
    public class BuildingUIInfo : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _type;
        [SerializeField] private TMP_Text _unitsBusy;
        [SerializeField] private Button _destroyBuildingInfo;
        [SerializeField] private Button _plusUnit;
        [SerializeField] private Button _minusUnit;
        private BuildingTypes _buildingTypes;
        private int _units;

        public Image Icon => _icon;
        public TMP_Text Type => _type;
        public TMP_Text UnitsBusy => _unitsBusy;
        public Button DestroyBuildingInfo => _destroyBuildingInfo;
        public Button PlusUnit => _plusUnit;
        public Button MinusUnit => _minusUnit;
        public int Units
        {
            get => _units;
            set => _units = value;
        }

        public BuildingTypes Types
        {
            get => _buildingTypes;
            set => _buildingTypes = value;
        }
        
        public void Hiring(bool isOn, TileUIController controller)
        {
            if (isOn)
            {
                if (_units < 5)
                {
                    controller.hiringUnits(1);
                    _units += 1;
                    _unitsBusy.text = _units + "/5";
                    controller.View.EightQuantity += 1;
                }
                

            }
            else
            {
                if (_units > 0)
                {
                    controller.RemoveFromHiringUnits(1);
                    _units -= 1;
                    _unitsBusy.text = _units + "/5";
                    controller.View.EightQuantity -= 1;
                }
                
            }
        }
    }
}