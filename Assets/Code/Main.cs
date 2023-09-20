using Izengard.EnemySystem;
using Izengard.Tools.Navigation;
using Izengard.UnitSystem;
using Izengard.UnitSystem.Data;
using Izengard.UnitSystem.View;
using UnityEngine;

namespace Izengard
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private UnitSettings _unitSettings;
        [SerializeField] private BaseUnitView _unitView;
        [SerializeField] private Transform _mainTower;
        [SerializeField] private NavigationSurfaceView _groundSurface;

        private UnitFactory _unitFactory;

        private NavigationUpdater _navigationUpdater;
        private EnemyController _enemyController;

        private void Start()
        {
            _navigationUpdater = new NavigationUpdater();
            _navigationUpdater.AddNavigationSurface(_groundSurface);

            var unitDefence = new UnitDefenceModel(_unitSettings.DefenceData);
            var unitOffence = new UnitOffenceModel(_unitSettings.OffenceData);
            
            var unitModel = new UnitModel(
                _unitSettings.Faction,
                _unitSettings.StatsData,
                unitDefence,
                unitOffence);

            var unitHandler = new UnitHandler(0, _unitView, unitModel);

            _enemyController = new EnemyController(unitHandler, _mainTower.position);
            _enemyController.Enable();
        }

        private void Update()
        {
            _enemyController.OnUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _enemyController.OnFixedUpdate(Time.fixedDeltaTime);
        }
    }
}
