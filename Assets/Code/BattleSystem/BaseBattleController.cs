using Abstraction;
using BattleSystem.MainTower;
using Configs;
using Tools;
using UnitSystem;
using UnitSystem.Enum;

namespace BattleSystem
{
    public abstract class BaseBattleController : IOnController, IOnUpdate, IPaused
    {
        protected readonly TargetFinder targetFinder;
        protected readonly IUnitsContainer unitsContainer;

        protected readonly float destinationPositionErrorSqr;
        protected readonly float deadUnitsDestroyDelay;

        private readonly MainTowerController _mainTower;

        public BaseBattleController(
            BattleSystemData data,
            TargetFinder targetFinder,
            IUnitsContainer unitsContainer,
            MainTowerController mainTower)
        {
            destinationPositionErrorSqr = data.DestinationPositionError * data.DestinationPositionError;
            deadUnitsDestroyDelay = data.UnitsDestroyDelay;

            this.targetFinder = targetFinder;
            this.unitsContainer = unitsContainer;
            this.unitsContainer.OnUnitDead += UpdateTargetExistance;

            _mainTower = mainTower;
            _mainTower.OnMainTowerDestroyed += MainTowerDestroyed;
        }


        public void OnUpdate(float deltaTime)
        {
            if (IsPaused)
                return;

            ExecuteOnUpdate(deltaTime);
        }

        protected abstract void MainTowerDestroyed();

        protected abstract void ExecuteOnUpdate(float deltaTime);
        protected abstract void UpdateTargetExistance(ITarget target);

        protected virtual void UnitIdleState(IUnit unit, float deltaTime) 
        {
            var target = targetFinder.GetTarget(unit);

            if (target is not NoneTarget)
            {
                unit.Target.SetTarget(target);

                unit.MoveTo(target.Position);

                unit.ChangeState(UnitStateType.Move);
            }
        }

        protected virtual void UnitMoveState(IUnit unit, float deltaTime) 
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
                unit.Stop();

                unit.ChangeState(UnitStateType.Idle);
            }
        }

        protected virtual void UnitAttackState(IUnit unit, float deltaTime) 
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
                            {
                                unit.TimeProgress = deltaTime;
                                unit.State.CurrentAttackPhase = AttackPhase.Cast;

                                var dir = unit.Target.CurrentTarget.Position - unit.GetPosition();
                                unit.SetRotation(dir);

                                StartAttackAnimation(unit);
                            }
                            break;
                        case AttackPhase.Cast:

                            unit.TimeProgress += deltaTime;

                            if (unit.TimeProgress >= unit.Offence.CastingTime)
                            {
                                var damage = unit.Offence.GetDamage();

                                DebugGameManager.Log($"{unit.Name} deal [{damage.BaseDamage} + {damage.FireDamage} + {damage.ColdDamage}] damamage to {target.Name}",
                                   new[] { DebugTags.Unit, DebugTags.Damage });

                                target.TakeDamage(damage);

                                unit.State.CurrentAttackPhase = AttackPhase.Attack;
                            }

                            break;
                        case AttackPhase.Attack:
                            {
                                unit.TimeProgress += deltaTime;

                                if (unit.TimeProgress >= unit.Offence.AttackTime)
                                {
                                    unit.State.CurrentAttackPhase = AttackPhase.None;
                                    unit.TimeProgress = 0.0f;
                                }
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

        private void StartAttackAnimation(IUnit unit)
        {
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                animView.StartCast();
            }
        }

        protected virtual void UnitDeadState(IUnit unit, float deltaTime) { }

        protected bool IsAttackDistanceSuitable(IUnit attacker)
        {
            var attackerPosition = attacker.GetPosition();
            var targetPosition = attacker.Target.CurrentTarget.Position;

            float maxAttackDistance = attacker.Offence.MaxRange;

            return maxAttackDistance * maxAttackDistance >= (attackerPosition - targetPosition).sqrMagnitude;
        }

        protected bool CheckIsOnDestinationPosition(IUnit unit)
        {
            var destination = unit.StartPosition;
            var position = unit.GetPosition();

            return (position - destination).sqrMagnitude <= destinationPositionErrorSqr;
        }

        #region IPaused
        
        public bool IsPaused { get; private set; }

        public virtual void OnPause()
        {
            IsPaused = true;
        }

        public virtual void OnRelease()
        {
            IsPaused = false;
        }

        #endregion
    }
}
