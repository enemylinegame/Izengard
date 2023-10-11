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
                    default:
                        break;

                    case UnitState.Idle:
                        {
                            var distance = GetDistanceToTarget(unit.GetPosition(), unit.SpawnPosition);
                            if (CheckStopDistance(distance, 0) == true)
                            {
                                StopUnit(unit);
                            }
                            else
                            {
                                ChangeUnitState(unit, UnitState.Search);
                            }
                            break;
                        }
                    case UnitState.Search:
                        {
                            var target = GetTargetPosition(unit);
                            if(target == unit.SpawnPosition)
                            {
                                MoveUnitToTarget(unit, unit.SpawnPosition);
                                ChangeUnitState(unit, UnitState.Idle);
                            }
                            else
                            {
                                unit.Target.SetPositionedTarget(target);
                                MoveUnitToTarget(unit, target);
                                ChangeUnitState(unit, UnitState.Move);
                            }
                            break;
                        }
                    case UnitState.Move:
                        {
                            var distance = GetDistanceToTarget(unit.GetPosition(), unit.Target.PositionedTarget);
                            if (CheckStopDistance(distance, unit.Stats.DetectionRange.GetValue()) == true)
                            {
                                Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Role} - startApproach");
                                ChangeUnitState(unit, UnitState.Approach);
                                MoveUnitToTarget(unit, unit.Target.PositionedTarget);
                            }
                            break;
                        }
                    case UnitState.Approach:
                        {
                            var distance = GetDistanceToTarget(unit.GetPosition(), unit.Target.PositionedTarget);
                            if (CheckStopDistance(distance, unit.Offence.MaxRange) == true)
                            {
                                StopUnit(unit);
                                ChangeUnitState(unit, UnitState.Attack);
                            }
                            break;
                        }
                    case UnitState.Attack:
                        {
                            Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Role} - startAttack");
                            ChangeUnitState(unit, UnitState.None);
                            break;
                        }
                    case UnitState.Die: 
                        {
                            break;
                        }
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
      
        private void UnitReachedZerohealth(IUnit unit)
        {
            Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Role} - dead");
        }

        private void ChangeUnitState(IUnit unit, UnitState state)
        {
            unit.UnitState.ChangeState(state);
        }

        #region Enemy moving logic

        private void MoveUnitToTarget(IUnit unit, Vector3 targetPos)
        {
            unit.Navigation.MoveTo(targetPos);
        }

        private void StopUnit(IUnit unit)
        {
            unit.Navigation.Stop();
        }

        private float GetDistanceToTarget(Vector3 unitPos, Vector3 targetPos) => 
            Vector3.Distance(unitPos, targetPos);

        private bool CheckStopDistance(float curDistance, float stopDistance)
        {
            if (curDistance <= stopDistance)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Enemy finding logic

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

        #endregion
    }
}
