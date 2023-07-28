using System.Collections.Generic;

using Code.TileSystem;


namespace CombatSystem
{
    public interface IDefendersControll
    {
        DefenderUnit CreateDefender(TileModel tile, DefenderSettings settings);
        void SendDefendersToBarrack(List<DefenderUnit> defenderUnits, TileModel tile);
        void SendDefenderToBarrack(DefenderUnit unit, TileModel tile);
        void KickDefendersOutOfBarrack(List<DefenderUnit> defenderUnits, TileModel tile);
        void KickDefenderOutOfBarrack(DefenderUnit unit, TileModel tile);
        void DismissDefender(DefenderUnit unit);
        void SendDefenderToTile(DefenderUnit unit, TileModel tile);

    }
}
