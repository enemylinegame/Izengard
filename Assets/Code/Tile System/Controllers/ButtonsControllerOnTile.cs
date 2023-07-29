using System.Collections.Generic;
using Code.UI;
using UnityEngine.UI;

namespace Code.TileSystem
{
    public class ButtonsControllerOnTile
    {
        private readonly List<ButtonView> _holder;

        public ButtonsControllerOnTile(UIController uiController)
        {
            _holder = uiController.BottomUI.TileUIView.ButtonsHolder;
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
        }
        /// <summary>
        /// Checks the health level, if it changes, then the button changes
        /// </summary>
        /// <param name="model"></param>
        public void ButtonsChecker(TileModel model)
        {
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
        /// <param name="type"></param>
        /// <param name="model"></param>
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