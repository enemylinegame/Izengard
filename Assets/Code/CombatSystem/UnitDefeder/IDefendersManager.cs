using Code.TileSystem;


namespace CombatSystem
{
    public interface IDefendersManager
    {
        void HireDefender();
        void DismissDefender(DefenderUnit[] units);
        void SendToBarrack(DefenderUnit[] units);
        void KickoutFromBarrack(DefenderUnit[] units);
        void SendToOtherTile(DefenderUnit[] units, TileView tile);
        void BarrackButtonClick();
    }
}
