using System.Collections.Generic;
using System.Linq;
using Code.BuildingSystem;
using Code.Player;
using Code.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Code.TileSystem
{
    public class ButtonsControllerOnTile
    {
        private readonly TileMainBoardController _tileMainBoard;
        private readonly InputController _inputController;

        public ButtonsControllerOnTile(TileMainBoardController tileMainBoard, InputController inputController)
        {
            _tileMainBoard = tileMainBoard;
            _inputController = inputController;
        }
        
        /// <summary>
        /// Adding buttons to the list
        /// </summary>
        /// <param name="model"></param>
        /// <param name="level"></param>
        public void ButtonAddListener(TileModel model, LevelOfLifeButtonsCustomizer level)
        {
            if(model.CenterBuilding == null) return;
            var upgrade = _tileMainBoard.HolderButton(ButtonTypes.Upgrade);
            _tileMainBoard.HolderButton(ButtonTypes.Repair).onClick.AddListener(() =>
            {
                level.RepairBuilding(model);
                upgrade.gameObject.SetActive(true);
                _tileMainBoard.HolderButton(ButtonTypes.Repair).gameObject.SetActive(false);
            });
            _tileMainBoard.HolderButton(ButtonTypes.Recovery).onClick.AddListener(() =>
            {
                level.RecoveryBuilding(model);
                upgrade.gameObject.SetActive(true);
                _tileMainBoard.HolderButton(ButtonTypes.Recovery).gameObject.SetActive(false);
            });
            
            if (model.TileType == TileType.All)
            {
                _tileMainBoard.HolderButton(ButtonTypes.Destroy).gameObject.SetActive(false);
            }
            else
            {
                _tileMainBoard.HolderButton(ButtonTypes.Destroy).gameObject.SetActive(true);
                _tileMainBoard.HolderButton(ButtonTypes.Destroy).onClick.AddListener(() =>
                {
                    while (true)
                    {
                        foreach (var build in model.FloodedBuildings.ToList())
                        {
                            if (build.BuildingTypes != 0)
                            {
                                model.FloodedBuildings.Remove(build);
                                Object.Destroy(build.Prefab);
                                if(!model.FloodedBuildings.Exists(building => building.BuildingTypes != 0)) break;
                            }
                        }
                        model.TileType = TileType.None;
                        model.CenterBuilding.gameObject.SetActive(false);
                        _inputController.HardOffTile();
                        return;
                    }
                });
                
            }
            
        }
        /// <summary>
        /// Checks the health level, if it changes, then the button changes
        /// </summary>
        public void ButtonsChecker(TileModel model)
        {
            if(model.CenterBuilding == null) return;
            if (model.CenterBuilding.CurrentHealth < model.CenterBuilding.MaxHealth &&
                model.CenterBuilding.CurrentHealth > 0)
            {
                _tileMainBoard.HolderButton(ButtonTypes.Repair).gameObject.SetActive(true);
                _tileMainBoard.HolderButton(ButtonTypes.Recovery).gameObject.SetActive(false);
                _tileMainBoard.HolderButton(ButtonTypes.Upgrade).gameObject.SetActive(false);
            }
            else if (model.CenterBuilding.CurrentHealth <= 0)
            {
                _tileMainBoard.HolderButton(ButtonTypes.Repair).gameObject.SetActive(false);
                _tileMainBoard.HolderButton(ButtonTypes.Recovery).gameObject.SetActive(true);
                _tileMainBoard.HolderButton(ButtonTypes.Upgrade).gameObject.SetActive(false);
                
            }
            else
            {
                _tileMainBoard.HolderButton(ButtonTypes.Repair).gameObject.SetActive(false);
                _tileMainBoard.HolderButton(ButtonTypes.Recovery).gameObject.SetActive(false);
                _tileMainBoard.HolderButton(ButtonTypes.Upgrade).gameObject.SetActive(true);
            }
        }
        
    }
}