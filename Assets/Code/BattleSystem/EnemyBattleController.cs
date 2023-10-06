using System.Collections.Generic;
using UnitSystem;
using UnitSystem.Enum;
using UnitSystem.View;
using UnityEngine;

namespace BattleSystem
{
    public class EnemyBattleController : BaseBattleController
    {
        private List<IUnit> _enemyUnitCollection;
        private List<IUnit> _defenderUnitCollection;

        public EnemyBattleController(TargetFinder targetFinder) : base(targetFinder)
        {
            _enemyUnitCollection = new List<IUnit>();
            _defenderUnitCollection = new List<IUnit>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var unit in _enemyUnitCollection)
            {
                switch (unit.UnitState.CurrentState)
                {
                    case UnitState.Idle:
                        {
                            EnemyInIdleLogic(unit);
                            break;
                        }
                    case UnitState.Move:
                        {
                            EnemyInMoveLogic(unit);
                            break;
                        }
                }
            }
        }

        private void EnemyInIdleLogic(IUnit unit)
        {
            switch (unit.Stats.Role)
            {
                default:
                    break;
                case UnitRoleType.Imp:
                    {
                        var target = GetClosestDefender(unit);
                        if (target is StubUnitView)
                        {
                            unit.Target.SetPositionedTarget(targetFinder.GetMainTowerPosition());
                            MoveUnitToTarget(unit, unit.Target.PositionedTarget);
                        }
                        else
                        {
                            unit.Target.SetUnitTarget(target);
                            MoveUnitToTarget(unit, unit.Target.UnitTarget);
                        }
                        break;
                    }
                case UnitRoleType.Hound:
                    {
                        var target = targetFinder.GetMainTowerPosition();
                        unit.Target.SetPositionedTarget(target);

                        MoveUnitToTarget(unit, unit.Target.PositionedTarget);
                        break;
                    }
            }

            unit.UnitState.ChangeState(UnitState.Move);
        }

        private void EnemyInMoveLogic(IUnit unit)
        {
            switch (unit.Stats.Role)
            {
                default:
                    break;
                case UnitRoleType.Imp:
                    {
                        Vector3 targetPos = Vector3.zero;

                        if(unit.Target.UnitTarget == null)
                        {
                            targetPos = unit.SpawnPosition;
                        }
                        else
                        {
                            targetPos = unit.Target.UnitTarget.SelfTransform.position;
                        }

                        if (CheckStopDistance(unit, targetPos) == true)
                        {
                            StopUnit(unit);
                        }
                        break;
                    }
                case UnitRoleType.Hound:
                    {
                        if (CheckStopDistance(unit, unit.Target.PositionedTarget) == true)
                        {
                            StopUnit(unit);
                        }
                        break;
                    }
            }
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            foreach (var unit in _enemyUnitCollection)
            {

            }
        }

        public override void AddUnit(IUnit unit)
        {
            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        _enemyUnitCollection.Add(unit);
                        InitUnitLogic(unit);
                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        _defenderUnitCollection.Add(unit);
                        break;
                    }
            }
        }

        private void InitUnitLogic(IUnit unit)
        {
            unit.Navigation.Enable();
            unit.OnReachedZeroHealth += UnitReachedZerohealth;

            unit.UnitState.ChangeState(UnitState.Idle);
        }

        private BaseUnitView GetClosestDefender(IUnit unit)
        {
            return targetFinder.GetClosestUnit(unit);
        }

        private void MoveUnitToTarget(IUnit unit, Vector3 target)
        {
            unit.Navigation.MoveTo(target);
        }

        private void MoveUnitToTarget(IUnit unit, IUnitView target)
        {
            unit.Navigation.MoveTo(target.SelfTransform.position);
        }

        private void StopUnit(IUnit unit)
        {
            unit.Navigation.Stop();
        }

        private bool CheckStopDistance(IUnit unit, Vector3 currentTarget)
        {
            var distance = Vector3.Distance(unit.GetPosition(), currentTarget);
            if (distance <= unit.Offence.MaxRange)
            {
                return true;
            }
            return false;
        }

        private void UnitReachedZerohealth(IUnit unit)
        {
            Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Role} - dead");
        }
    }
}
