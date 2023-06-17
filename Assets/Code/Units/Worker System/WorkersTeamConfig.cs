using UnityEngine;


[CreateAssetMenu(fileName = "WorkersTeam", menuName ="Workers team")]
public class WorkersTeamConfig : ScriptableObject
{
    [SerializeField]
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;


    [SerializeField]
    private GameObject _workerPrefab;
    public GameObject WorkerPrefab => _workerPrefab;

    [Header("Mine worker")]
    [SerializeField]
    private float _smokeBreakTime;
    public float SmokeBreakTime => _smokeBreakTime;

    [SerializeField]
    private float _timeToProcessWorkSec;
    public float TimeToProcessWork => _timeToProcessWorkSec;

    [SerializeField]
    private int _portionSize;
    public int MineWorkerPortionSize => _portionSize;

    [Header("Craft worker")]
    [SerializeField]
    private float _craftWorkerPerformance;//обсудить с Николаем
    public float CraftWorkerPerformance => _craftWorkerPerformance;
}
