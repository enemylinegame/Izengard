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
        private readonly InputController _inputController;
        private readonly List<ButtonView> _holder;

        public ButtonsControllerOnTile(TileUIView tileUIView, InputController inputController)
        {
            _inputController = inputController;
            _holder = tileUIView.ButtonsHolder;
        }
        /// <summary>
        /// Returns a button with a specific type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Button HolderButton(ButtonTypes type)
        {
            foreach (var view in _holder)
            {
                if (view.Type == type)
                {
                    view.gameObject.SetActive(true);
                    return view.Button;
                }
            }
            
            return null;
        } 
        /// <summary>
        /// Adding buttons to the list
        /// </summary>
        /// <param name="model"></param>
        /// <param name="level"></param>
        public void ButtonAddListener(TileModel model, LevelOfLifeButtonsCustomizer level)
        {
            if(model.CenterBuilding == null) return;
            var upgrade = HolderButton(ButtonTypes.Upgrade);
            HolderButton(ButtonTypes.Repair).onClick.AddListener(() =>
            {
                level.RepairBuilding(model);
                upgrade.gameObject.SetActive(true);
                HolderButton(ButtonTypes.Repair).gameObject.SetActive(false);
            });
            HolderButton(ButtonTypes.Recovery).onClick.AddListener(() =>
            {
                level.RecoveryBuilding(model);
                upgrade.gameObject.SetActive(true);
                HolderButton(ButtonTypes.Recovery).gameObject.SetActive(false);
            });
            
            if (model.TileType == TileType.All)
            {
                HolderButton(ButtonTypes.Destroy).gameObject.SetActive(false);
            }
            else
            {
                HolderButton(ButtonTypes.Destroy).gameObject.SetActive(true);
                HolderButton(ButtonTypes.Destroy).onClick.AddListener(() =>
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
                HolderButton(ButtonTypes.Repair).gameObject.SetActive(true);
                HolderButton(ButtonTypes.Recovery).gameObject.SetActive(false);
                HolderButton(ButtonTypes.Upgrade).gameObject.SetActive(false);
            }
            else if (model.CenterBuilding.CurrentHealth <= 0)
            {
                HolderButton(ButtonTypes.Repair).gameObject.SetActive(false);
                HolderButton(ButtonTypes.Recovery).gameObject.SetActive(true);
                HolderButton(ButtonTypes.Upgrade).gameObject.SetActive(false);
                
            }
            else
            {
                HolderButton(ButtonTypes.Repair).gameObject.SetActive(false);
                HolderButton(ButtonTypes.Recovery).gameObject.SetActive(false);
                HolderButton(ButtonTypes.Upgrade).gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// Removes all dependencies from buttons
        /// </summary>
        public void RemoveListeners(ButtonTypes type, TileModel model)
        {
            if(model.CenterBuilding == null) return;
            foreach (var view in _holder)
            {
                if (view.Type == type)
                {
                    view.Button.onClick.RemoveAllListeners();
                }
                else
                {
                    view.Button.onClick.RemoveAllListeners();
                }
            }
        }
    }



    public enum ButtonTypes
    {
        Upgrade,
        Recovery,
        Repair,
        Destroy,
        All
    }
}