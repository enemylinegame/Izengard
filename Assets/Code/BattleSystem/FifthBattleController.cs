using System;
using Abstraction;
using EnemySystem;
using System.Collections.Generic;
using BattleSystem.Models;
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
        
        private class DeadUnit
        {
            public IUnit Unit;
            public float TimeLeft;

            public DeadUnit(IUnit unit)
            {
                Unit = unit;
                TimeLeft = DEAD_UNITS_DESTROY_DELAY;
            }
        }

        private const float DESTINATION_POSITION_ERROR_SQR = 0.3f * 0.3f;
        private const float DEAD_UNITS_DESTROY_DELAY = 10.0f;
        
        private List<IUnit> _enemyUnits;
        private List<IUnit> _defenderUnits;
        private List<AttackModel> _attackModels;
        private List<DeadUnit> _deadUnits;


        public FifthBattleController(TargetFinder targetFinder) : base(targetFinder)
        {
            _enemyUnits = new ();
            _defenderUnits = new ();
            _attackModels = new();
            _deadUnits = new();
        }

        public override void OnUpdate(float deltaTime)
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

            for (int i = _deadUnits.Count - 1; i >= 0; i--)
            {
                DeadUnit undead = _deadUnits[i];
                undead.TimeLeft -= deltaTime;
                if (undead.TimeLeft <= 0.0f)
                {
                    RemoveDeadUnit(undead);
                }
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
            unit.Target.ResetTarget();
            ChangeUnitState(unit, UnitState.Die);
            unit.Navigation.Stop();

            if (unit.Stats.Faction == UnitFactionType.Enemy)
            {
                _enemyUnits.Remove(unit);
            }
            else if (unit.Stats.Faction == UnitFactionType.Defender)
            {
                _defenderUnits.Remove(unit);
            }

            DeadUnit undead = new DeadUnit(unit);
            _deadUnits.Add(undead);

            //RemoveUnit(unit);
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
                        
                        unit.Target.ResetTarget();
                        unit.Disable();
                        _enemyUnits.Remove(unit);

                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        Debug.Log($"FifthBattleController->RemoveUnit: {unit.View.SelfTransform.gameObject.name}");
                        
                        unit.Target.ResetTarget();
                        unit.Disable();
                        _defenderUnits.Remove(unit);
                        break;
                    }
            }
        }

        private void RemoveDeadUnit(DeadUnit undead)
        {
            undead.Unit.Disable();
            _deadUnits.Remove(undead);
            //Debug.Log($"FifthBattleController->UnitReachedZeroHealth: {undead.Unit.View.SelfTransform.gameObject.name}");
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
                    if (!list.Exists(def => def.Id == target.Id)) 
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
                IUnit unitTarget = FindUnitByITarget(unit.Target.CurrentTarget);
                
                if (unitTarget != null) 
                {
                    if (IsAttackDistanceSuitable(unit, unitTarget))
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


                                    // Debug.Log("FifthBattleController->UnitAttackState: " +
                                    //           unit.View.SelfTransform.gameObject.name + " ==>> " +
                                    //           unitTarget.View.SelfTransform.gameObject.name);
                                    unitTarget.TakeDamage(unit.Offence.GetDamage());
                                    attack.Phase = AttackPhase.None;
                                    attack.TimingProgress = 0.0f;

                                    StartAttackAnimation(unit);
                                    StartTakeDamageAnimation(unitTarget);

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
                    else
                    {
                        attack.Phase = AttackPhase.None;
                        ChangeUnitState(unit, UnitState.Approach);
                    }

                }
                else
                {
                    unit.Target.ResetTarget();
                    ChangeUnitState(unit, UnitState.Idle);
                }
                
            }
        }

        private bool IsAttackDistanceSuitable(IUnit attacker, IUnit target)
        {
            Vector3 attackerPosition = attacker.GetPosition();
            Vector3 targetPosition = target.GetPosition();
            float maxAttackDistance = attacker.Offence.MaxRange;
            
            return maxAttackDistance * maxAttackDistance >= (attackerPosition - targetPosition).sqrMagnitude;
        }

        private IUnit FindUnitByITarget(IAttackTarget target)
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
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                switch (state)
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
                            result = targetFinder.GetMainTower();
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

                if(targetType != UnitType.None && foeUnit.Stats.Type != targetType)
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
