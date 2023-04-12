using System;
using UnityEngine;

namespace EquipmentSystem
{
    [Serializable]
    public sealed class ArmorBindingModel
    {
        [SerializeField] private ArmorBindingsTypes _armorBindingsType;
        [SerializeField] private Transform[] _bonesForBinding;
        [SerializeField] private Transform _rootBone;

        public int ArmorBindingID => (int)_armorBindingsType;
        public Transform[] BonesForBinding => _bonesForBinding;
        public Transform RootBone => _rootBone;
    }
}
