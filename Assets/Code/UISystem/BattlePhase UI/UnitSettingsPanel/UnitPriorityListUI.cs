using System.Collections.Generic;
using UnitSystem.Data;
using UnitSystem.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UnitPriorityListUI : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;
        [SerializeField]
        private int _containerItems;
        [SerializeField]
        private UnitPriorityUI _priorirtyUiPrefab;
        [SerializeField]
        private Button _plusButton;
        [SerializeField]
        private Button _minusButton;

        private List<UnitPriorityUI> unitPriorityUIList;
        private int _itemsCount;
        
        public void Init(IList<UnitPriorityData> unitPriorities)
        {
            if (unitPriorities.Count > unitPriorityUIList.Count)
                return;

            for (int i = 0; i < unitPriorities.Count; i++)
            {
                var unitPriority = unitPriorityUIList[i];

                unitPriority.Init(unitPriorities[i].UnitPriority);

                unitPriority.SetActive(true);

                _itemsCount++;
            }
        }

        public List<UnitPriorityData> GetPriorityData()
        {
            var result = new List<UnitPriorityData>();
            for(int i =0; i< unitPriorityUIList.Count; i++)
            {
                var item = unitPriorityUIList[i];
                if (item.gameObject.activeSelf)
                {
                    result.Add(new UnitPriorityData(item.Type, UnitType.None));
                }
            }

            return result;
        }

        private void Awake()
        {
            unitPriorityUIList = new List<UnitPriorityUI>();
            for (int i = 0; i < _containerItems; i++)
            {
                var newPriority = CreatePriority(UnitPriorityType.None);
                newPriority.transform.SetParent(_container);

                newPriority.SetActive(false);

                unitPriorityUIList.Add(newPriority);
            }

            _plusButton.onClick.AddListener(AddPriority);
            _minusButton.onClick.AddListener(RemovePriority);

            _itemsCount = 0;
        }

        private UnitPriorityUI CreatePriority(UnitPriorityType priorityType)
        {
            var prGo = Instantiate(_priorirtyUiPrefab);
            prGo.Init(priorityType);

            return prGo;
        }

        private void AddPriority()
        {
            if(_itemsCount < unitPriorityUIList.Count)
            {
                var priority = unitPriorityUIList[_itemsCount];
                priority.SetActive(true);

                _itemsCount++;
            }
        }

        private void RemovePriority()
        {
            if (_itemsCount > 0)
            {
                var priority = unitPriorityUIList[_itemsCount - 1];
                priority.ResetUI();
                priority.SetActive(false);

                _itemsCount--;

                if(_itemsCount < 0)
                    _itemsCount = 0;
            }
        }


        private void OnDestroy()
        {
            _plusButton.onClick.RemoveListener(AddPriority);
            _minusButton.onClick.RemoveListener(RemovePriority);

            for(int i = 0; i < unitPriorityUIList.Count; i++)
            {
                Destroy(unitPriorityUIList[i].gameObject);
            }
            unitPriorityUIList.Clear();
        }
    }
}
