using UnityEngine;
using UnityEngine.AI;

public class Building : BaseBuildAndResources
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private NavMeshLink _navMeshLink;

    public string BuildingID;

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