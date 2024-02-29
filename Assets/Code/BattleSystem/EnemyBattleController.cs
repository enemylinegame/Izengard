using Abstraction;
using BattleSystem.MainTower;
using Configs;
using SpawnSystem;
using Tools;
using UnitSystem;
using UnitSystem.Enum;

namespace BattleSystem
{
    public class EnemyBattleController : BaseBattleController
    {
        private readonly ISpawnController _enemySpawner;

        public EnemyBattleController(
            BattleSystemData data,
            TargetFinder targetFinder,
            IUnitsContainer unitsContainer,
            MainTowerController mainTower,
            ISpawnController enemySpawner) : base(data, targetFinder, unitsContainer, mainTower)
        {
            _enemySpawner = enemySpawner;

            this.unitsContainer.OnDefenderAdded += ResetUnitState;
        }

        public override void OnPause()
        {
            base.OnPause();

            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

                unit.Stop();
            }
        }

        private void ResetUnitState()
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

                unit.ChangeState(UnitStateType.Idle);
            }
        }

        protected override void MainTowerDestroyed()
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                var unit = unitsContainer.EnemyUnits[i];

                unit.Stop();
                unit.ChangeState(UnitStateType.None);
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
                unit.Stop();

                unit.ChangeState(UnitStateType.Idle);               
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
