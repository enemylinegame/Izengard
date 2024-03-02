using Abstraction;
using BattleSystem.MainTower;
using Configs;
using Tools;
using UnitSystem;
using UnitSystem.Enum;


namespace BattleSystem
{
    public class DefenderBattleController : BaseBattleController
    {
        public DefenderBattleController(
            BattleSystemData data,
            TargetFinder targetFinder,
            IUnitsContainer unitsContainer,
            MainTowerController mainTower) : base(data, targetFinder, unitsContainer, mainTower)
        {
        }

        public override void OnPause()
        {
            base.OnPause();

            for (int i = 0; i < unitsContainer.DefenderUnits.Count; i++)
            {
                var unit = unitsContainer.DefenderUnits[i];

                unit.Stop();
            }
        }

        protected override void MainTowerDestroyed()
        {
            for (int i = 0; i < unitsContainer.DefenderUnits.Count; i++)
            {
                var unit = unitsContainer.DefenderUnits[i];

                unit.Stop();
                unit.ChangeState(UnitStateType.None);
            }
        }

        protected override void ExecuteOnUpdate(float deltaTime)
        {
            for (int i = 0; i < unitsContainer.DefenderUnits.Count; i++)
            {
                var unit = unitsContainer.DefenderUnits[i];

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
                = unitsContainer.DefenderUnits.FindAll(e => e.Target.CurrentTarget.Id == target.Id);

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
