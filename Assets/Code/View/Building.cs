using System;
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
    public int CurrentBuildingID;

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
        CurrentBuildingID = r.Next(0, 100);
    }


    public void SetAvailableToInstant(bool available)
    {
        if (available)
        {
            _renderer.material.color = Color.green;
        }
        else
        {
            _renderer.material.color = Color.red;
        }
    }

    public void SetNormalColor()
    {
        _renderer.material.color = Color.white;
    }

    public void SetPointDestination(Vector3 pointDestination)
    {
        _navMeshLink.endPoint = pointDestination;
    }
}