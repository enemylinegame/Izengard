using UnityEngine;

namespace ResourceSystem
{
    
    public interface IResurse :IIconHolder
    {        
        public ResourceType ResourceType { get; }
        public string NameOFResurse { get; }
        public GoldCost CostInGold { get; }
        
        public void SetNameResurse(string name);
        public void SetIconOfResurses(Sprite icon);           

    }
}
