using BattleSystem;
using System;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

namespace EnemySystem.Controllers
{
    public class EnemyHunterController : EnemyBaseController
    {
        private Vector3 _currentTarget;

        public EnemyHunterController(TargetFinder targetFinder) : base(targetFinder)
        {
        }
        
        public override event Action<IUnit> OnUnitDone;

        protected override void InitUnitLogic(IUnit unit)
        {
            unit.Navigation.Enable();

            unit.UnitState.ChangeState(UnitState.Idle);

            _currentTarget
                = targetFinder.GetMainTowerPosition();

            MoveUnitToTarget(unit, _currentTarget);
        }

        protected override void DeinitUnitLogic(IUnit unit)
        {
            unit.Navigation.Disable();
        }

        protected override void OnUnitUpdate(float deltaTime)
        {
            if (unitCollection.Count != 0)
            {
                for (int i = 0; i < unitCollection.Count; i++)
                {
                    var unit = unitCollection[i];
                }
            }
        }

        protected override void OnUnitFixedUpdate(float fixedDeltaTime)
        {
            if (unitCollection.Count != 0)
            {
                for (int i = 0; i < unitCollection.Count; i++)
                {
                    var unit = unitCollection[i];

                    if (unit.UnitState.CurrentState == UnitState.Move)
                    {
                        if (CheckStopDistance(unit, _currentTarget) == true)
                        {
                            StopUnit(unit);
                            OnUnitDone?.Invoke(unit);
                        }
                    }
                }
            }
        }

        private void MoveUnitToTarget(IUnit unit, Vector3 target)
        {
            unit.Navigation.MoveTo(target);
            unit.UnitState.ChangeState(UnitState.Move);
        }

        private void StopUnit(IUnit unit)
        {
            unit.Navigation.Stop();
            unit.UnitState.ChangeState(UnitState.Idle);
        }
    }
}
