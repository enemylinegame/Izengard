using System.Collections.Generic;
using BattleSystem;
using BattleSystem.Buildings;
using BattleSystem.Buildings.Configs;
using BattleSystem.Buildings.View;
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

    [SerializeField] private BattleSystemData _battleSystemData;

    [Header("Enemy Spawn Parametrs")]
    [SerializeField] private SpawnerView _enemySpawner;
    [SerializeField] private WaveSettings _waveSettings;

    [Header("Defender Spawn Parametrs")]
    [SerializeField] private bool _enableDefenders = true;
    [SerializeField] private SpawnSettings _defenderSpawnSettings;
    [SerializeField] private List<Transform> _defenderSpawnPoints;
    [Space(10)]
    [SerializeField] private NavigationSurfaceView _groundSurface;


    private TimeRemainingController timeRemainingController;

    private NavigationUpdater _navigationUpdater;

    private UnitsContainer _unitsContainer;

    private WarBuildingsController _mainTowerController;

    private EnemySpawnController _enemySpawnController;
    private DefenderSpawnTestController _defenderSpawnController;
    private TargetFinder _targetFinder;
    
    private BaseBattleController _enemyBattleController;
    private DefenderBattleController _defenderBattleController;

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

        _unitsContainer = new UnitsContainer();

        _targetFinder = new TargetFinder(_mainTowerController, _unitsContainer);

        _enemySpawnController = new EnemySpawnController(_enemySpawner);
        _enemySpawnController.OnUnitSpawned += OnCreatedUnit;
        _onUpdates.Add(_enemySpawnController);
        
        _enemyBattleController 
            = new EnemyBattleController(_battleSystemData, _targetFinder, _unitsContainer);
        _onUpdates.Add(_enemyBattleController);
        
        _enemySpawnHandler = new EnemySpawnHandler(_enemySpawnController, _waveSettings);
        _enemySpawnHandler.OnWavesEnd += OnEnemySpawnEnd;

        if (_enableDefenders)
        {
            _defenderSpawnController = new DefenderSpawnTestController(_defenderSpawnPoints, _defenderSpawnSettings);
            _defenderSpawnController.OnUnitSpawned += OnCreatedUnit;
            _onUpdates.Add(_defenderSpawnController);
            
            _defenderBattleController
                = new DefenderBattleController(_battleSystemData, _targetFinder, _unitsContainer);
            _onUpdates.Add(_defenderBattleController);

            _defenderSpawnController.SpawnUnit(UnitType.Militiaman);
            _defenderSpawnController.SpawnUnit(UnitType.Militiaman);
            _defenderSpawnController.SpawnUnit(UnitType.Militiaman);
        }

        _mainTowerController.OnStart();

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
        _unitsContainer.AddUnit(unit);
    }

    private void OnEnemySpawnEnd()
    {
        Debug.Log("enemy spawn ends!");
    }
}
