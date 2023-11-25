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
                    case UnitState.Attack:
                        {
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

        protected override void UnitIdleState(IUnit unit, float deltaTime)
        {
            IAttackTarget target = GetTarget(unit);
            unit.Target.SetTarget(target);

            MoveUnitToTarget(unit, unit.Target.CurrentTarget);
            ChangeUnitState(unit, UnitState.Move);
        }

        protected override void UnitMoveState(IUnit unit, float deltaTime)
        {
            Vector3 turgentPos = unit.Target.CurrentTarget.Position;
            float distance = GetDistanceToTarget(unit.GetPosition(), turgentPos);
            if (CheckStopDistance(distance, unit.Offence.MaxRange) == true)
            {
                StopUnit(unit);

                ChangeUnitState(unit, UnitState.Attack);
            }
        }

        protected override void UnitAttackState(IUnit unit, float deltaTime)
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
                            }

                            break;
                        case AttackPhase.Attack:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    unit.UnitState.CurrentAttackPhase = AttackPhase.None;
                    ChangeUnitState(unit, UnitState.Move);
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

        private void StartAttackAnimation(IUnit unit)
        {
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                animView.StartCast();
            }
        }

        protected override void UnitDeadState(IUnit unit, float deltaTime) { }

        private void ExecuteFight(IDamageDealer damageDealer, IDamageable damageableTarget)
        {
            var damage = damageDealer.GetAttackDamage();
            damageableTarget.TakeDamage(damage);
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

        #region Enemy finding logic

        private IAttackTarget GetTarget(IUnit unit)
        {
            //Debug.Log("EnemyBattleController->GetTarget:");
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

            Vector3 unitPos = unit.GetPosition();
            float minDist = float.MaxValue;

            for (int i = 0; i < unitsContainer.DefenderUnits.Count; i++)
            {
                IUnit defender = unitsContainer.DefenderUnits[i];

                if(targetType != UnitType.None && defender.Stats.Type != targetType)
                    continue;

                Vector3 defenderPos = defender.GetPosition();

                float distance = Vector3.Distance(unitPos, defenderPos);
                if (distance < minDist)
                {
                    //minDist = distance;
                    //target = defender.View;
                    throw new NotImplementedException();
                }
            }

            return target;
        }


        #endregion
    }
}
