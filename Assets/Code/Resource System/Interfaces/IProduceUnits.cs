using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

public interface IProduceUnits
{
    public List<GameObject> ProducedPrefab { get; }
    public int ProducedValue { get; }
    public float ProduceTime { get; }
    public ResourceCost ProduceCost { get; }    

}
