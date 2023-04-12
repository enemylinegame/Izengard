
using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = "Resurse Craft", menuName = "Resurse System/ResurseCraft", order = 1)]
    [System.Serializable]
    public class ResurseCraft : ScriptableObject, IResurse
    {
        
        public ResourceType ResourceType => resourceType;
        public string NameOFResurse => _nameOFResurse;
        public Sprite Icon => _icon;
        public GoldCost CostInGold => _costInGold;
        

        [SerializeField]
        private ResourceType resourceType;
        [SerializeField]
        private string _nameOFResurse;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private GoldCost _costInGold;
        
        public ResurseCraft (ResurseCraft res)
        {
            resourceType = res.ResourceType;
            _nameOFResurse = res.NameOFResurse;
            _icon = res.Icon;
            _costInGold = res.CostInGold;
        }
        public void SetNameResurse(string name)
        {
            _nameOFResurse = name;
        }
        public void SetIconOfResurses(Sprite icon)
        {
            _icon = icon;
        }        
    }
}

