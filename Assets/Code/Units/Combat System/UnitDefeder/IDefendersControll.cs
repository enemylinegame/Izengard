using System.Collections.Generic;

using Code.TileSystem;


namespace CombatSystem
{
    public interface IDefendersControll
    {
        DefenderUnit CreateDefender(TileView tile, DefenderSettings settings);
        void SendDefendersToBarrack(List<DefenderUnit> defenderUnits, TileView tile);
        void SendDefenderToBarrack(DefenderUnit unit, TileView tile);
        void KickDefendersOutOfBarrack(List<DefenderUnit> defenderUnits, TileView tile);
        void KickDefenderOutOfBarrack(DefenderUnit unit, TileView tile);
        void DismissDefender(DefenderUnit unit);
        void SendDefenderToTile(DefenderUnit unit, TileView tile);

    }
}
