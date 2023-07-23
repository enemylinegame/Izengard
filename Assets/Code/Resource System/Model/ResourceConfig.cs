
using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = "ResourceConfig", menuName = "Resщurсe System/ResourceConfig", order = 1)]
    [System.Serializable]
    public class ResourceConfig : ScriptableObject
    {
        
        public ResourceType ResourceType => _resourceType;
        public string ItemName => _itemName;
        public Sprite Icon => _icon;
        public int MaxHoldedAmount => _maxHoldedAmount;
     
        

        [SerializeField]
        private ResourceType _resourceType;
        [SerializeField]
        private string _itemName;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private int _maxHoldedAmount;

    }
}

