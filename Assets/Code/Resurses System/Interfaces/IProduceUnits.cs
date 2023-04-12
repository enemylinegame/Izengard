using System;
using System.Collections;
using System.Collections.Generic;
using ResurseSystem;
using UnityEngine;

public interface IProduceUnits
{
    public List<GameObject> ProducedPrefab { get; }
    public int ProducedValue { get; }
    public float ProduceTime { get; }
    public ResurseCost ProduceCost { get; }    

}
