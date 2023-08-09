using UnityEngine;

public class HouseAnimation : MonoBehaviour, IWorkerPreparation
{
    [Header("Smoke")]
    [SerializeField]
    private GameObject _particleSystem;

    private int _workersCount = 0;
    private bool _isEnableToAll = true;

    private void Awake()
    {
        _particleSystem.SetActive(false);
    }

    public void Stop()
    {
        if (0 == _workersCount)
            return;

        --_workersCount;

        if (0 == _workersCount && _isEnableToAll)
            _particleSystem.SetActive(false);
    }

    public void Begin()
    {
        if (0 == _workersCount && _isEnableToAll)
            _particleSystem.SetActive(true);
        
        ++_workersCount;
    }

    public void EnableAll()
    {
        _isEnableToAll = true;

        if (_workersCount > 0)
            _particleSystem.SetActive(true);
    }

    public void DisableAll()
    {
        _isEnableToAll = false;
        _particleSystem.SetActive(false);
    }
}
