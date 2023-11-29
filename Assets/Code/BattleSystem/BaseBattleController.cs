using Abstraction;
using Configs;
using UnitSystem;

namespace BattleSystem
{
    public abstract class BaseBattleController : IOnController, IOnUpdate
    {
        protected readonly TargetFinder targetFinder;
        protected readonly UnitsContainer unitsContainer;
        
        protected readonly float destinationPositionErrorSqr;
        protected readonly float deadUnitsDestroyDelay;

        public BaseBattleController(
            BattleSystemData data,
            TargetFinder targetFinder, 
            UnitsContainer unitsContainer)
        {
            destinationPositionErrorSqr = data.DestinationPositionError * data.DestinationPositionError;
            deadUnitsDestroyDelay = data.UnitsDestroyDelay;

            this.targetFinder = targetFinder;
            this.unitsContainer = unitsContainer;

            this.unitsContainer.OnUnitDead += UpdateTargetExistance;
        }

        public void OnUpdate(float deltaTime) 
        {
            ExecuteOnUpdate(deltaTime);
        }

        protected abstract void ExecuteOnUpdate(float deltaTime);
        protected abstract void UpdateTargetExistance(ITarget target);

        protected virtual void UnitIdleState(IUnit unit, float deltaTime) { }
        
        protected virtual void UnitMoveState(IUnit unit, float deltaTime) { }
        
        protected virtual void UnitAttackState(IUnit unit, float deltaTime) { }

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

    }
}
