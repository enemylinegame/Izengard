using Code.TileSystem;
using ResourceSystem;
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
        private BuildingTypes _Types;
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
            get => _Types;
            set => _Types = value;
        }
        
        public void Hiring(bool isOn, TileController controller, Building building)
        {
            if (isOn)
            {
                if (_units < 5)
                {
                    controller.hiringUnits(1);
                    controller.View.UpdateWorkerAssigment(_Types, building);
                    _units += 1;
                    _unitsBusy.text = _units + "/5";
                }
                

            }
            else
            {
                if (_units > 0)
                {
                    controller.View.RemoveWorkerAssigment(_Types, building);
                    controller.RemoveFromHiringUnits(1);
                    _units -= 1;
                    _unitsBusy.text = _units + "/5";
                }
                
            }
        }
    }
}