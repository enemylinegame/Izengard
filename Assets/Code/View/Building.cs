using System;
using Code.TileSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Random = System.Random;

public class Building : BaseBuildAndResources
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private NavMeshLink _navMeshLink;
    [SerializeField] private Image _icon;
    private BuildingTypes _buildingTypes;
    public int Units;
    public int BuildingID;

    public Image Icon
    {
        get => _icon;
        set => _icon = value;
    }

    public BuildingTypes Type
    {
        get => _buildingTypes;
        set => _buildingTypes = value;
    }

    private void Start()
    {
        var r = new Random();
        BuildingID = r.Next(0, 100);
    }
}