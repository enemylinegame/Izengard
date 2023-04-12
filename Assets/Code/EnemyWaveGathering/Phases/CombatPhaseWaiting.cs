using Wave.Interfaces;


namespace Wave
{
    public class CombatPhaseWaiting : PhaseWaitingBase
    {
        private readonly ISendingEnemys _sendingEnemys;
        

        public CombatPhaseWaiting(ISendingEnemys sendingEnemys)
        {
            _sendingEnemys = sendingEnemys;
        }

        protected override bool IsConditionsMet() => _sendingEnemys.IsSending || (_sendingEnemys.LifeEnemys.Count > 0);
    }
}