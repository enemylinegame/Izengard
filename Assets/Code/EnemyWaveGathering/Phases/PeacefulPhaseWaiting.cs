using System;
using Code;
using TMPro;

namespace Wave
{
    public class PeacefulPhaseWaiting : PhaseWaitingBase
    {
        private readonly float _peacefulPhaseDuration;
        private readonly Action<float> _uiAction;
        private float _timeLeft;
        private BaseCenterText _centerText;
        

        public PeacefulPhaseWaiting(float peacefulPhaseDuration, Action<float> uiAction, BaseCenterText centerText)
        {
            _peacefulPhaseDuration = peacefulPhaseDuration;
            _uiAction = uiAction;
            _centerText = centerText;
        }

        public override void StartPhase()
        {
            base.StartPhase();
            _centerText.NotificationUI("the end of the peaceful phase!!", 3000);
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