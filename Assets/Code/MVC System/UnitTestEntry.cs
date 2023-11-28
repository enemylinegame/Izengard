using System.Collections.Generic;
using BattleSystem;
using BattleSystem.Buildings;
using Code.SceneConfigs;
using Configs;
using SpawnSystem;
using Tools;
using Tools.Navigation;
using UnitSystem;
using UnityEngine;

public class UnitTestEntry : MonoBehaviour
{
    [SerializeField] private SceneObjectsHolder _sceneObjects;
    [SerializeField] private ConfigsHolder _configsHolder;

    [SerializeField] private bool _enableDefenders = true;


    private TimeRemainingController timeRemainingController;

    private NavigationUpdater _navigationUpdater;

    private UnitsContainer _unitsContainer;

    private WarBuildingsController _mainTowerController;

    private ISpawnController _enemySpawnController;
    private ISpawnController _defenderSpawnController;
    private TargetFinder _targetFinder;
    
    private BaseBattleController _enemyBattleController;
    private DefenderBattleController _defenderBattleController;

    private EnemySpawnHandler _enemySpawnHandler;
    private DefenderSpawnHandler _defenderSpawnHandler;

    private List<IOnUpdate> _onUpdates = new List<IOnUpdate>();
    private List<IOnFixedUpdate> _onFixedUpdates = new List<IOnFixedUpdate>();

    private int _surfaceId;

    private void Start()
    {
        timeRemainingController = new TimeRemainingController();
        _onUpdates.Add(timeRemainingController);

        _navigationUpdater = new NavigationUpdater();
        _surfaceId = _navigationUpdater.AddNavigationSurface(_sceneObjects.GroundSurface);
       
        _mainTowerController = new WarBuildingsController(_sceneObjects.MainTower, _configsHolder.MainTowerSettings);

        _unitsContainer = new UnitsContainer();

        _targetFinder = new TargetFinder(_mainTowerController, _unitsContainer);

        _enemySpawnController = new EnemySpawnController(_sceneObjects.EnemySpawner);
        _enemySpawnController.OnUnitSpawned += OnCreatedUnit;
        _onUpdates.Add(_enemySpawnController);
        
        _enemyBattleController 
            = new EnemyBattleController(_configsHolder.BattleSystemConst, _targetFinder, _unitsContainer, _enemySpawnController);
        _onUpdates.Add(_enemyBattleController);
        
        _enemySpawnHandler = new EnemySpawnHandler(_enemySpawnController, _configsHolder.EnemyWaveSettings);
        _enemySpawnHandler.OnWavesEnd += OnEnemySpawnEnd;

        if (_enableDefenders)
        {
            _defenderSpawnController = new DefendersSpawnController(_sceneObjects.DefendersSpawner);
            _defenderSpawnController.OnUnitSpawned += OnCreatedUnit;
            _onUpdates.Add(_defenderSpawnController);
            
            _defenderBattleController
                = new DefenderBattleController(_configsHolder.BattleSystemConst, _targetFinder, _unitsContainer);
            _onUpdates.Add(_defenderBattleController);

            _defenderSpawnHandler = new DefenderSpawnHandler(_defenderSpawnController);
        }

        _mainTowerController.OnStart();

        _enemySpawnHandler.StartSpawn();
        _defenderSpawnHandler.StartSpawn();
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
