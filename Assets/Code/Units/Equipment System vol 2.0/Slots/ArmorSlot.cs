using System;
using System.Collections.Generic;
using UnityEngine;

namespace EquipmentSystem
{
    [Serializable]
    public class ArmorSlot : EquipmentSlot<ArmorModel>
    {
        [SerializeField] private List<ArmorBindingModel> _armorBindingModels;
        [SerializeField] private ArmorSlotTypes _slotType;
        [SerializeField] private ArmorModel baseModel;
        [SerializeField] private SkinnedMeshRenderer _currentSkinnedMeshRenderer;

        public ArmorModel BaseModel => baseModel;

        public override void EquipItem(ArmorModel newItemModel)
        {
            if (newItemModel.ArmorSlotTypeID != (int)_slotType)
            {
                Debug.Log("Данный предмет не подходит к этому слоту");
                return;
            }
            if (_currentSkinnedMeshRenderer == null)
            {
                _itemModel = new ArmorModel (newItemModel,_holderTransform);
            }
            else
            { 
                _itemModel = newItemModel;
            }
            ExpandSkinnedMesh();
            _unitView.RebindAnimation();
        }

        public override ArmorModel UnequipItem()
        {
            var previousModel = _itemModel;
            if (!(_itemModel is null))
            {
                EquipItem(baseModel);                
            }
            return previousModel;
        }
        public void AwakeModel()
        {
            if (_itemModel==null)
            { 
                EquipItem(baseModel);
            }
            
        }
        private void ExpandSkinnedMesh()
        {
            if (_currentSkinnedMeshRenderer==null)
            { 
                _itemModel.ItemObject.transform.position = _holderTransform.transform.position;
                _itemModel.ItemObject.transform.SetParent(_holderTransform);
                _currentSkinnedMeshRenderer = _itemModel.SkinnedMeshRenderer;
            }

            var bindingModel = SearchBindingModel(_itemModel.ArmorBindingsID);

            if (bindingModel is null)
            {
                throw new Exception($"Нет подходящего набора костей для развертки");
            }
            _currentSkinnedMeshRenderer.sharedMesh = _itemModel.SkinnedMeshRenderer.sharedMesh;
            _currentSkinnedMeshRenderer.rootBone = bindingModel.RootBone;
            _currentSkinnedMeshRenderer.bones = bindingModel.BonesForBinding;
        }

        private ArmorBindingModel SearchBindingModel(int armorBindingsID)
        {
            ArmorBindingModel armorBindingModel = null;
            for (int i = 0; i < _armorBindingModels.Count; i++)
            {
                if (_armorBindingModels[i].ArmorBindingID == armorBindingsID)
                {
                    armorBindingModel = _armorBindingModels[i];
                }
            }
            return armorBindingModel;
        }
                
    }
}
