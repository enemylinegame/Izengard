using System;
using Abstraction;
using EnemySystem;
using System.Collections.Generic;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

namespace BattleSystem
{
    public class FifthBattleController : BaseBattleController
    {
        
        private enum AttackPhase
        {
            None = 0,
            Cast,
            Attack
        }
        
        private class AttackModel
        {
            public IUnit Attacker;
            public float TimingProgress;
            public AttackPhase Phase;
        }

        private const float DESTINATION_POSITION_ERROR_SQR = 0.1f * 0.1f;
        
        private List<IUnit> _enemyUnits;
        private List<IUnit> _defenderUnits;
        private List<AttackModel> _attackModels;


        public FifthBattleController(TargetFinder targetFinder) : base(targetFinder)
        {
            _enemyUnits = new ();
            _defenderUnits = new ();
            _attackModels = new();
        }

        public override void OnUpdate(float deltaTime)
        {
            
            for(int i=0; i < _enemyUnits.Count; i++)
            {
                IUnit unit = _enemyUnits[i];

                ExecuteUnitUpdate(unit, deltaTime);
            }
            
            for(int i=0; i < _defenderUnits.Count; i++)
            {
                IUnit unit = _defenderUnits[i];

                ExecuteUnitUpdate(unit, deltaTime);
            }
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            // for (int i = 0; i < _enemyUnitCollection.Count; i++)
            // {
            //     IUnit unit = _enemyUnitCollection[i];
            // }
        }

        private void ExecuteUnitUpdate(IUnit unit, float deltaTime)
        {
            UpdateTargetExistence(unit);

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

        public override void AddUnit(IUnit unit)
        {
            if (unit == null) return;
            if (_defenderUnits.Contains(unit)) return;
            if (_enemyUnits.Contains(unit)) return;
            
            Debug.Log("FifthBattleController->AddUnit: " + unit.View.SelfTransform.gameObject.name);
            
            unit.OnReachedZeroHealth += UnitReachedZeroHealth;

            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        unit.Navigation.Enable();
                        unit.UnitState.ChangeState(UnitState.Idle);

                        _enemyUnits.Add(unit);
                        break;
                    }
                case UnitFactionType.Defender:
                    {    
                        unit.Navigation.Enable();
                        unit.UnitState.ChangeState(UnitState.Idle);
                        
                        _defenderUnits.Add(unit);
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
                        Debug.Log($"FifthBattleController->RemoveUnit: [{unit.Id}]_{unit.Stats.Role} - dead");
                        
                        unit.Target.SetTarget(new NoneTarget());
                        unit.Disable();
                        _enemyUnits.Remove(unit);

                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        Debug.Log($"FifthBattleController->RemoveUnit: {unit.View.SelfTransform.gameObject.name}");
                        
                        unit.Target.SetTarget(new NoneTarget());
                        unit.Disable();
                        _defenderUnits.Remove(unit);
                        break;
                    }
            }
        }

        private void UpdateTargetExistence(IUnit unit)
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
                    List<IUnit> list = (unit.Stats.Faction == UnitFactionType.Enemy) ? _defenderUnits : _enemyUnits;

                    ITarget target = unit.Target.CurrentTarget;
                    if (!list.Exists(def => def.Id == target.Id)) 
                    {
                        unit.Target.SetTarget(new NoneTarget());

                        ChangeUnitState(unit, UnitState.Idle);
                    }
                    break;
                }
            }
        }

        private void UnitIdleState(IUnit unit, float deltaTime)
        {
            //Debug.Log("FifthBattleController->UnitIdleState:");
            ITarget target = GetTarget(unit);

            // if (target.Id < 0)
            // {
            //     Debug.Log("FifthBattleController->UnitIdleState: target.Id < 0 ");
            // }
            // if (target is NoneTarget)
            // {
            //     Debug.Log("FifthBattleController->UnitIdleState: target is NoneTarget ");
            // }

            if (target.Id > 0)
            {
                unit.Target.SetTarget(target);
                ChangeUnitState(unit, UnitState.Approach);
                MoveUnitToTarget(unit, target);
            }
            else 
            {
                if (!CheckIsOnDestinationPosition(unit))
                {
                    ChangeUnitState(unit, UnitState.Move);
                    unit.Navigation.MoveTo(unit.StartPosition);
                }
            }

        }

        private void UnitMoveState(IUnit unit, float deltaTime)
        {
            ITarget target = GetTarget(unit);
            if (target.Id > 0)
            {
                unit.Target.SetTarget(target);
                ChangeUnitState(unit, UnitState.Approach);
                MoveUnitToTarget(unit, target);
            }
            else 
            {
                if (CheckIsOnDestinationPosition(unit))
                {
                    ChangeUnitState(unit, UnitState.Idle);
                    unit.Navigation.Stop();
                }
            }
        }
        
        private void UnitApproachState(IUnit unit, float deltaTime)
        {
            Vector3 targetPos = unit.Target.CurrentTarget.Position;
            float distance = GetDistanceToTarget(unit.GetPosition(), targetPos);
            if (distance <= unit.Offence.MaxRange)
            {
                StopUnit(unit);

                ChangeUnitState(unit, UnitState.Attack);
            }
        }

        private void UnitAttackState(IUnit unit, float deltaTime)
        {
            AttackModel attack = _attackModels.Find(model => model.Attacker == unit);
            if (attack == null)
            {
                attack = new AttackModel()
                {
                    Attacker = unit,
                    TimingProgress = deltaTime,
                    Phase = AttackPhase.Cast
                };
                _attackModels.Add(attack);
            }
            else
            {
                switch (attack.Phase)
                {
                    case AttackPhase.None:
                        attack.TimingProgress = deltaTime;
                        attack.Phase = AttackPhase.Cast;
                        break;
                    case AttackPhase.Cast:
                        attack.TimingProgress += deltaTime;
                        if (attack.TimingProgress >= unit.Offence.CastingTime)
                        {
                            IUnit unitTarget = FindUnitByITarget(unit.Target.CurrentTarget);
                            if (unitTarget != null)
                            {
                                unitTarget.TakeDamage(unit.Offence.GetDamage());
                                attack.Phase = AttackPhase.None;
                                attack.TimingProgress = 0.0f;
                            }
                            else
                            {
                                unit.Target.SetTarget(new NoneTarget());
                                ChangeUnitState(unit, UnitState.Idle);
                            }
                        }
                        break;
                    case AttackPhase.Attack:
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                        break;
                }
            }
        }

        private IUnit FindUnitByITarget(ITarget target)
        {
            IUnit targetUnit = null;

            targetUnit = _defenderUnits.Find(u => u.Id == target.Id);
            if (targetUnit == null)
            {
                targetUnit = _enemyUnits.Find(u => u.Id == target.Id);
            }

            return targetUnit;
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


        private bool CheckIsOnDestinationPosition(IUnit unit)
        {
            Vector3 destination = unit.StartPosition;
            Vector3 position = unit.GetPosition();

            return (position - destination).sqrMagnitude <= DESTINATION_POSITION_ERROR_SQR;
        }
        
        // private bool CheckStopDistance(float curDistance, float stopDistance)
        // {
        //     if (curDistance <= stopDistance)
        //     {
        //         return true;
        //     }
        //     return false;
        // }

        #endregion

        #region Enemy finding logic

        private ITarget GetTarget(IUnit unit, int recursionCounter = -1 )
        {
            //Debug.Log("FifthBattleController->GetTarget:");
            
            if (recursionCounter == -1)
            {
                recursionCounter = 3;
            }
            else
            {
                recursionCounter--;
                if (recursionCounter == 0)
                {
                    return new NoneTarget();
                }
            }

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
                        ITarget target = GetClosestFoe(unit);
                        if (target is NoneTarget)
                        {
                            return GetTarget(unit, recursionCounter);
                        }
                        unit.Priority.ResetIndex();
                        return target;
                    }
                case UnitPriorityType.SpecificFoe:
                    {
                        ITarget target = GetClosestFoe(unit, nextUnitPriority.roleType);
                        if (target is NoneTarget)
                        {
                            return GetTarget(unit, recursionCounter);
                        }
                        unit.Priority.ResetIndex();
                        return target;
                    }
            }
        }

        private ITarget GetClosestFoe(IUnit unit, UnitRoleType targetRole = UnitRoleType.None)
        {
            ITarget target = new NoneTarget();
            
            List<IUnit> foeUnitList = (unit.Stats.Faction == UnitFactionType.Enemy) ? _defenderUnits : _enemyUnits;

            Vector3 unitPos = unit.GetPosition();
            float minDist = float.MaxValue;

            for (int i = 0; i < foeUnitList.Count; i++)
            {
                IUnit foeUnit = foeUnitList[i];

                if(targetRole != UnitRoleType.None && foeUnit.Stats.Role != targetRole)
                    continue;

                Vector3 defenderPos = foeUnit.GetPosition();

                float distance = Vector3.Distance(unitPos, defenderPos);
                if (distance < minDist)
                {
                    minDist = distance;
                    target = foeUnit.View;
                }
            }

            return target;
        }

        #endregion
    }
}
