using Abstraction;
using BattleSystem.MainTower;
using Configs;
using UnitSystem;

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
