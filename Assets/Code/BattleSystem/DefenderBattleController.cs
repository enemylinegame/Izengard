using System;
using Abstraction;
using Configs;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;


namespace BattleSystem
{
    public class DefenderBattleController : BaseBattleController
    {        
        public DefenderBattleController(
            BattleSystemData data, 
            TargetFinder targetFinder, 
            UnitsContainer unitsContainer) : base(data, targetFinder, unitsContainer)
        {

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

        protected override void UpdateTargetExistance(ITarget target)
        {
            var linkedUnits
                = unitsContainer.DefenderUnits.FindAll(e => e.Target.CurrentTarget.Id == target.Id);

            if (linkedUnits == null)
                return;

            for (int i = 0; i < linkedUnits.Count; i++)
            {
                var unit = linkedUnits[i];

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
            
            if (target is not NoneTarget) 
            {
                if (IsAttackDistanceSuitable(unit))
                {
                    switch (unit.State.CurrentAttackPhase)
                    {
                        default:
                            break;

                        case AttackPhase.None:
                            unit.TimeProgress = deltaTime;
                            unit.State.CurrentAttackPhase = AttackPhase.Cast;

                            var dir = unit.Target.CurrentTarget.Position - unit.GetPosition();
                            unit.SetRotation(dir);

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
                            {
                                Debug.Log($"Unit - {unit.Stats.Faction} in Attack phase");
                                break;
                            }
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

        protected override void UnitDeadState(IUnit unit, float deltaTime)
        {
            base.UnitDeadState(unit, deltaTime);
        }

        private void StartAttackAnimation(IUnit unit)
        {
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                animView.StartCast();
            }
        }
    }
}
