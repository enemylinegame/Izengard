using BuildingSystem;
using EquipmentSystem;
using ResourceSystem;
using UnityEngine;

public class WorkerView : UnitView
{
    [SerializeField]
    private ResourceHolder _Resholder;
    [SerializeField]
    private ItemСarrierHolder _Itemholder;

    [SerializeField] public ResourceType AssignedResource;
    
    private float _currentMineTime;

    public void MineResource (ResourceMine mine,float time)
    {
        _currentMineTime += time;
        if (_currentMineTime>=mine.ExtractionTime)
        {
            _Resholder = mine.MineResource();
            _currentMineTime = 0;
        }
    }
    public void GiveMeItem(BuildingModel building)
    {

    }
}
