using Abstraction;
using BattleSystem.MainTower;
using Configs;
using SpawnSystem;
using Tools;
using UnitSystem;
using UnitSystem.Enum;

namespace BattleSystem
{
    public class EnemyBattleController : BaseBattleController
    {
        private readonly ISpawnController _enemySpawner;

        public EnemyBattleController(
            BattleSystemData data,
            TargetFinder targetFinder,
            IUnitsContainer unitsContainer,
            MainTowerController mainTower,
            ISpawnController enemySpawner) : base(data, targetFinder, unitsContainer, mainTower)
        {
            _enemySpawner = enemySpawner;

            this.unitsContainer.OnDefenderAdded += ResetUnitState;
        }

        public override void OnPause()
        {
            base.OnPause();

            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

                unit.Stop();
            }
        }

        private void ResetUnitState()
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

                unit.ChangeState(UnitStateType.Idle);
            }
        }

        protected override void MainTowerDestroyed()
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

                unit.Stop();
                unit.ChangeState(UnitStateType.None);
            }
        }

        protected override void ExecuteOnUpdate(float deltaTime)
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

                switch (unit.State.Current)
                {
                    default:
                        break;

                    case UnitStateType.Idle:
                        {
                            UnitIdleState(unit, deltaTime);
                            break;
                        }
                    case UnitStateType.Move:
                        {
                            UnitMoveState(unit, deltaTime);
                            break;
                        }
                    case UnitStateType.Attack:
                        {
                            UnitAttackState(unit, deltaTime);
                            break;
                        }
                    case UnitStateType.Die:
                        {
                            UnitDeadState(unit, deltaTime);
                            break;
                        }
                }

            }
        }

        protected override void UpdateTargetExistance(ITarget target)
        {
            var linkedUnits
                = unitsContainer.EnemyUnits.FindAll(e => e.Target.CurrentTarget.Id == target.Id);

            if (linkedUnits == null)
                return;

            for (int i = 0; i < linkedUnits.Count; i++)
            {
                var unit = linkedUnits[i];

                unit.Target.ResetTarget();

                unit.ChangeState(UnitStateType.Idle);
            }
        }
    }
}
