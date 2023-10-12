using Abstraction;
using System.Collections.Generic;
using UnitSystem;
using UnitSystem.Enum;
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
                            var target = GetTarget(unit);
                            if(target is NoneTarget)
                            {
                                unit.Target.SetTarget(targetFinder.GetMainTower());
                                MoveUnitToTarget(unit, unit.Target.CurrentTarget);
                                ChangeUnitState(unit, UnitState.Idle);
                            }
                            else
                            {
                                unit.Target.SetTarget(target);
                                MoveUnitToTarget(unit, target);
                                ChangeUnitState(unit, UnitState.Move);
                            }
                            break;
                        }
                    case UnitState.Move:
                        {
                            var turgentPos = unit.Target.CurrentTarget.Position;
                            var distance = GetDistanceToTarget(unit.GetPosition(), turgentPos);
                            if (CheckStopDistance(distance, unit.Stats.DetectionRange.GetValue()) == true)
                            {
                                Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Role} - startApproach");
                                ChangeUnitState(unit, UnitState.Approach);
                                MoveUnitToTarget(unit, unit.Target.CurrentTarget);
                            }
                            break;
                        }
                    case UnitState.Approach:
                        {
                            var turgentPos = unit.Target.CurrentTarget.Position;
                            var distance = GetDistanceToTarget(unit.GetPosition(), turgentPos);
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

                            var target = unit.Target.CurrentTarget;
                            var targetUnit = _defenderUnitCollection.Find(u => u.Id == target.Id);
                          
                            if(targetUnit != null)
                            {
                                var enemyDamage = unit.Offence.GetDamage();

                                targetUnit.TakeDamage(enemyDamage);
                            }
                    
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
                        InitUnitLogic(unit);
                        _enemyUnitCollection.Add(unit);
                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        unit.OnReachedZeroHealth += DefenderReachedZeroHp;
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

        private void DefenderReachedZeroHp(IUnit unit)
        {
            unit.OnReachedZeroHealth -= DefenderReachedZeroHp;
            unit.Disable();
            _defenderUnitCollection.Remove(unit);
        }

        private void ChangeUnitState(IUnit unit, UnitState state)
        {
            unit.UnitState.ChangeState(state);
        }

        #region Enemy moving logic

        private void MoveUnitToTarget(IUnit unit, ITarget target)
        {
            unit.Navigation.MoveTo(target.Position);
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

        private ITarget GetTarget(IUnit unit)
        {
            var nextUnitPriority = unit.Priority.GetNext();
           
            switch (nextUnitPriority.priorityType)
            {
                default:
                case UnitPriorityType.MainTower:
                    {
                        return targetFinder.GetMainTower();
                    }
                case UnitPriorityType.ClosestFoe:
                    {
                        var target = GetClosestDefender(unit);
                        if (target is NoneTarget)
                        {
                            return GetTarget(unit);
                        }
                        return target;
                    }
                case UnitPriorityType.SpecificFoe:
                    {
                        var target = GetClosestDefender(unit, nextUnitPriority.roleType);
                        if (target is NoneTarget)
                        {
                            return GetTarget(unit);
                        }
                        return target;
                    }
            }
        }

        private ITarget GetClosestDefender(IUnit unit, UnitRoleType targetRole = UnitRoleType.None)
        {
            ITarget target = new NoneTarget();

            var unitPos = unit.GetPosition();
            var minDist = float.MaxValue;

            for (int i = 0; i < _defenderUnitCollection.Count; i++)
            {
                var defender = _defenderUnitCollection[i];

                if(targetRole != UnitRoleType.None && defender.Stats.Role != targetRole)
                    continue;

                var defenderPos = defender.GetPosition();

                var distance = Vector3.Distance(unitPos, defenderPos);
                if (distance < minDist)
                {
                    minDist = distance;
                    target = defender.View;
                }
            }

            return target;
        }

        #endregion
    }
}
