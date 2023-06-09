using System.Collections.Generic;

using Code.TileSystem;


namespace CombatSystem
{
    public sealed class TileDefendersSquad
    {

        private List<DefenderUnit> _defenderUnits;
        private TileView _tileView;
        private bool _isDefendersInsideBarrack;


        public List<DefenderUnit> DefenderUnits { get => _defenderUnits; }

        public bool IsDefendersInside { get => _isDefendersInsideBarrack; set => _isDefendersInsideBarrack = value; }

        public TileView View { get => _tileView; }


        public TileDefendersSquad(TileView tileView)
        {
            _defenderUnits = new List<DefenderUnit>();
            _tileView = tileView;
        }

    }
}
