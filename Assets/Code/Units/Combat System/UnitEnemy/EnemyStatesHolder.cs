using System;
using System.Collections.Generic;
using CombatSystem.UnitEnemy.EnemyStates;
using Wave;

namespace CombatSystem.UnitEnemy
{
    public class EnemyStatesHolder : IDisposable
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
            Enemy unit,
            IEnemyAnimationController animationController) 
        {
            _enemyStates[EnemyStateType.Idle] = new EnemyIdleState(unit, animationController);
            _enemyStates[EnemyStateType.Move] = new EnemyMoveState(unit, animationController);
            _enemyStates[EnemyStateType.Attack] = new EnemyAttackState(unit, animationController);
            _enemyStates[EnemyStateType.SearchForTarget] = new EnemySearchForTargetState(unit, animationController);
            _enemyStates[EnemyStateType.ChangeTarget] = new EnemyChangeTargetState(unit, animationController);
            _enemyStates[EnemyStateType.Dying] = new EnemyDyingState(unit, animationController);

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
    }
}
