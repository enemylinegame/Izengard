using System.Collections.Generic;
using Code.TileSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class TileMainBoardController
    {
        private readonly TileUIMainBoard _view;

        public TileMainBoardController(TileUIMainBoard view)
        {
            _view = view;
        }
        
        public void LoadAllTextsFieldsAndImaged(TileConfig config, TileModel model, out int currentLVl)
        {
            int hashCode = config.TileLvl.GetHashCode();
            _view.LvlText.text = $"{hashCode} LVL";
            currentLVl = hashCode;

            _view.MaxWorkersCount = config.MaxWorkers;
            _view.WorkersCount = model.WorkersCount;
            
            _view.Icon.sprite = config.IconTile;
            _view.NameTile.text = model.TileType.ToString();
        }

        public void ChangeWorkersCount(int value)
        {
            _view.WorkersCount = value;
        }

        public void UnSubscribeButtons()
        {
            _view.ButtonsHolder.ForEach(x => x.Button.onClick.RemoveAllListeners());
        }
        /// <summary>
        /// Returns a button with a specific type
        /// </summary>
        public Button HolderButton(ButtonTypes type)
        {
            foreach (var view in _view.ButtonsHolder)
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
        /// Removes all dependencies from buttons
        /// </summary>
        public void RemoveListeners(ButtonTypes type, TileModel model)
        {
            if(model.CenterBuilding == null) return;
            foreach (var view in _view.ButtonsHolder)
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
}