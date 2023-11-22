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
     
        protected virtual void ChangeUnitState(IUnit unit, UnitState state)
        {
            unit.UnitState.ChangeState(state);
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                switch (state)
                {
                    case UnitState.None:
                        animView.Reset();
                        break;
                    case UnitState.Idle:
                        animView.IsMoving = false;
                        break;
                    case UnitState.Move:
                        animView.IsMoving = true;
                        break;
                    case UnitState.Search:
                        break;
                    case UnitState.Attack:
                        animView.IsMoving = false;
                        break;
                    case UnitState.Die:
                        animView.StartDead();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
