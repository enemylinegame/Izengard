namespace CombatSystem
{
    public interface IDefendersManager
    {
        void HireDefender();
        void DismissDefender(IDefenderUnitView unit);
        void SendToBarrack(IDefenderUnitView unit);
        void KickoutFromBarrack(IDefenderUnitView unit);
        void BarrackButtonClick();
    }
}
