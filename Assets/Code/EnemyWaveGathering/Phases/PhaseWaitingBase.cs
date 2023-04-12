using System;
using Wave.Interfaces;


namespace Wave
{
    public abstract class PhaseWaitingBase : IPhaseWaiting, IOnUpdate
    {
        protected const float CONDITIONS_CHECK_TIMER = 0.5f;

        public event Action PhaseEnded;
        protected bool _isPhaseStarted;
        private float _conditionsCheckTimer;


        public virtual void StartPhase()
        {
            _isPhaseStarted = true;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isPhaseStarted) PhaseActions(deltaTime);
        }

        protected abstract bool IsConditionsMet();

        private void PhaseActions(float deltaTime)
        {
            _conditionsCheckTimer -= deltaTime;
            if (_conditionsCheckTimer <= 0)
            {
                _conditionsCheckTimer += CONDITIONS_CHECK_TIMER;
                AditionalActions();

                if (!IsConditionsMet()) EndPhase();
            }
        }

        protected virtual void AditionalActions()
        { }

        private void EndPhase()
        {
            _conditionsCheckTimer = 0;
            _isPhaseStarted = false;
            PhaseEnded?.Invoke();
        }
    }
}