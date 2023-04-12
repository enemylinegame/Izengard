using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResurseSystem;

namespace BuildingSystem
{ 
    public interface IProduct<M,T> where M:IHolder<T> where T:ScriptableObject
    {
        public M ProductHolder { get; }
        public ResurseCost ProductCost { get; }
        public float ProducingTime { get; }
        public float CurrentProduceTime { get; }
        public bool autoProduce { get; }
    }
}
