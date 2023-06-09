using System;
using Code.UI;
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
        private ITextVisualizationOnUI _notificationUI;


        public PreparatoryPhaseWaiting(float preparatoryPhaseDuration, Action<float> uiAction, IDowntimeChecker downtimeChecker, ITextVisualizationOnUI notificationUI)
        {
            _preparatoryPhaseDuration = preparatoryPhaseDuration;
            _uiAction = uiAction;
            _downtimeChecker = downtimeChecker;
            _notificationUI = notificationUI;
        }

        public override void StartPhase()
        {
            base.StartPhase();
            _notificationUI.BasicTemporaryUIVisualization("end of the military phase!!", 3);
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