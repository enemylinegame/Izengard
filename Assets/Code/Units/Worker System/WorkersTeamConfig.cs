using UnityEngine;


[CreateAssetMenu(fileName = "WorkersTeam", menuName ="Workers team")]
public class WorkersTeamConfig : ScriptableObject
{
    [SerializeField]
    private GameObject _workerPrefab;
    [Header("Mine worker smoke break time")]
    [SerializeField, Range(0, 100)]
    private float _smokeBreakTime;
    
    [Header("Mine worker mining time"), SerializeField, Range(0, 100)]
    private float _timeToProcessWorkSec;
    
    [Header("Mine worker portion size"), SerializeField, Range(0, 100)]
    private int _portionSize;
    
    [Header("Craft worker performance"), SerializeField, Range(0, 100)]
    private float _craftWorkerPerformance;


    public float TimeToProcessWork => _timeToProcessWorkSec;
    public int MineWorkerPortionSize => _portionSize;
    public float SmokeBreakTime => _smokeBreakTime;
    public GameObject WorkerPrefab => _workerPrefab;
    public float CraftWorkerPerformance => _craftWorkerPerformance;
}
