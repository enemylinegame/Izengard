using System.Collections.Generic;
using EnemySystem;
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

    private IUnitController _enemyHunterController;
    private IUnitController _enemyMilitiamanController;

    private List<IOnUpdate> _onUpdates = new List<IOnUpdate>();
    private List<IOnFixedUpdate> _onFixedUpdates = new List<IOnFixedUpdate>();

    private void Start()
    {
        _navigationUpdater = new NavigationUpdater();
        _navigationUpdater.AddNavigationSurface(_groundSurface);

        _enemyHunterController = new EnemyHunterController();
        _enemyHunterController.Enable();
        _onUpdates.Add(_enemyHunterController);
        _onFixedUpdates.Add(_enemyHunterController);

        _enemyMilitiamanController = new EnemyMilitiamanController();
        _enemyMilitiamanController.Enable();
        _onUpdates.Add(_enemyMilitiamanController);
        _onFixedUpdates.Add(_enemyMilitiamanController);

        _enemySpawnController = new EnemySpawnController(_enemySpawnPoints, _enemySpawnSettings);
        _onUpdates.Add(_enemySpawnController);

        _enemySpawnController.OnUnitSpawned += _enemyHunterController.AddUnit;
        _enemySpawnController.OnUnitSpawned += _enemyMilitiamanController.AddUnit;

        _enemySpawnController.SpawnUnit(UnitRoleType.Militiaman);
        _enemySpawnController.SpawnUnit(UnitRoleType.Militiaman);
        _enemySpawnController.SpawnUnit(UnitRoleType.Hunter);
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
}
