using System.Collections.Generic;
using BattleSystem;
using SpawnSystem;
using Tools;
using Tools.Navigation;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

public class UnitTestEntry : MonoBehaviour
{
    [SerializeField] private MainTowerView _mainTower;
    [Header("Enemy Spawn Parametrs")]
    [SerializeField] private Transform _spawner;
    [SerializeField] private WaveSettings _waveSettings;
    [SerializeField] private SpawnSettings _enemySpawnSettings;
    [SerializeField] private List<Transform> _enemySpawnPoints;

    [Header("Defender Spawn Parametrs")]
    [SerializeField] private SpawnSettings _defenderSpawnSettings;
    [SerializeField] private List<Transform> _defenderSpawnPoints;
    [Space(10)]
    [SerializeField] private NavigationSurfaceView _groundSurface;

    private TimeRemainingController timeRemainingController;

    private NavigationUpdater _navigationUpdater;
    private EnemySpawnController _enemySpawnController;
    private DefenderSpawnTestController _defenderSpawnController;
    private TargetFinder _targetFinder;
    private EnemyBattleController _enemyBattleController;

    private EnemySpawnHandler _enemySpawnHandler;

    private List<IOnUpdate> _onUpdates = new List<IOnUpdate>();
    private List<IOnFixedUpdate> _onFixedUpdates = new List<IOnFixedUpdate>();

    private void Start()
    {
        timeRemainingController = new TimeRemainingController();
        _onUpdates.Add(timeRemainingController);

        _navigationUpdater = new NavigationUpdater();
        _navigationUpdater.AddNavigationSurface(_groundSurface);
        
        _enemySpawnController = new EnemySpawnController(_spawner, _enemySpawnPoints, _enemySpawnSettings);
        _enemySpawnController.OnUnitSpawned += OnCreatedUnit;
        _onUpdates.Add(_enemySpawnController);

        _defenderSpawnController = new DefenderSpawnTestController(_defenderSpawnPoints, _defenderSpawnSettings);
        _defenderSpawnController.OnUnitSpawned += OnCreatedUnit;
        _onUpdates.Add(_defenderSpawnController);

        _targetFinder = new TargetFinder(_mainTower);

        _enemyBattleController = new EnemyBattleController(_targetFinder);
        _onUpdates.Add(_enemyBattleController);
        _onFixedUpdates.Add(_enemyBattleController);

        _enemySpawnHandler = new EnemySpawnHandler(_enemySpawnController, _waveSettings);
        _enemySpawnHandler.OnWavesEnd += OnEnemySpawnEnd;

        _enemySpawnHandler.StartSpawn();

        _defenderSpawnController.SpawnUnit(UnitRoleType.Militiaman);
        _defenderSpawnController.SpawnUnit(UnitRoleType.Militiaman);
    }


    private void Update()
    {
        foreach (var ell in _onUpdates)
        {
            ell.OnUpdate(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        foreach (var ell in _onFixedUpdates)
        {
            ell.OnFixedUpdate(Time.fixedDeltaTime);
        }
    }

    private void OnCreatedUnit(IUnit unit) 
    {
        unit.Enable();
        _enemyBattleController.AddUnit(unit);
    }

    private void OnEnemySpawnEnd()
    {
        Debug.Log("enemy spawn ends!");
    }
}
