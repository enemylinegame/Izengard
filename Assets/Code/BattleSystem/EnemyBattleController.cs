using Abstraction;
using Configs;
using SpawnSystem;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

namespace BattleSystem
{
    public class EnemyBattleController : BaseBattleController
    {
        private readonly ISpawnController _enemySpawner;

        public EnemyBattleController(
            BattleSystemData data,
            TargetFinder targetFinder,
            UnitsContainer unitsContainer,
            ISpawnController enemySpawner) : base(data, targetFinder, unitsContainer)
        {
            _enemySpawner = enemySpawner;

            unitsContainer.OnDefenderAdded += ResetUnitState;
        }

        private void ResetUnitState()
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

                unit.ChangeState(UnitStateType.Idle);
            }
        }

        protected override void ExecuteOnUpdate(float deltaTime)
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

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

        protected override void UpdateTargetExistance(ITarget target)
        {
            var linkedUnits
                = unitsContainer.EnemyUnits.FindAll(e => e.Target.CurrentTarget.Id == target.Id);

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
                unit.MoveTo(target.Position);
                unit.ChangeState(UnitStateType.Move);
            }
            else
            {
                unit.Stop();
                unit.ChangeState(UnitStateType.None);
            }
        }

        protected override void UnitMoveState(IUnit unit, float deltaTime)
        {
            var target = unit.Target.CurrentTarget;

            if (target is not NoneTarget)
            {
                float distanceSqr = (unit.GetPosition() - target.Position).sqrMagnitude;
                if (distanceSqr <= unit.Offence.MaxRange * unit.Offence.MaxRange)
                {
                    unit.Stop();
                    unit.ChangeState(UnitStateType.Attack);
                }
                else
                {
                    if (unit.Target.IsTargetChangePosition())
                    {
                        unit.MoveTo(target.Position);
                    }
                }
            }
            else
            {
                if (CheckIsOnDestinationPosition(unit))
                {
                    unit.ChangeState(UnitStateType.Idle);
                    unit.Stop();
                }
            }
        }

        protected override void UnitAttackState(IUnit unit, float deltaTime)
        {
            var target = unit.Target.CurrentTarget;

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
