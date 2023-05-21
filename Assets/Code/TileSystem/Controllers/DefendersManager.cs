using System.Collections.Generic;
using System.Linq;
using Code.TileSystem.Interfaces;
using Code.UI;
using CombatSystem;
using CombatSystem.Views;
using UnityEngine;

namespace Code.TileSystem
{
    public class DefendersManager : IDefendersManager, ITileLoadInfo
    {
        private readonly TileController _tileController;
        private readonly IDefendersControll _defendersController;
        private readonly WarsView _warsView;

        
        private TileModel SelectedTileModel => _tileController.TileModel;
        private TileView SelectedTileView => _tileController.View;
        

        public DefendersManager(TileController tileController, IDefendersControll defendersController, 
            UIController uiController)
        {
            _tileController = tileController;
            _defendersController = defendersController;
            _warsView = uiController.WarsView;
            _warsView.SetDefendersManager(this);
        }

        public void HireDefender()
        {
            List<DefenderUnit> defendersOnTile = SelectedTileModel.DefenderUnits;
            int unitsQuantity = defendersOnTile.Count;
            if (unitsQuantity < SelectedTileModel.MaxWarriors)
            {
                var unit = _defendersController.CreateDefender(SelectedTileView);
                defendersOnTile.Add(unit);
                unit.Tile = SelectedTileModel; 
                unit.DefenderUnitDead += DefenderDead;
            }
            _warsView.UpdateDefenders();
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
            Debug.LogWarning("DefendersMenager->BarrackButtonClick: not implemented ");
        }

        public void BarrackButtonClick()
        {
            Debug.LogWarning("DefendersMenager->BarrackButtonClick: not implemented ");
        }

        public void LoadInfoToTheUI(TileView tile)
        {
            _warsView.SetDefenders(tile.TileModel.DefenderUnits);
        }

        public void Cancel()
        {
            _warsView.ClearDefenders();
        }

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
    }
}