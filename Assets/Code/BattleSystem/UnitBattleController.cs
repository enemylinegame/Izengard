using System;
using Abstraction;
using EnemySystem;
using System.Collections.Generic;
using BattleSystem.Models;
using Configs;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;


namespace BattleSystem
{
    public class UnitBattleController : IOnController, IOnUpdate
    {
        private readonly TargetFinder _targetFinder;
        
        private List<IUnit> _enemyUnits;
        private List<IUnit> _defenderUnits;
        private List<IUnit> _deadUnits;
        
        private float _destinationPositionErrorSqr;
        private float _deadUnitsDestroyDelay;
        
        
        public UnitBattleController(BattleSystemConstants battleSystemConstants, TargetFinder targetFinder)
        {
            _targetFinder = targetFinder;
            _enemyUnits = new ();
            _defenderUnits = new ();
            _deadUnits = new();
            _destinationPositionErrorSqr = 
                battleSystemConstants.DestinationPositionError * battleSystemConstants.DestinationPositionError;
            _deadUnitsDestroyDelay = battleSystemConstants.DeadUnitsDestroyDelay;
        }

        public void OnUpdate(float deltaTime)
        {
            
            for (int i = 0; i < _enemyUnits.Count; i++)
            {
                IUnit unit = _enemyUnits[i];

                ExecuteUnitUpdate(unit, deltaTime);
            }
            
            for (int i = 0; i < _defenderUnits.Count; i++)
            {
                IUnit unit = _defenderUnits[i];

                ExecuteUnitUpdate(unit, deltaTime);
            }
            
            for (int i = 0; i < _deadUnits.Count; i++)
            {
                IUnit unit = _deadUnits[i];

                UnitDeadState(unit, deltaTime);
            }
        }

        private void ExecuteUnitUpdate(IUnit unit, float deltaTime)
        {
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
                    UpdateTargetExistence(unit);
                    UnitApproachState(unit, deltaTime);
                    break;
                }
                case UnitState.Search:
                {
                    break;
                }
                case UnitState.Attack:
                {
                    UpdateTargetExistence(unit);
                    UnitAttackState(unit, deltaTime);
                    break;
                }
                case UnitState.Die:
                {
                    UnitDeadState(unit, deltaTime);
                    break;
                }
            }
        }

        public void AddUnit(IUnit unit)
        {
            if (unit == null) return;
            if (_defenderUnits.Contains(unit)) return;
            if (_enemyUnits.Contains(unit)) return;
            
            //Debug.Log("FifthBattleController->AddUnit: " + unit.View.SelfTransform.gameObject.name);
            
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
            //Debug.Log($"FifthBattleController->UnitReachedZeroHealth: {unit.View.SelfTransform.gameObject.name}");
            unit.OnReachedZeroHealth -= UnitReachedZeroHealth;
            ChangeUnitState(unit, UnitState.Die);
            
            if (unit.Stats.Faction == UnitFactionType.Enemy)
            {
                _enemyUnits.Remove(unit);
            }
            else if (unit.Stats.Faction == UnitFactionType.Defender)
            {
                _defenderUnits.Remove(unit);
            }
            _deadUnits.Add(unit);
            
        }


        public void RemoveUnit(IUnit unit)
        {
            if (unit.UnitState.CurrentState == UnitState.Die)
            {
                unit.Disable();
                _deadUnits.Remove(unit);
            }
            else
            {
                unit.OnReachedZeroHealth -= UnitReachedZeroHealth;

                switch (unit.Stats.Faction)
                {
                    default:
                        break;
                    case UnitFactionType.Enemy:
                    {
                        //Debug.Log($"FifthBattleController->RemoveUnit: [{unit.Id}]_{unit.Stats.Role} - dead");

                        unit.Target.ResetTarget();
                        unit.Disable();
                        _enemyUnits.Remove(unit);

                        break;
                    }
                    case UnitFactionType.Defender:
                    {
                        //Debug.Log($"FifthBattleController->RemoveUnit: {unit.View.SelfTransform.gameObject.name}");

                        unit.Target.ResetTarget();
                        unit.Disable();
                        _defenderUnits.Remove(unit);
                        break;
                    }
                }
            }
        }

        private void UpdateTargetExistence(IUnit unit)
        {
            switch (unit.Priority.Current.Priority)
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

                    IAttackTarget target = unit.Target.CurrentTarget;
                    if (!target.IsAlive)
                    {
                        unit.Target.ResetTarget();
                        ChangeUnitState(unit, UnitState.Idle);
                    }
                    else if (!list.Exists(def => def.Id == target.Id)) 
                    {
                        unit.Target.ResetTarget();
                        ChangeUnitState(unit, UnitState.Idle);
                    }
                    break;
                }
            }
        }

        private void UnitIdleState(IUnit unit, float deltaTime)
        {
            //Debug.Log("FifthBattleController->UnitIdleState:");
            IAttackTarget target = GetTarget(unit);

            // if (target.Id < 0)
            // {
            //     Debug.Log("FifthBattleController->UnitIdleState: target.Id < 0 ");
            // }
            // if (target is NoneTarget)
            // {
            //     Debug.Log("FifthBattleController->UnitIdleState: target is NoneTarget ");
            // }

            if (target.Id >= 0)
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
            IAttackTarget target = GetTarget(unit);
            if (target.Id >= 0)
            {
                unit.Target.SetTarget(target);
                ChangeUnitState(unit, UnitState.Approach);
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
            float distanceSqr = (unit.GetPosition() - targetPos).sqrMagnitude;
            if (distanceSqr <= unit.Offence.MaxRange * unit.Offence.MaxRange)
            {
                StopUnit(unit);

                ChangeUnitState(unit, UnitState.Attack);
            }
            else
            {
                unit.Navigation.MoveTo(targetPos);
            }
        }

        private void UnitAttackState(IUnit unit, float deltaTime)
        {
            IAttackTarget target = unit.Target.CurrentTarget;
            
            if (target != null) 
            {
                if (IsAttackDistanceSuitable(unit, target.Position))
                {
                    switch (unit.UnitState.CurrentAttackPhase)
                    {
                        case AttackPhase.None:
                            unit.TimeProgress = deltaTime;
                            unit.UnitState.CurrentAttackPhase = AttackPhase.Cast;
                            break;
                        case AttackPhase.Cast:
                            unit.TimeProgress += deltaTime;
                            if (unit.TimeProgress >= unit.Offence.CastingTime)
                            {

                                target.TakeDamage(unit.Offence.GetDamage());
                                unit.UnitState.CurrentAttackPhase = AttackPhase.None;
                                unit.TimeProgress = 0.0f;

                                StartAttackAnimation(unit);
                                //StartTakeDamageAnimation(unitTarget);
                            }

                            break;
                        case AttackPhase.Attack:
                            throw new NotImplementedException();
                            //break;
                        default:
                            throw new ArgumentOutOfRangeException();
                            //break;
                    }
                }
                else
                {
                    unit.UnitState.CurrentAttackPhase = AttackPhase.None;
                    ChangeUnitState(unit, UnitState.Approach);
                }

            }
            else
            {
                unit.Target.ResetTarget();
                ChangeUnitState(unit, UnitState.Idle);
            }

        }

        private bool IsAttackDistanceSuitable(IUnit attacker, Vector3 targetPosition)
        {
            Vector3 attackerPosition = attacker.GetPosition();
            float maxAttackDistance = attacker.Offence.MaxRange;
            
            return maxAttackDistance * maxAttackDistance >= (attackerPosition - targetPosition).sqrMagnitude;
        }

        private void UnitDeadState(IUnit unit, float deltaTime)
        {
            unit.TimeProgress += deltaTime;
            if (unit.TimeProgress >= _deadUnitsDestroyDelay)
            {
                RemoveUnit(unit);
            }
        }

        private void ChangeUnitState(IUnit unit, UnitState newState)
        {
            unit.UnitState.ChangeState(newState);
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                switch (newState)
                {
                    case UnitState.None:
                        animView.Reset();
                        break;
                    case UnitState.Idle:
                        animView.IsMoving = false;
                        break;
                    case UnitState.Move:
                        animView.IsMoving = true;
                        break;
                    case UnitState.Approach:
                        animView.IsMoving = true;
                        break;
                    case UnitState.Search:
                        break;
                    case UnitState.Attack:
                        animView.IsMoving = false;
                        break;
                    case UnitState.Die:
                        unit.Target.ResetTarget();
                        unit.Navigation.Stop();
                        unit.TimeProgress = 0.0f;
                        animView.StartDead();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        private void StartAttackAnimation(IUnit unit)
        {
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                animView.StartCast();
            }
        }

        private void StartTakeDamageAnimation(IUnit unit)
        {
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                animView.TakeDamage();
            }
        }

        #region Movement logic

        private void MoveUnitToTarget(IUnit unit, IAttackTarget target)
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

            return (position - destination).sqrMagnitude <= _destinationPositionErrorSqr;
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

        #region Find target logic

        private IAttackTarget GetTarget(IUnit unit)
        {
            IAttackTarget result = new NoneTarget();

            while (unit.Priority.GetNext())
            {
                var currentPriority = unit.Priority.Current;

                switch (currentPriority.Priority)
                {
                    default:
                    case UnitPriorityType.MainTower:
                        {
                            result = _targetFinder.GetMainTower();
                            break;
                        }

                    case UnitPriorityType.ClosestFoe:
                        {
                            result = GetClosestFoe(unit);
                            break;
                        }
                    case UnitPriorityType.SpecificFoe:
                        {
                            result = GetClosestFoe(unit, currentPriority.Type);
                            break;
                        }
                }

                if (result is not NoneTarget)
                {
                    break;
                }
            }

            unit.Priority.Reset();

            return result;
        }

        private IAttackTarget GetClosestFoe(IUnit unit, UnitType targetType = UnitType.None)
        {
            IAttackTarget target = new NoneTarget();
            
            List<IUnit> foeUnitList = (unit.Stats.Faction == UnitFactionType.Enemy) ? _defenderUnits : _enemyUnits;

            Vector3 unitPos = unit.GetPosition();
            float minDist = float.MaxValue;

            for (int i = 0; i < foeUnitList.Count; i++)
            {
                IUnit foeUnit = foeUnitList[i];

                if( (targetType != UnitType.None && foeUnit.Stats.Type != targetType) || !foeUnit.IsAlive )
                    continue;

                Vector3 defenderPos = foeUnit.GetPosition();

                float distance = Vector3.Distance(unitPos, defenderPos);
                if (distance < minDist)
                {
                    minDist = distance;
                    
                    
                    target = new TargetModel(foeUnit, foeUnit.View);
                }
            }

            return target;
        }

        #endregion
    }
}
