using BuildingSystem;
using System.Collections.Generic;
using EquipmentSystem;
using ResurseSystem;
using UnityEngine;

public class WorkerView : UnitView
{
    [SerializeField]
    private ResurseHolder _Resholder;
    [SerializeField]
    private ItemÑarrierHolder _Itemholder;
    [SerializeField]
    
    private float _currentMineTime;

    public void GetResurseOutOfHolder(WareHouseBuildModel model)
    {
        model.AddInStock(_Resholder);
    }
    public void GetItemOutOfHolder (WareHouseBuildModel model)
    {
        model.AddInStock(_Itemholder);
    }
    public void MineResurse (ResurseMine mine,float time)
    {
        _currentMineTime += time;
        if (_currentMineTime>=mine.ExtractionTime)
        {
            _Resholder = mine.MineResurse();
            _currentMineTime = 0;
        }
    }
    public void GiveMeItem(BuildingModel building)
    {

    }
}
