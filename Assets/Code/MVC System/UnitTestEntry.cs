using System.Collections.Generic;
using EnemySystem;
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

    private List<IOnUpdate> _onUpdates = new List<IOnUpdate>();
    private List<IOnFixedUpdate> _onFixedUpdates = new List<IOnFixedUpdate>();

    private void Start()
    {
        _navigationUpdater = new NavigationUpdater();
        _navigationUpdater.AddNavigationSurface(_groundSurface);

        _enemySpawnController = new EnemySpawnController(_enemySpawnPoints, _enemySpawnSettings);
        _enemySpawnController.OnUnitSpawned += OnUnitCreated;

        _enemySpawnController.SpawnUnit(UnitRoleType.Militiaman);
        _enemySpawnController.SpawnUnit(UnitRoleType.Militiaman);
        _enemySpawnController.SpawnUnit(UnitRoleType.Hunter);
    }

    private void OnUnitCreated(IUnit unit)
    {
        var enemyController = new EnemyController(unit);
        enemyController.Enable();

        _onUpdates.Add(enemyController);
        _onFixedUpdates.Add(enemyController);
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
