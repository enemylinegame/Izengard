using BuildingSystem;
using EquipmentSystem;
using ResourceSystem;
using ResourceSystem.SupportClases;
using TMPro;
using UnityEngine;

public class WorkerView : UnitView
{
    [SerializeField] private ResourceHolder _Resholder;
    [SerializeField] private ItemСarrierHolder _Itemholder;
    [SerializeField] private float _currentMineTime;
    private BuildingTypes _type;

    public BuildingTypes AssignedResource
    {
        get => _type;
        set => _type = value;
    }

   /* public void GetResurseOutOfHolder(WareHouseBuildModel model)
    {
        model.AddInStock(_Resholder);
    }
    public void GetItemOutOfHolder (WareHouseBuildModel model)
    {
        model.AddInStock(_Itemholder);
    }*/
    public void MineResurse (ResourceMine mine,float time)
    {
        _currentMineTime += time;
        if (_currentMineTime>=mine.ExtractionTime)
        {
            _Resholder = mine.MineResource();
            _currentMineTime = 0;
        }
    }
   
}
