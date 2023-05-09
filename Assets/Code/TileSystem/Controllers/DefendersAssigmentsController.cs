using System.Collections.Generic;
using Code.UI;
using CombatSystem;

namespace Code.TileSystem
{
    public class DefendersAssigmentsController : IDefendersManager
    {
        private TileController _tileController;
        private DefendersController _defendersController;

        private TileModel _tileModel => _tileController.TileModel;
        private TileView _tileView => _tileController.View;
        private List<DefenderUnit> _defenderUnits => _tileModel.DefenderUnits;

        private int _eightQuantity
        {
            get => _tileModel.EightQuantity;
            set => _tileModel.EightQuantity = value;
        }

        public DefendersAssigmentsController(TileController tileController, DefendersController defendersController, UIController uiController)
        {
            _tileController = tileController;
            _defendersController = defendersController;
            uiController.WarsView.DefendersManager = this;
        }

        public void HireDefender()
        {
            var unit = _defendersController.CreateDefender(_tileView);
            _tileModel.DefenderUnits.Add(unit);
        }

        public void DismissDefender(IDefenderUnitView unit)
        {
              // _defendersController.SendDefenderToBarrack(unit , _tileView);
              // _tileModel.DefenderUnits.Remove(unit);
        }

        public void SendToBarrack(IDefenderUnitView unit)
        {
            // _defendersController.SendDefenderToBarrack(unit, _tileView);
        }

        public void KickoutFromBarrack(IDefenderUnitView unit)
        {
            // _defendersController.KickDefenderOutOfBarrack(unit, _tileView);
        }

        public void BarrackButtonClick()
        {
            _defendersController.SendDefendersToBarrack(_tileModel.DefenderUnits, _tileView);
        }
    }
}