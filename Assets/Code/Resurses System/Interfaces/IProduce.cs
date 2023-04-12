using BuildingSystem;
using ResurseSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProduce
{
    public ResurseCost NeeddedResursesForProduce { get; }    
    public float ProducingTime { get; }
    public float CurrentProduceTime { get; }
    public int ProducedValue { get; }
    public bool autoProduce { get; }

    public void StartProduce(float time);
    public void CheckResurseForProduce();
    public void SetAutoProduceFlag();
    public void GetResurseForProduce();
}
