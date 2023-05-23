using System;
using System.Security.Cryptography.X509Certificates;
using Code.BuldingsSystem;
using Code.TileSystem;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Random = System.Random;

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
    public string VisibleName { get; set; }
    public string NameBuiding { get; set; }
    
    public void InitBuilding()
    {
        BuildingID = UnityEngine.Random.Range(0, 1000000);

        Prefab = gameObject;
        SpawnPosition = transform.position;
    }

    public void InitMineral(MineralConfig mineralConfig)
    {
        BuildingID = UnityEngine.Random.Range(0, 1000000);
        new ResourceHolder(mineralConfig.ResourceType, mineralConfig.CurrentMineValue,mineralConfig.CurrentMineValue);
        MineralConfig = mineralConfig;
        ResourceType = mineralConfig.ResourceType;
        Prefab = gameObject;
        SpawnPosition = transform.position;
    }

}