using System;
using Abstraction;
using Configs;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;


namespace BattleSystem
{
    public class UnitBattleController : BaseBattleController
    {        
        private float _destinationPositionErrorSqr;
        private float _deadUnitsDestroyDelay;


        public UnitBattleController(
            TargetFinder targetFinder, 
            UnitsContainer unitsContainer,
            BattleSystemConstants battleSystemConstants) : base(targetFinder, unitsContainer)
        {
            _destinationPositionErrorSqr =
                battleSystemConstants.DestinationPositionError * battleSystemConstants.DestinationPositionError;
            _deadUnitsDestroyDelay = battleSystemConstants.DeadUnitsDestroyDelay;
        }

        protected override void ExecuteOnUpdate(float deltaTime)
        {
            for (int i = 0; i < unitsContainer.DefenderUnits.Count; i++)
            {
                var unit = unitsContainer.DefenderUnits[i];

                switch (unit.State.Current)
                {
                    default:
                        break;

                    case UnitStateType.Idle:
                        {
                            UnitIdleState(unit, deltaTime);
                            break;
                        }
                    case UnitStateType.Move:
                        {
                            UnitMoveState(unit, deltaTime);
                            break;
                        }
                    case UnitStateType.Attack:
                        {
                            UpdateTargetExistence(unit);
                            UnitAttackState(unit, deltaTime);
                            break;
                        }
                    case UnitStateType.Die:
                        {
                            UnitDeadState(unit, deltaTime);
                            break;
                        }
                }
            }

            for (int i = 0; i < unitsContainer.DeadUnits.Count; i++)
            {
                IUnit unit = unitsContainer.DeadUnits[i];

                UnitDeadState(unit, deltaTime);
            }
        }

        private void UpdateTargetExistence(IUnit unit)
        {
            IAttackTarget target = unit.Target.CurrentTarget;
            if (target is NoneTarget)
            {
                unit.Target.ResetTarget();
                unit.ChangeState(UnitStateType.Idle);
            }
        }

        protected override void UnitIdleState(IUnit unit, float deltaTime)
        {
            var target = targetFinder.GetTarget(unit);

            if (target is not NoneTarget)
            {
                unit.Target.SetTarget(target);
                unit.ChangeState(UnitStateType.Move);
                unit.Navigation.MoveTo(target.Position);
            }
            else 
            {
                if (!CheckIsOnDestinationPosition(unit))
                {
                    unit.ChangeState(UnitStateType.Move);
                    unit.Navigation.MoveTo(unit.StartPosition);
                }
            }

        }

        protected override void UnitMoveState(IUnit unit, float deltaTime)
        {
            if (unit.Target.CurrentTarget is not NoneTarget)
            {
                var targetPos = unit.Target.CurrentTarget.Position;
                float distanceSqr = (unit.GetPosition() - targetPos).sqrMagnitude;
                if (distanceSqr <= unit.Offence.MaxRange * unit.Offence.MaxRange)
                {
                    unit.Navigation.Stop();
                    unit.ChangeState(UnitStateType.Attack);
                }
                else
                {
                    if (unit.Target.IsTargetChangePosition())
                    {
                        unit.Navigation.MoveTo(targetPos);
                    }
                }
            }
            else 
            {
                if (CheckIsOnDestinationPosition(unit))
                {
                    unit.ChangeState(UnitStateType.Idle);
                    unit.Navigation.Stop();
                }
            }
        }


        protected override void UnitAttackState(IUnit unit, float deltaTime)
        {
            IAttackTarget target = unit.Target.CurrentTarget;
            
            if (target != null) 
            {
                if (IsAttackDistanceSuitable(unit, target.Position))
                {
                    switch (unit.State.CurrentAttackPhase)
                    {
                        case AttackPhase.None:
                            unit.TimeProgress = deltaTime;
                            unit.State.CurrentAttackPhase = AttackPhase.Cast;
                            break;
                        case AttackPhase.Cast:
                            unit.TimeProgress += deltaTime;
                            if (unit.TimeProgress >= unit.Offence.CastingTime)
                            {

                                target.TakeDamage(unit.Offence.GetDamage());
                                unit.State.CurrentAttackPhase = AttackPhase.None;
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
                    unit.State.CurrentAttackPhase = AttackPhase.None;
                    unit.ChangeState(UnitStateType.Move);
                }

            }
            else
            {
                unit.Target.ResetTarget();
                unit.ChangeState(UnitStateType.Idle);
            }

        }

        private bool IsAttackDistanceSuitable(IUnit attacker, Vector3 targetPosition)
        {
            Vector3 attackerPosition = attacker.GetPosition();
            float maxAttackDistance = attacker.Offence.MaxRange;
            
            return maxAttackDistance * maxAttackDistance >= (attackerPosition - targetPosition).sqrMagnitude;
        }

        protected override void UnitDeadState(IUnit unit, float deltaTime)
        {
            unit.TimeProgress += deltaTime;
            if (unit.TimeProgress >= _deadUnitsDestroyDelay)
            {
                unitsContainer.RemoveUnit(unit);
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

        private bool CheckIsOnDestinationPosition(IUnit unit)
        {
            Vector3 destination = unit.StartPosition;
            Vector3 position = unit.GetPosition();

            return (position - destination).sqrMagnitude <= _destinationPositionErrorSqr;
        }
    }
}
