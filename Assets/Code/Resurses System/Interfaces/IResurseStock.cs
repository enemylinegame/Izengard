using System.Collections.Generic;
using System;
using UnityEngine;

namespace ResurseSystem
{   
    public interface IResurseStock 
    {        
        public List<ResurseHolder> ResursesInStock { get; }        

        public float GetResursesCount(ResurseType type);
        public float GetResursesInStock(ResurseType type, float count);
        public void AddResursesFromHolder(IResurseHolder _getterHolder);
        public void AddResurseHolder(ResurseHolder holder);
        public void AddResursesCount(ResurseType res, float value);






    }
}
