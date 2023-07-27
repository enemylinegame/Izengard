using System.Collections.Generic;

using Code.TileSystem;


namespace CombatSystem
{
    public interface IDefendersManager
    {
        void HireDefender();
        void DismissDefender(List<DefenderPreview> units);
        void SendToBarrack(List<DefenderPreview> units);
        void KickoutFromBarrack(List<DefenderPreview> units);
        void SendToOtherTile(List<DefenderPreview> units, TileView tile);
        void BarrackButtonClick();
    }
}
