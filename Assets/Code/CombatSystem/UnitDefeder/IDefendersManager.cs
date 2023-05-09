namespace CombatSystem
{
    public interface IDefendersManager
    {
        void HireDefender();
        void DismissDefender(DefenderUnit unit);
        void SendToBarrack(DefenderUnit unit);
        void KickoutFromBarrack(DefenderUnit unit);
        void BarrackButtonClick();
    }
}
