using System;
using Abstraction;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

namespace BattleSystem
{
    public class EnemyBattleController : BaseBattleController
    {
        public EnemyBattleController(TargetFinder targetFinder, UnitsContainer unitsContainer) 
            : base(targetFinder, unitsContainer)
        {

        }

        protected override void ExecuteOnUpdate(float deltaTime)
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

                UpdateTarget(unit);
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
        }

        private void UpdateTarget(IUnit unit)
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
                        if(unit.Target.CurrentTarget is NoneTarget)
                        {
                            unit.ChangeState(UnitStateType.Idle);
                            return;
                        }

                        if (unit.Target.IsTargetChangePosition())
                        {
                            MoveUnitToTarget(unit, unit.Target.CurrentTarget);
                            unit.ChangeState(UnitStateType.Move);
                            return;
                        }

                        break;
                    }
            }
        }

        protected override void UnitIdleState(IUnit unit, float deltaTime)
        {
            IAttackTarget target = targetFinder.GetTarget(unit);
            unit.Target.SetTarget(target);

            MoveUnitToTarget(unit, unit.Target.CurrentTarget);
            unit.ChangeState(UnitStateType.Move);
        }

        protected override void UnitMoveState(IUnit unit, float deltaTime)
        {
            Vector3 turgentPos = unit.Target.CurrentTarget.Position;
            float distance = GetDistanceToTarget(unit.GetPosition(), turgentPos);
            if (CheckStopDistance(distance, unit.Offence.MaxRange) == true)
            {
                StopUnit(unit);

                unit.ChangeState(UnitStateType.Attack);
            }
        }

        protected override void UnitAttackState(IUnit unit, float deltaTime)
        {
            var target = unit.Target.CurrentTarget;

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
                            }

                            break;
                        case AttackPhase.Attack:
                            throw new NotImplementedException();
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

        }


        private bool IsAttackDistanceSuitable(IUnit attacker, Vector3 targetPosition)
        {
            Vector3 attackerPosition = attacker.GetPosition();
            float maxAttackDistance = attacker.Offence.MaxRange;

            return maxAttackDistance * maxAttackDistance >= (attackerPosition - targetPosition).sqrMagnitude;
        }

        private void StartAttackAnimation(IUnit unit)
        {
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                animView.StartCast();
            }
        }

 
        #region Enemy moving logic

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

        private bool CheckStopDistance(float curDistance, float stopDistance)
        {
            if (curDistance <= stopDistance)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
