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
            for (int i=0; i < _enemyUnitCollection.Count; i++)
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
                            break;
                        }
                    case UnitState.Search:
                        {
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
            unit.OnReachedZeroHealth += UnitReachedZeroHealth;
            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        unit.OnAttackProcessEnd += ExecuteFight;

                        unit.Navigation.Enable();
                        unit.UnitState.ChangeState(UnitState.Idle);
                        
                        _enemyUnitCollection.Add(unit);
                        break;
                    }
                case UnitFactionType.Defender:
                    {    
                        _defenderUnitCollection.Add(unit);
                        break;
                    }
            }
        }
      
        private void UnitReachedZeroHealth(IUnit unit)
        {
            RemoveUnit(unit);
        }


        public void RemoveUnit(IUnit unit)
        {
            unit.OnReachedZeroHealth -= UnitReachedZeroHealth;

            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Type} - dead");

                        unit.OnAttackProcessEnd -= ExecuteFight;

                        unit.Disable();

                        unit.Target.ResetTarget();

                        _enemyUnitCollection.Remove(unit);

                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        var linkedEnemy = 
                            _enemyUnitCollection.FindAll(e => e.Target.CurrentTarget == unit.View);

                        foreach(var enemy in linkedEnemy)
                        {
                            enemy.Target.ResetTarget();
                        }

                        _defenderUnitCollection.Remove(unit);
                        break;
                    }
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
                        if(unit.Target.CurrentTarget is NoneTarget)
                        {
                            ChangeUnitState(unit, UnitState.Idle);
                            return;
                        }

                        if (unit.Target.IsTargetChangePosition())
                        {
                            MoveUnitToTarget(unit, unit.Target.CurrentTarget);
                            ChangeUnitState(unit, UnitState.Move);
                            return;
                        }

                        break;
                    }
            }
        }

        private void UnitIdleState(IUnit unit, float deltaTime)
        {
            var target = GetTarget(unit);
            unit.Target.SetTarget(target);

            MoveUnitToTarget(unit, unit.Target.CurrentTarget);
            ChangeUnitState(unit, UnitState.Move);
        }

        private void UnitMoveState(IUnit unit, float deltaTime)
        {
            Vector3 turgentPos = unit.Target.CurrentTarget.Position;
            float distance = GetDistanceToTarget(unit.GetPosition(), turgentPos);
            if (CheckStopDistance(distance, unit.Offence.MaxRange) == true)
            {
                StopUnit(unit);

                ChangeUnitState(unit, UnitState.Attack);
            }
        }

        private void UnitAttackState(IUnit unit, float deltaTime)
        {
            var target = unit.Target.CurrentTarget;
      
            if (target is not NoneTarget)
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
                            var targetUnit = _defenderUnitCollection.Find(u => u.Id == target.Id);
                            
                            float timeSinceLastAttack = Time.fixedTime - unit.Offence.LastAttackTime;
                            
                            if (timeSinceLastAttack >= unit.Offence.AttackTime)
                            {
                                unit.StartAttack(targetUnit);
                                unit.Offence.LastAttackTime = Time.fixedTime;
                            }

                            break;
                        }
                }
            }
        }

        private void ExecuteFight(IDamageDealer damageDealer, IDamageable damageableTarget)
        {
            var damage = damageDealer.GetAttackDamage();
            damageableTarget.TakeDamage(damage);
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
            Debug.Log("EnemyBattleController->GetTarget:");
            (UnitPriorityType priorityType, UnitRoleType roleType) nextUnitPriority = unit.Priority.GetNext();
           
            switch (nextUnitPriority.priorityType)
            {
                default:
                case UnitPriorityType.MainTower:
                    {
                        unit.Priority.ResetIndex();
                        return targetFinder.GetMainTower();
                    }
                case UnitPriorityType.ClosestFoe:
                    {
                        ITarget target = GetClosestDefender(unit);
                        if (target is NoneTarget)
                        {
                            return GetTarget(unit);
                        }
                        unit.Priority.ResetIndex();
                        return target;
                    }
                case UnitPriorityType.SpecificFoe:
                    {
                        ITarget target = GetClosestDefender(unit, nextUnitPriority.roleType);
                        if (target is NoneTarget)
                        {
                            return GetTarget(unit);
                        }
                        unit.Priority.ResetIndex();
                        return target;
                    }
            }
        }

        private ITarget GetClosestDefender(IUnit unit, UnitType targetRole = UnitType.None)
        {
            ITarget target = new NoneTarget();

            Vector3 unitPos = unit.GetPosition();
            float minDist = float.MaxValue;

            for (int i = 0; i < _defenderUnitCollection.Count; i++)
            {
                IUnit defender = _defenderUnitCollection[i];

                if(targetRole != UnitType.None && defender.Stats.Type != targetRole)
                    continue;

                Vector3 defenderPos = defender.GetPosition();

                float distance = Vector3.Distance(unitPos, defenderPos);
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
