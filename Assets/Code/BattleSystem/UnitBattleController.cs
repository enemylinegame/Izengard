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

        public event Action OnAllEnemyDestroyed;
        
        private readonly TargetFinder _targetFinder;
        private readonly IUnitsContainer _unitsContainer;
        
        private float _destinationPositionErrorSqr;
        private float _deadUnitsDestroyDelay;

        private bool _isLastEnemyDestroyedThisTact; 
        
        
        public UnitBattleController(BattleSystemConstants battleSystemConstants,
            IUnitsContainer unitsContainer,
            TargetFinder targetFinder)
        {
            _unitsContainer = unitsContainer;
            _targetFinder = targetFinder;
            _destinationPositionErrorSqr = 
                battleSystemConstants.DestinationPositionError * battleSystemConstants.DestinationPositionError;
            _deadUnitsDestroyDelay = battleSystemConstants.DeadUnitsDestroyDelay;
        }

        public void OnUpdate(float deltaTime)
        {
            
            for (int i = 0; i < _unitsContainer.EnemyUnits.Count; i++)
            {
                IUnit unit = _unitsContainer.EnemyUnits[i];

                ExecuteUnitUpdate(unit, deltaTime);
            }
            
            for (int i = 0; i < _unitsContainer.DefenderUnits.Count; i++)
            {
                IUnit unit = _unitsContainer.DefenderUnits[i];

                ExecuteUnitUpdate(unit, deltaTime);
            }
            
            for (int i = 0; i < _unitsContainer.DeadUnits.Count; i++)
            {
                IUnit unit = _unitsContainer.DeadUnits[i];

                UnitDeadState(unit, deltaTime);
            }

            if (_isLastEnemyDestroyedThisTact)
            {
                _isLastEnemyDestroyedThisTact = false;
                OnAllEnemyDestroyed?.Invoke();
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
            
            //Debug.Log("FifthBattleController->AddUnit: " + unit.View.SelfTransform.gameObject.name);
            
            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        if (!_unitsContainer.EnemyUnits.Contains(unit))
                        {
                            _unitsContainer.EnemyUnits.Add(unit);
                            unit.OnReachedZeroHealth += UnitReachedZeroHealth;
                            unit.Navigation.Enable();
                            unit.UnitState.ChangeState(UnitState.Idle);
                        }
                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        if (!_unitsContainer.DefenderUnits.Contains(unit))
                        {
                            _unitsContainer.DefenderUnits.Add(unit);
                            unit.OnReachedZeroHealth += UnitReachedZeroHealth;
                            unit.Navigation.Enable();
                            unit.UnitState.ChangeState(UnitState.Idle);
                        }
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
                _unitsContainer.EnemyUnits.Remove(unit);
                if (_unitsContainer.EnemyUnits.Count == 0)
                {
                    _isLastEnemyDestroyedThisTact = true;
                }
            }
            else if (unit.Stats.Faction == UnitFactionType.Defender)
            {
                _unitsContainer.DefenderUnits.Remove(unit);
            }
            _unitsContainer.DeadUnits.Add(unit);
            
        }

        public void RemoveUnit(IUnit unit)
        {
            if (unit.UnitState.CurrentState == UnitState.Die)
            {
                unit.Disable();
                _unitsContainer.DeadUnits.Remove(unit);
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
                        _unitsContainer.EnemyUnits.Remove(unit);

                        break;
                    }
                    case UnitFactionType.Defender:
                    {
                        //Debug.Log($"FifthBattleController->RemoveUnit: {unit.View.SelfTransform.gameObject.name}");

                        unit.Target.ResetTarget();
                        unit.Disable();
                        _unitsContainer.DefenderUnits.Remove(unit);
                        break;
                    }
                }
            }
        }

        private void UpdateTargetExistence(IUnit unit)
        {
            IAttackTarget target = unit.Target.CurrentTarget;
            if (!target.IsAlive)
            {
                unit.Target.ResetTarget();
                ChangeUnitState(unit, UnitState.Idle);
            }
        }

        private void UnitIdleState(IUnit unit, float deltaTime)
        {
            //Debug.Log("FifthBattleController->UnitIdleState:");
            IAttackTarget target = _targetFinder.GetTarget(unit);

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
                unit.Navigation.MoveTo(target.Position);
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
            IAttackTarget target = _targetFinder.GetTarget(unit);
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
                unit.Navigation.Stop();;

                ChangeUnitState(unit, UnitState.Attack);
            }
            else
            {
                if (unit.Target.IsTargetChangePosition())
                {
                    unit.Navigation.MoveTo(targetPos);
                }
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

        private bool CheckIsOnDestinationPosition(IUnit unit)
        {
            Vector3 destination = unit.StartPosition;
            Vector3 position = unit.GetPosition();

            return (position - destination).sqrMagnitude <= _destinationPositionErrorSqr;
        }
        
    }
}
