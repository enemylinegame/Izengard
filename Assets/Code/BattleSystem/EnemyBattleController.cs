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
            for(int i=0; i < _enemyUnitCollection.Count; i++)
            {
                var unit = _enemyUnitCollection[i];
               
                UpdateTarget(unit);

                switch (unit.UnitState.CurrentState)
                {
                    default:
                        break;

                    case UnitState.Idle:
                        {
                            UnitIdleState(unit, deltaTime);
                            break;
                        }
                    case UnitState.Move:
                        {
                            UnitMoveState(unit, deltaTime);   
                            break;
                        }
                    case UnitState.Approach:
                        {
                            UnitApproachState(unit, deltaTime);
                            break;
                        }
                    case UnitState.Search:
                        {
                            UnitSearchState(unit, deltaTime);
                            break;
                        }
                    case UnitState.Attack:
                        {
                            UnitAttackState(unit, deltaTime);
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
            for (int i =0; i < _enemyUnitCollection.Count; i++)
            {
                var unit = _enemyUnitCollection[i];
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
                        unit.OnReachedZeroHealth += UnitReachedZeroHealth;
                        _defenderUnitCollection.Add(unit);
                        break;
                    }
            }
        }

        private void InitUnitLogic(IUnit unit)
        {
            unit.Navigation.Enable();
            unit.OnReachedZeroHealth += UnitReachedZeroHealth;

            unit.UnitState.ChangeState(UnitState.Idle);
        }
      
        private void UnitReachedZeroHealth(IUnit unit)
        {
            if(unit.Stats.Faction == UnitFactionType.Enemy)
            {
                unit.OnReachedZeroHealth -= UnitReachedZeroHealth;
                Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Role} - dead");

                return;
            }
            
            if(unit.Stats.Faction == UnitFactionType.Defender)
            {
                unit.OnReachedZeroHealth -= UnitReachedZeroHealth;
                unit.Disable();
                _defenderUnitCollection.Remove(unit);

                return;
            }
        }

        private void UpdateTarget(IUnit unit)
        {
            switch (unit.Priority.CurrentPriority)
            {
                case UnitPriorityType.MainTower:
                    {
                        break;
                    }
                case UnitPriorityType.ClosestFoe:
                case UnitPriorityType.FarthestFoe:
                case UnitPriorityType.SpecificFoe:
                    {
                        var target = unit.Target.CurrentTarget;
                        if (_defenderUnitCollection.Exists(def => def.Id == target.Id) == false) 
                        {
                            ChangeUnitState(unit, UnitState.Search);
                        }
                        break;
                    }
            }
        }

        private void UnitIdleState(IUnit unit, float deltaTime)
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
        }

        private void UnitMoveState(IUnit unit, float deltaTime)
        {
            var turgentPos = unit.Target.CurrentTarget.Position;
            var distance = GetDistanceToTarget(unit.GetPosition(), turgentPos);
            if (CheckStopDistance(distance, unit.Stats.DetectionRange.GetValue()) == true)
            {
                Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Role} - startApproach");
                ChangeUnitState(unit, UnitState.Approach);
                MoveUnitToTarget(unit, unit.Target.CurrentTarget);
            }
        }

        private void UnitSearchState(IUnit unit, float deltaTime)
        {
            var target = GetTarget(unit);
            if (target is NoneTarget)
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
        }

        private void UnitApproachState(IUnit unit, float deltaTime)
        {
            var turgentPos = unit.Target.CurrentTarget.Position;
            var distance = GetDistanceToTarget(unit.GetPosition(), turgentPos);
            if (CheckStopDistance(distance, unit.Offence.MaxRange) == true)
            {
                StopUnit(unit);
                ChangeUnitState(unit, UnitState.Attack);
            }
        }

        private void UnitAttackState(IUnit unit, float deltaTime)
        {
            Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Role} - startAttack");

            var target = unit.Target.CurrentTarget;
            var targetUnit = _defenderUnitCollection.Find(u => u.Id == target.Id);

            if (targetUnit != null)
            {
                var enemyDamage = unit.Offence.GetDamage();

                targetUnit.TakeDamage(enemyDamage);
            }

            ChangeUnitState(unit, UnitState.None);
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
