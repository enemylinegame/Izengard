using UnityEngine;

public class WorkerHousePlace : MonoBehaviour
{
    [SerializeField]
    private Transform[] _workersHousePlaces;

    public Vector3 this[int index]
    {
        get => _workersHousePlaces[index].position;
    }

    public int Length 
    {
        get => _workersHousePlaces.Length;
    }
}
