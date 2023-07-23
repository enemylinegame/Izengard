using UnityEngine;

public class HouseAnimation : MonoBehaviour, IWorkerPreparation
{
    [Header("Smoke")]
    [SerializeField]
    private GameObject _particleSystem;

    private int _workersCount = 0;

    private void Awake()
    {
        _particleSystem.SetActive(false);
    }

    public void AfterWork()
    {
        if (0 == _workersCount)
            return;

        --_workersCount;

        if (0 == _workersCount)
            _particleSystem.SetActive(false);
    }

    public void BeforWork()
    {
        if (0 == _workersCount)
            _particleSystem.SetActive(true);
        
        ++_workersCount;
    }
}
