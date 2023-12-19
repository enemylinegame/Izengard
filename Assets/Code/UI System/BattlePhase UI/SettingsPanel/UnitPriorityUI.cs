using UnityEngine;
using TMPro;
using UnitSystem.Enum;
using System.Collections.Generic;

namespace UI
{
    public class UnitPriorityUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown _priorityDropDown;

        private UnitPriorityType _type = UnitPriorityType.None;

        public UnitPriorityType Type => _type;

        public void Init(UnitPriorityType priorityType)
        {
            _type = priorityType;

            _priorityDropDown.ClearOptions();

            var optData = new List<string>
            {
                nameof(UnitPriorityType.None),
                nameof(UnitPriorityType.MainTower),
                nameof(UnitPriorityType.ClosestFoe),
                nameof(UnitPriorityType.FarthestFoe)
            };

            _priorityDropDown.AddOptions(optData);

            _priorityDropDown.value = (int)_type;
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }

        public void ResetUI()
        {
            _priorityDropDown.value = (int)UnitPriorityType.None;
        }

    }
}
