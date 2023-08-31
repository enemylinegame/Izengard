using System;
using Code.Player;
using Code.TileSystem;
using Code.Units.HireDefendersSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.UI
{
    public class RightPanelController : IOnTile, ITileLoadInfo
    {
        private RightPanel _view;
        public HireUnitUIView HireUnitView => _view.HireUnits;
        public event Action<int> TileSelected;
        
        public event Action OpenMarketButton;
        public RightPanelController(RightPanelFactory factory, InputController inputController)
        {
            _view = factory.GetView(factory.UIElementsConfig.RightPanel);
            DeactivateOpenMarketButton();
            inputController.Add(this);
            
            _view.OpenMarketButton.onClick.AddListener((() => OpenMarketButton?.Invoke()));
        }
        
        public void LoadInfoToTheUI(TileView tile)
        {
            ActivateOpenMarketButton();
        }

        public void Cancel()
        {
            DeactivateOpenMarketButton();
        }

        private void ActivateOpenMarketButton()
        {
            _view.OpenMarketButton.gameObject.SetActive(true);
        }

        private void DeactivateOpenMarketButton()
        {
            _view.OpenMarketButton.gameObject.SetActive(false);
        }
        
        public void TimeCountShow(float time)
        {
            _view.Timer.text = time.ToString();
        }

        public void StartSpawnTiles(GameConfig gameConfig)
        {
            _view.ButtonSelectTileFirst.image.sprite = gameConfig.FirstTile.IconTile;
            _view.ButtonSelectTileSecond.image.sprite = gameConfig.SecondTile.IconTile;
            _view.ButtonSelectTileThird.image.sprite = gameConfig.ThirdTile.IconTile;
        }

        public void SubscribeTileSelButtons()
        {
            _view.ButtonSelectTileFirst.onClick.AddListener(() => TileSelected?.Invoke(0));
            _view.ButtonSelectTileSecond.onClick.AddListener(() => TileSelected?.Invoke(1));
            _view.ButtonSelectTileThird.onClick.AddListener(() => TileSelected?.Invoke(2));
        }
        
        public void UnSubscribeTileSelButtons()
        {
            _view.ButtonSelectTileFirst.onClick.RemoveAllListeners();
            _view.ButtonSelectTileSecond.onClick.RemoveAllListeners();
            _view.ButtonSelectTileThird.onClick.RemoveAllListeners();
        }
        
        public void DeactivateTileSelButtons()
        {
            _view.ButtonSelectTileFirst.gameObject.SetActive(false);
            _view.ButtonSelectTileSecond.gameObject.SetActive(false);
            _view.ButtonSelectTileThird.gameObject.SetActive(false);
        }

        public Transform GetButtonParents()
        {
            return _view.buttonParents;
        }

        public void ActivateButtonParents()
        {
            _view.buttonParents.gameObject.SetActive(true);
        }
        public void DeactivateButtonParents()
        {
            _view.buttonParents.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _view.OpenMarketButton.onClick.RemoveAllListeners();
        }
    }
}