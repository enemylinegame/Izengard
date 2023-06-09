using System;
using Code.UI;

namespace Wave
{
    public class PeacefulPhaseWaiting : PhaseWaitingBase
    {
        private readonly float _peacefulPhaseDuration;
        private readonly Action<float> _uiAction;
        private float _timeLeft;
        private ITextVisualizationOnUI _notificationUI;
        

        public PeacefulPhaseWaiting(float peacefulPhaseDuration, Action<float> uiAction, ITextVisualizationOnUI notificationUI)
        {
            _peacefulPhaseDuration = peacefulPhaseDuration;
            _uiAction = uiAction;
            _notificationUI = notificationUI;
        }

        public override void StartPhase()
        {
            base.StartPhase();
            _notificationUI.BasicTemporaryUIVisualization("the end of the peaceful phase!!", 3);
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