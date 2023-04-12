using System.Collections;
using System.Collections.Generic;
using ResurseSystem;
using UnityEngine;

namespace BuildingSystem
{ 
    public interface IResurseStockHolder
    {
        public ResurseStock ThisBuildingStock { get; }

        public void AddResurseInStock(IResurseHolder holder);

        public IResurseHolder GetResursesInStock(IResurse resurse, float value);
    }
}
