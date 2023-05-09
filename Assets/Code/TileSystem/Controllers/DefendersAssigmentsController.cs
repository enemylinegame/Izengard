using System.Collections.Generic;
using Code.UI;
using CombatSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class DefendersAssigmentsController : IDefendersManager
    {
        private TileController _tileController;
        private DefendersController _defendersController;
        private UIController _uiController;

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
            _uiController = uiController;
            uiController.WarsView.DefendersManager = this;
        }

        public void HireDefender()
        {
            var unit = _defendersController.CreateDefender(_tileView);
            _tileModel.DefenderUnits.Add(unit);
            _uiController.WarsView.SetDefenders(_tileModel.DefenderUnits);
        }

        public void DismissDefender(DefenderUnit unit)
        {
            _defendersController.SendDefenderToBarrack(unit , _tileView);
            _tileModel.DefenderUnits.Remove(unit);
            Object.Destroy(unit.DefenderGameObject);
            _uiController.WarsView.SetDefenders(_tileModel.DefenderUnits);
        }

        public void SendToBarrack(DefenderUnit unit)
        {
            _defendersController.SendDefenderToBarrack(unit, _tileView);
            _uiController.WarsView.SetDefenders(_tileModel.DefenderUnits);
        }

        public void KickoutFromBarrack(DefenderUnit unit)
        {
            _defendersController.KickDefenderOutOfBarrack(unit, _tileView);
            _uiController.WarsView.SetDefenders(_tileModel.DefenderUnits);
        }

        public void BarrackButtonClick()
        {
            _defendersController.SendDefendersToBarrack(_tileModel.DefenderUnits, _tileView);
            _uiController.WarsView.SetDefenders(_tileModel.DefenderUnits);
        }
    }
}