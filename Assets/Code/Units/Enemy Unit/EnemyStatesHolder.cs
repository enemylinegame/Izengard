using System;
using System.Collections.Generic;
using CombatSystem;
using EnemyUnit.Core;
using EnemyUnit.EnemyStates;

namespace EnemyUnit
{
    public class EnemyStatesHolder : IDisposable, IOnUpdate
    {
        private readonly Dictionary<EnemyStateType, EnemyBaseState> _enemyStates 
            = new Dictionary<EnemyStateType, EnemyBaseState>();

        private EnemyBaseState _currentState;
        public EnemyBaseState CurrentState 
        {
            get => _currentState;
            private set
            {
                _currentState = value;
            }
        }

        public EnemyStatesHolder(
            EnemyModel model,
            IEnemyAnimationController animationController, 
            EnemyCore core)
        {
            _enemyStates[EnemyStateType.Idle] = new EnemyIdleState(model, animationController);
            _enemyStates[EnemyStateType.Move] = new EnemyMoveState(model, animationController, core);
            _enemyStates[EnemyStateType.Attack] = new EnemyAttackState(model, animationController);
            _enemyStates[EnemyStateType.SearchForTarget] = new EnemySearchForTargetState(model, animationController);
            _enemyStates[EnemyStateType.ChangeTarget] = new EnemyChangeTargetState(model, animationController);
            _enemyStates[EnemyStateType.Dying] = new EnemyDyingState(model, animationController);

            CurrentState = _enemyStates[EnemyStateType.Idle];
            CurrentState.OnEnter();
        }

        public void ChangeState(EnemyStateType targetState)
        {
            if (targetState == EnemyStateType.None)
                return;

            CurrentState.OnExit();
            CurrentState = _enemyStates[targetState];
            CurrentState.OnEnter();       
        }

        public void Dispose()
        {

            CurrentState.OnExit();
            CurrentState = default;

            _enemyStates.Clear();
        }

        public void OnUpdate(float deltaTime)
        {
            CurrentState.OnUpdate();
        }
    }
}
