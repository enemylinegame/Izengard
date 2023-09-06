using System;
using EnemyUnit.Core;
using EnemyUnit.Interfaces;

namespace EnemyUnit
{
    public class EnemyController : IEnemyController
    {
        private readonly EnemyModel _model;
        private readonly EnemyView _view;
        private readonly EnemyCore _core;
        private readonly EnemyStatesHolder _statesHolder;

        public event Action<int> OnDeath;

        public int Index { get; private set; }

        public EnemyController(
            EnemyModel model, 
            EnemyView view,
            EnemyCore core,
            EnemyStatesHolder statesHolder)
        {
            _model = model;
            _view = view;
            _core = core;
            _statesHolder = statesHolder;

            _view.OnTakeDamage += TakeDamage;
            _statesHolder.CurrentState.OnStateComplete += ChangeToNewState;
        }

        private void TakeDamage(int damageAmount)
        {
            _model.DecreaseHealth(damageAmount);
            if (_model.CurrentHealth == 0)
            {
                ChangeToNewState(EnemyStateType.Dying);
                OnDeath?.Invoke(Index);
            }
        }

        private void ChangeToNewState(EnemyStateType state)
        {
            _statesHolder.CurrentState.OnStateComplete -= ChangeToNewState;

            switch (state)
            {
                default:
                    break;
                case EnemyStateType.Idle:
                    {
                        _statesHolder.ChangeState(EnemyStateType.Move);
                        break;
                    }
                case EnemyStateType.Move:
                    {
                        _statesHolder.ChangeState(EnemyStateType.SearchForTarget);
                        break;
                    }
            }

            _statesHolder.CurrentState.OnStateComplete += ChangeToNewState;
        }


        public void OnUpdate(float deltaTime)
        {
            _statesHolder.OnUpdate(deltaTime);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            _statesHolder.OnFixedUpdate(fixedDeltaTime);
        }

        public void SetIndex(int index) 
            => Index = index;


        public void Dispose()
        {
            _statesHolder.CurrentState.OnStateComplete -= ChangeToNewState;
            _statesHolder?.Dispose();
        }
    }
}
