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
        private GameObject _container;
        [SerializeField]
        private int _containerItems;
        [SerializeField]
        private UnitPriorityUI _priorirtyUiPrefab;
        [SerializeField]
        private Button _plusButton;
        [SerializeField]
        private Button _minusButton;

        private IList<UnitPriorityData> _defaultPriorities;

        private List<UnitPriorityUI> _unitPriorityUIList = new();

        private int _itemsCount;
        
        public void Init(IList<UnitPriorityData> unitPriorities)
        {
            if (unitPriorities.Count > _containerItems)
                return;

            _itemsCount = 0;

            ResetUI();

            CreatePriorityItems(_containerItems);

            for (int i = 0; i < unitPriorities.Count; i++)
            {
                var unitPriority = _unitPriorityUIList[i];

                unitPriority.Init(unitPriorities[i].UnitPriority);

                unitPriority.SetActive(true);

                _itemsCount++;
            }

            if (_defaultPriorities == null)
            {
                _defaultPriorities = unitPriorities;
            }

            _plusButton.onClick.AddListener(AddPriority);
            _minusButton.onClick.AddListener(RemovePriority);
        }

        private void CreatePriorityItems(int itemsCount)
        {
            if(_unitPriorityUIList.Count != 0)
                return;

            for (int i = 0; i < itemsCount; i++)
            {
                var newPriority = CreatePriority(UnitPriorityType.None);
                newPriority.transform.SetParent(_container.transform, false);

                newPriority.SetActive(false);

                _unitPriorityUIList.Add(newPriority);
            }
        }

        public void ResetData()
        {
            ResetUI();

            Init(_defaultPriorities);
        }

        private void ResetUI()
        {
            for (int i = 0; i < _unitPriorityUIList.Count; i++)
            {
                var priority = _unitPriorityUIList[i];
                priority.ResetUI();
                priority.SetActive(false);
            }
        }

        public List<UnitPriorityData> GetPriorityData()
        {
            var result = new List<UnitPriorityData>();
            for(int i =0; i< _unitPriorityUIList.Count; i++)
            {
                var item = _unitPriorityUIList[i];
                if (item.gameObject.activeSelf)
                {
                    result.Add(new UnitPriorityData(item.Type, UnitType.None));
                }
            }

            return result;
        }

        private UnitPriorityUI CreatePriority(UnitPriorityType priorityType)
        {
            var prGo = Instantiate(_priorirtyUiPrefab);
            prGo.Init(priorityType);

            return prGo;
        }

        private void AddPriority()
        {
            if(_itemsCount < _unitPriorityUIList.Count)
            {
                var priority = _unitPriorityUIList[_itemsCount];
                priority.SetActive(true);

                _itemsCount++;
            }
        }

        private void RemovePriority()
        {
            if (_itemsCount > 0)
            {
                var priority = _unitPriorityUIList[_itemsCount - 1];
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

            for(int i = 0; i < _unitPriorityUIList.Count; i++)
            {
                Destroy(_unitPriorityUIList[i].gameObject);
            }
            _unitPriorityUIList.Clear();
        }
    }
}
