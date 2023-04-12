using System;
using Wave.Interfaces;


namespace Wave
{
    public class PreparatoryPhaseWaiting : PhaseWaitingBase
    {
        public bool IsDowntime;
        private readonly float _preparatoryPhaseDuration;
        private readonly Action<float> _uiAction;
        private float _timeLeft;
        private readonly IDowntimeChecker _downtimeChecker;


        public PreparatoryPhaseWaiting(float preparatoryPhaseDuration, Action<float> uiAction, IDowntimeChecker downtimeChecker)
        {
            _preparatoryPhaseDuration = preparatoryPhaseDuration;
            _uiAction = uiAction;

            _downtimeChecker = downtimeChecker;
        }

        public override void StartPhase()
        {
            base.StartPhase();
            _timeLeft = _preparatoryPhaseDuration;
        }

        protected override bool IsConditionsMet() => _timeLeft > 0 && _downtimeChecker.IsDowntime;

        protected override void AditionalActions()
        {
            _timeLeft -= CONDITIONS_CHECK_TIMER;
            _uiAction?.Invoke((int)_timeLeft);
        }
    }
}