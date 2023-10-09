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
                        var target = GetTargetPosition(unit);
                        unit.Target.SetPositionedTarget(target);
                        MoveUnitToTarget(unit, target);
                        break;
                    }
                case UnitRoleType.Hound:
                    {
                        var target = GetTargetPosition(unit);
                        unit.Target.SetPositionedTarget(target);
                        MoveUnitToTarget(unit, target);
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
                        if (CheckStopDistance(unit, unit.Target.PositionedTarget) == true)
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

        private void MoveUnitToTarget(IUnit unit, Vector3 targetPos)
        {
            unit.Navigation.MoveTo(targetPos);
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

        private Vector3 GetTargetPosition(IUnit unit)
        {
            var result = Vector3.zero;

            var nextUnitPriority = unit.Priority.GetNext();
           
            switch (nextUnitPriority.priorityType)
            {
                default:
                    {
                        result = unit.SpawnPosition;
                        break;
                    }
                case UnitPriorityType.MainTower:
                    {
                        result = targetFinder.GetMainTowerPosition();
                        break;
                    }
                case UnitPriorityType.ClosestFoe:
                    {
                        result = GetClosestDefenderPosition(unit);
                        break;
                    }
                case UnitPriorityType.SpecificFoe:
                    {
                        result = GetClosestDefenderPosition(unit, nextUnitPriority.roleType);
                        break;
                    }
            }

            return result;
        }

        private Vector3 GetClosestDefenderPosition(IUnit unit)
        {
            Vector3 resultPos = unit.SpawnPosition;

            var unitPos = unit.GetPosition();
            var minDist = float.MaxValue;
            
            foreach (var defender in _defenderUnitCollection)
            {
                var defenderPos = defender.GetPosition();
                var distance = Vector3.Distance(unitPos, defenderPos);
                if(distance < minDist)
                {
                    minDist = distance;
                    resultPos = defenderPos;
                }
            }

            return resultPos;
        }


        private Vector3 GetClosestDefenderPosition(IUnit unit, UnitRoleType targetRole)
        {
            Vector3 resultPos = unit.SpawnPosition;

            var unitPos = unit.GetPosition();
            var minDist = float.MaxValue;

            foreach (var defender in _defenderUnitCollection)
            {
                if (defender.Stats.Role != targetRole)
                    continue;

                var defenderPos = defender.GetPosition();
                var distance = Vector3.Distance(unitPos, defenderPos);
                if (distance < minDist)
                {
                    minDist = distance;
                    resultPos = defenderPos;
                }
            }

            return resultPos;
        }
    }
}
