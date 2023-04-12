using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResurseSystem;

namespace BuildingSystem
{
    public interface IProduceResurse:IProduce
    {        
        public ResurseCraft ProducedResurse { get; }
        
        


    }
}
