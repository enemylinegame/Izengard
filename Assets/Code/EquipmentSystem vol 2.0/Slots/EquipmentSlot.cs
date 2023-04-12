using System;
using UnityEngine;

namespace EquipmentSystem
{
    [Serializable]
    public abstract class EquipmentSlot<T> where T : ItemModel
    {
        protected T _itemModel;
        protected  UnitView _unitView;
        [SerializeField] protected Transform _holderTransform;
        

        public T ItemModel => _itemModel;
        public Transform HolderTransform=> _holderTransform;

        public abstract void EquipItem(T itemModel);
        public abstract T UnequipItem();
        public void SetUnitView(UnitView unit)
        {
            _unitView = unit;
        }
        
    }
}