using System.Collections.Generic;
using Code.UI;
using Code.Units.HireDefendersSystem;
using CombatSystem;
using CombatSystem.Views;
using UnityEngine;


namespace Code.TileSystem
{
    public class DefendersManager : IDefendersManager, ITileLoadInfo, IOnTile
    {
        private const string CANNOT_SEND_DEFENDER_TO_TILE = 
            "Can't send defender to tile, target tile is full.";
        
        private readonly TileController _tileController;
        private readonly IDefendersControll _defendersController;
        private readonly WarsView _warsView;
        private readonly HireUnitView _hireUnitView;
        private readonly PaymentDefendersSystem _paymentSystem;

        private DefendersSet _defendersSet;
        
        private TileModel SelectedTileModel => _tileController.TileModel;
        private TileView SelectedTileView => _tileController.View;

        private int _nextDefenderTypeIndex;

        private bool _isHireDefenderPenelOpened;
        

        public DefendersManager(TileController tileController, IDefendersControll defendersController, 
            UIController uiController, HireUnitView hireUnitView, DefendersSet defendersSet, 
            PaymentDefendersSystem paymentSystem)
        {
            _tileController = tileController;
            _defendersController = defendersController;
            _warsView = uiController.WarsView;
            _warsView.SetDefendersManager(this);
            _defendersSet = defendersSet;
            _hireUnitView = hireUnitView;
            _hireUnitView.OnHireButtonClick += HireDefenderButtonClick;
            _hireUnitView.OnCloseButtonClick += CloseHireDefenderPanel;
            _paymentSystem = paymentSystem;
        }


        #region IDefendersManager

        public void HireDefender()
        {
            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < _defendersSet.Defenders.Count; i++)
            {
                sprites.Add( _defendersSet.Defenders[i].Icon);
            }
            _hireUnitView.Show(sprites);
            _isHireDefenderPenelOpened = true;
        }

        public void DismissDefender(List<DefenderUnit> units)
        {
            if (units.Count > 0)
            {
                List<DefenderUnit> defendersOnTile = SelectedTileModel.DefenderUnits;

                for (int i = 0; i < units.Count; i++)
                {
                    DefenderUnit defender = units[i];
                    if (defendersOnTile.Remove(defender))
                    {
                        defender.DefenderUnitDead -= DefenderDead;
                        _defendersController.DismissDefender(defender);
                    }
                }
            }
            _warsView.UpdateDefenders();
        }

        public void SendToBarrack(List<DefenderUnit> units)
        {
            if (units.Count > 0)
            {
                _defendersController.SendDefendersToBarrack(units, SelectedTileView);
            }
            _warsView.UpdateDefenders();
        }

        public void KickoutFromBarrack(List<DefenderUnit> units)
        {
            if (units.Count > 0)
            {
                _defendersController.KickDefendersOutOfBarrack(units, SelectedTileView);
            }
            _warsView.UpdateDefenders();
        }

        public void SendToOtherTile(List<DefenderUnit> units, TileView tile)
        {
            TileModel current = SelectedTileModel;
            TileModel other = tile.TileModel;
            if (current != other)
            {
                for (int i = 0; i < units.Count; i++)
                {
                    DefenderUnit unit = units[i];
                    if (!SendDefenderToTile(unit, tile))
                    {
                        break;
                    }

                }
            }
            _warsView.UpdateDefenders();
        }

        public void BarrackButtonClick()
        {
            Debug.LogWarning("DefendersManager->BarrackButtonClick: not implemented ");
        }
        
        #endregion

        private void StartHireDefenderProcess(DefenderSettings settings)
        {
            List<DefenderUnit> defendersOnTile = SelectedTileModel.DefenderUnits;
            int unitsQuantity = defendersOnTile.Count;
            if (unitsQuantity < SelectedTileModel.MaxWarriors)
            {
                if (_paymentSystem.PayForDefender(settings.HireCost))
                {
                    var unit = _defendersController.CreateDefender(SelectedTileView, settings);
                    defendersOnTile.Add(unit);
                    unit.Tile = SelectedTileModel;
                    unit.DefenderUnitDead += DefenderDead;
                }
            }
            _warsView.UpdateDefenders();
        }

        #region ITileLoadInfo
        
        public void LoadInfoToTheUI(TileView tile)
        {
            _warsView.SetDefenders(tile.TileModel.DefenderUnits);
        }

        public void Cancel()
        {
            CloseHireDefenderPanel();
            _warsView.ClearDefenders();
        }

        #endregion

        private void DefenderDead(DefenderUnit defender)
        {
            defender.Tile.DefenderUnits.Remove(defender);
            TileModel defendersTile = defender.Tile;
            defender.Tile = null;
            if (SelectedTileModel == defendersTile)
            {
                _warsView.UpdateDefenders();
            }
        }

        private bool SendDefenderToTile(DefenderUnit defender, TileView tile)
        {
            bool hasSent = false;

            TileModel destinationTile = tile.TileModel;
            if (destinationTile.DefenderUnits.Count < destinationTile.MaxWarriors)
            {
                if (defender.IsInBarrack)
                {
                    _defendersController.KickDefenderOutOfBarrack(defender, SelectedTileView);
                }
                defender.Tile.DefenderUnits.Remove(defender);
                destinationTile.DefenderUnits.Add(defender);
                defender.Tile = destinationTile;
                _defendersController.SendDefenderToTile(defender, tile);
                hasSent = true;
            }
            else
            {
                Debug.Log("DefendersManager->SendDefenderToTile: " + CANNOT_SEND_DEFENDER_TO_TILE);
            }

            return hasSent;
        }

        private void HireDefenderButtonClick(int index)
        {
            if (_isHireDefenderPenelOpened)
            {
                StartHireDefenderProcess(_defendersSet.Defenders[index]);
            }
            //CloseHireDefenderPanel();
        }

        private void CloseHireDefenderPanel()
        {
            _hireUnitView.Hide();
            _isHireDefenderPenelOpened = false;
        }
 
        
    }
}