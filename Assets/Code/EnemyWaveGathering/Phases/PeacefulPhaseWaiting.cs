using System;


namespace Wave
{
    public class PeacefulPhaseWaiting : PhaseWaitingBase
    {
        private readonly float _peacefulPhaseDuration;
        private readonly Action<float> _uiAction;
        private float _timeLeft;
        

        public PeacefulPhaseWaiting(float peacefulPhaseDuration, Action<float> uiAction)
        {
            _peacefulPhaseDuration = peacefulPhaseDuration;
            _uiAction = uiAction;
        }

        public override void StartPhase()
        {
            base.StartPhase();
            _timeLeft = _peacefulPhaseDuration;
        }

        protected override bool IsConditionsMet() => _timeLeft > 0;

        protected override void AditionalActions()
        {
            _timeLeft -= CONDITIONS_CHECK_TIMER;
            _uiAction?.Invoke((int)_timeLeft);
        }
    }
}