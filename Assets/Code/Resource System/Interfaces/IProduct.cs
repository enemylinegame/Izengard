using ResourceSystem;
using UnityEngine;

namespace BuildingSystem
{ 
    public interface IProduct<M,T> where M:IHolder<T> where T:ScriptableObject
    {
        public M ProductHolder { get; }
        public ResourceCost ProductCost { get; }
        public float ProducingTime { get; }
        public float CurrentProduceTime { get; }
        public bool autoProduce { get; }
    }
}
