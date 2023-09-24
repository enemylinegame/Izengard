using System;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using BuildingSystem;
using Random = UnityEngine.Random;

public class Building : BaseBuildAndResources, ICollectable
{
    public int BuildingID { get; set; }
    public ResourceType ResourceType { get; set; }
    public BuildingTypes BuildingTypes { get; set; }
    public MineralConfig MineralConfig { get; set; }
    public GameObject Prefab { get; set; }
    public Sprite Icon { get; set; }
    public Vector3 SpawnPosition { get; set; }
    
    public float CollectTime { get; set; }
    public int MaxWorkers { get; set; }
    public int WorkersCount { get; set; }
    public string VisibleName { get; set; }
    public string Name { get; set; }

    public void InitBuilding()
    {
        BuildingID = Random.Range(0, 1000000);

        Prefab = gameObject;
        SpawnPosition = transform.position;
    }

    public void InitMineral(MineralConfig mineralConfig)
    {
        BuildingID = Random.Range(0, 1000000);
        new ResourceHolder(mineralConfig.ResourceType, mineralConfig.CurrentMineValue,mineralConfig.CurrentMineValue);
        MineralConfig = mineralConfig;
        ResourceType = mineralConfig.ResourceType;
        Prefab = gameObject;
        SpawnPosition = transform.position;
        MaxWorkers = 999999999;
    }

}