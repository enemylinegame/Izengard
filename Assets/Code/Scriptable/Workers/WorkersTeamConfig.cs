using UnityEngine;


[CreateAssetMenu(fileName = "WorkersTeam", menuName ="Workers team")]
public class WorkersTeamConfig : ScriptableObject
{
    [SerializeField]
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;

    [SerializeField]
    private float _timeToProcessWorkSec;
    public float TimeToProcessWork => _timeToProcessWorkSec;

    [SerializeField]
    private int _workersAmount;
    public int WorkersAmount => _workersAmount;

    [SerializeField]
    private GameObject _workerPrefab;
    public GameObject WorkerPrefab => _workerPrefab;
}
