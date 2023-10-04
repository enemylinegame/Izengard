using System.Collections.Generic;
using BattleSystem;
using EnemySystem.Controllers;
using SpawnSystem;
using Tools.Navigation;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

public class UnitTestEntry : MonoBehaviour
{
    [SerializeField] private Transform _mainTower;
    [SerializeField] private SpawnSettings _enemySpawnSettings;
    [SerializeField] private List<Transform> _enemySpawnPoints;
    [SerializeField] private NavigationSurfaceView _groundSurface;

    private NavigationUpdater _navigationUpdater;
    private EnemySpawnController _enemySpawnController;
    private TargetFinder _targetFinder;


    private Dictionary<UnitRoleType, IUnitController> _enemyControllersCollection = new();
    private Dictionary<UnitRoleType, IUnitController> _defenderControllersCollection = new();

    private List<IOnUpdate> _onUpdates = new List<IOnUpdate>();
    private List<IOnFixedUpdate> _onFixedUpdates = new List<IOnFixedUpdate>();

    private void Start()
    {
        _navigationUpdater = new NavigationUpdater();
        _navigationUpdater.AddNavigationSurface(_groundSurface);
        
        _enemySpawnController = new EnemySpawnController(_enemySpawnPoints, _enemySpawnSettings);
        _enemySpawnController.OnUnitSpawned += OnCreatedUnit;
        _onUpdates.Add(_enemySpawnController);

        _targetFinder = new TargetFinder(_mainTower);

        _enemyControllersCollection[UnitRoleType.Militiaman] = new EnemyMilitiamanController(_targetFinder);
        _enemyControllersCollection[UnitRoleType.Hunter] = new EnemyHunterController(_targetFinder);

        InitUnitCollection(_enemyControllersCollection);
        InitUnitCollection(_defenderControllersCollection);

        _enemySpawnController.SpawnUnit(UnitRoleType.Militiaman);
        _enemySpawnController.SpawnUnit(UnitRoleType.Militiaman);
        _enemySpawnController.SpawnUnit(UnitRoleType.Hunter);
    }


    private void InitUnitCollection(Dictionary<UnitRoleType, IUnitController> unitCollection)
    {
        foreach (var entry in unitCollection)
        {
            var unitController = entry.Value;

            unitController.OnUnitDone += OnUnitControllerDone;

            _onUpdates.Add(unitController);
            _onFixedUpdates.Add(unitController);
        }
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
        switch (unit.UnitStats.Faction)
        {
            default:
                break;
            case UnitFactionType.Enemy:
                {
                    var unitRole = unit.UnitStats.Role;
                    unit.Enable();
                    _enemyControllersCollection[unitRole].AddUnit(unit);

                    break;
                }
            case UnitFactionType.Defender:
                {
                    break;
                }
        }
    }

    private void OnUnitControllerDone(IUnit unit)
    {
        switch (unit.UnitStats.Faction)
        {
            default:
                break;
            case UnitFactionType.Enemy:
                {
                    var unitRole = unit.UnitStats.Role;
                    var unitId = unit.Id;
                    _enemyControllersCollection[unitRole].RemoveUnit(unitId);
                    Debug.Log($"Enemy[{unitId}]_{unitRole} reached trget");
                    break;
                }
            case UnitFactionType.Defender:
                {
                    break;
                }
        }
    }
}
