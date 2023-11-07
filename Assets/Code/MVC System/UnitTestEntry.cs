using System.Collections.Generic;
using BattleSystem;
using BattleSystem.Buildings;
using BattleSystem.Buildings.Configs;
using BattleSystem.Buildings.View;
using Code.SceneConfigs;
using Configs;
using SpawnSystem;
using Tools;
using Tools.Navigation;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

public class UnitTestEntry : MonoBehaviour
{
    [SerializeField] private WarBuildingView _mainTower;
    [SerializeField] private WarBuildingConfig _mainTowerConfig;

    [SerializeField] private BattleSystemConstants _battleSystemConst;

    [Header("Enemy Spawn Parametrs")]
    [SerializeField] private SpawnerView _enemySpawner;
    [SerializeField] private WaveSettings _waveSettings;

    [Header("Defender Spawn Parametrs")]
    [SerializeField] private bool _enableDefenders = true;
    [SerializeField] private SpawnSettings _defenderSpawnSettings;
    [SerializeField] private List<Transform> _defenderSpawnPoints;
    [Space(10)]
    [SerializeField] private NavigationSurfaceView _groundSurface;

    [Header("Obstacles")]
    [SerializeField] private List<DefendWallObstacleView> _defendWalls;

    private TimeRemainingController timeRemainingController;

    private NavigationUpdater _navigationUpdater;

    private WarBuildingsController _mainTowerController;
    private ObstacleController _obstacleController;

    private EnemySpawnController _enemySpawnController;
    private DefenderSpawnTestController _defenderSpawnController;
    private TargetFinder _targetFinder;
    
    private EnemyTestBattleController _enemyBattleController;
    private UnitBattleController _defenderBattleController;

    private EnemySpawnHandler _enemySpawnHandler;

    private List<IOnUpdate> _onUpdates = new List<IOnUpdate>();
    private List<IOnFixedUpdate> _onFixedUpdates = new List<IOnFixedUpdate>();

    private int _surfaceId;

    private void Start()
    {
        timeRemainingController = new TimeRemainingController();
        _onUpdates.Add(timeRemainingController);

        _navigationUpdater = new NavigationUpdater();
        _surfaceId = _navigationUpdater.AddNavigationSurface(_groundSurface);

        _mainTowerController = new WarBuildingsController(_mainTower, _mainTowerConfig);
        _obstacleController = new ObstacleController(_surfaceId, _navigationUpdater, _defendWalls);

        _targetFinder = new TargetFinder(_mainTowerController);

        _enemySpawnController = new EnemySpawnController(_enemySpawner);
        _enemySpawnController.OnUnitSpawned += OnCreatedUnit;
        _onUpdates.Add(_enemySpawnController);
        
        _enemyBattleController = new EnemyTestBattleController(_targetFinder, _obstacleController);
        _onUpdates.Add(_enemyBattleController);
        _onFixedUpdates.Add(_enemyBattleController);
        
        _enemySpawnHandler = new EnemySpawnHandler(_enemySpawnController, _waveSettings);
        _enemySpawnHandler.OnWavesEnd += OnEnemySpawnEnd;

        if (_enableDefenders)
        {
            _defenderSpawnController = new DefenderSpawnTestController(_defenderSpawnPoints, _defenderSpawnSettings);
            _defenderSpawnController.OnUnitSpawned += OnCreatedUnit;
            _onUpdates.Add(_defenderSpawnController);
            
            var unitsContainer = new UnitsContainer();

            _defenderBattleController = new UnitBattleController(_battleSystemConst, unitsContainer, _targetFinder);
            _onUpdates.Add(_defenderBattleController);

            _defenderSpawnController.SpawnUnit(UnitType.Militiaman);
            _defenderSpawnController.SpawnUnit(UnitType.Militiaman);
        }

        _mainTowerController.OnStart();
        _obstacleController.OnStart();

        _enemySpawnHandler.StartSpawn();
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

        if(_enableDefenders) 
        {
            _defenderBattleController.AddUnit(unit);
        }
    }

    private void OnEnemySpawnEnd()
    {
        Debug.Log("enemy spawn ends!");
    }
}
