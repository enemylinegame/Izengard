using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResurseSystem
{
    
    public interface IResurse :IIconHolder
    {        
        public ResurseType ResurseType { get; }
        public string NameOFResurse { get; }
        public GoldCost CostInGold { get; }
        
        public void SetNameResurse(string name);
        public void SetIconOfResurses(Sprite icon);           

    }
}
