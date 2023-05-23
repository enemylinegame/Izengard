using Code.BuldingsSystem;
using Code.TileSystem;
using Code.TileSystem.Interfaces;
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
        private int _units;
        public BuildingTypes BuildingType { get; set; }
        public ResourceType ResourceType { get; set; }

        public Image Icon => _icon;
        public TMP_Text Type => _type;
        public TMP_Text UnitsBusy => _unitsBusy;
        public Button DestroyBuildingInfo => _destroyBuildingInfo;
        public Button PlusUnit => _plusUnit;
        public Button MinusUnit => _minusUnit;
        public int Units { get => _units; set => _units = value; }


        /// <summary>
        /// найм юнитов для определеного типа здания
        /// </summary>
        public void Hiring(bool isOn, TileController controller, ICollectable building)
        {
            var hire = isOn 
                ? controller.WorkerMenager.UpdateWorkerAssignment(this, building) 
                : controller.WorkerMenager.RemoveWorkerAssignment(this, building);

            if (hire)
            {
                _units += isOn ? 1 : -1;
                _unitsBusy.text = $"{_units}/5";
            }
        }
    }
}