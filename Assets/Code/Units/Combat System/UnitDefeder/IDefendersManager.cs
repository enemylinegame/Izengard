using System.Collections.Generic;

using Code.TileSystem;


namespace CombatSystem
{
    public interface IDefendersManager
    {
        void HireDefender();
        void DismissDefender(List<DefenderUnit> units);
        void SendToBarrack(List<DefenderUnit> units);
        void KickoutFromBarrack(List<DefenderUnit> units);
        void SendToOtherTile(List<DefenderUnit> units, TileView tile);
        void BarrackButtonClick();
    }
}
