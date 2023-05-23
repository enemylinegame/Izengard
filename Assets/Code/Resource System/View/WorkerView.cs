using BuildingSystem;
using Code.BuldingsSystem;
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
    private BuildingTypes _buildingTypes;
    private ResourceType _resourceType;

    public BuildingTypes BuildingTypes
    {
        get => _buildingTypes;
        set => _buildingTypes = value;
    }

    public ResourceType ResourceType
    {
        get => _resourceType;
        set => _resourceType = value;
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
