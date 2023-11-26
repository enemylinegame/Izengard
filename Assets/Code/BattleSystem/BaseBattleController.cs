using System;
using UnitSystem;
using UnitSystem.Enum;

namespace BattleSystem
{
    public abstract class BaseBattleController : IOnController, IOnUpdate
    {
        protected readonly TargetFinder targetFinder;
        protected readonly UnitsContainer unitsContainer;
        
        public BaseBattleController(TargetFinder targetFinder, UnitsContainer unitsContainer)
        {
            this.targetFinder = targetFinder;
            this.unitsContainer = unitsContainer;
        }

        public void OnUpdate(float deltaTime) 
        {
            ExecuteOnUpdate(deltaTime);
        }

        protected abstract void ExecuteOnUpdate(float deltaTime);
        protected virtual void UnitIdleState(IUnit unit, float deltaTime) { }
        protected virtual void UnitMoveState(IUnit unit, float deltaTime) { }
        protected virtual void UnitAttackState(IUnit unit, float deltaTime) { }
        protected virtual void UnitDeadState(IUnit unit, float deltaTime) { }
    }
}
