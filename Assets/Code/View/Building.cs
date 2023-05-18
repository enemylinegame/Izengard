using System;
using System.Security.Cryptography.X509Certificates;
using Code.TileSystem;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Random = System.Random;

public class Building : BaseBuildAndResources
{
    [field: SerializeField] public Image Icon { get; set; }
    [field: SerializeField] public BuildingTypes BuildingTypes { get; set; }
    [field: SerializeField] public ResourceType ResourceType { get; set; }
    [field: SerializeField] public MineralConfig MineralConfig { get; set; }
    
    public string NameBuiding { get; set; }
    public int BuildingID;
    
    public void InitBuilding() => BuildingID = UnityEngine.Random.Range(0, 1000000);
    public void InitMineral(MineralConfig mineralConfig)
    {
        BuildingID = UnityEngine.Random.Range(0, 1000000);
        new ResourceHolder(mineralConfig.ResourceType, mineralConfig.CurrentMineValue,mineralConfig.CurrentMineValue);
        MineralConfig = mineralConfig;
        ResourceType = mineralConfig.ResourceType;
    }
}