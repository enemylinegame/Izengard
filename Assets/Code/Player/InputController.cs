using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Code.TileSystem;
using Code.UI;
using Interfaces;


namespace Code.Player
{
    public class InputController : IOnController, IOnUpdate
    {
        private OutlineController _outlineController;
        private CenterUI _centerUI;
        private ITileSelector _tileSelector;
        private List<ITileLoadInfo> _loadInfoToTheUis = new List<ITileLoadInfo>();
        private TileView _tile;

        private bool _isSpecialMode;

        public bool IsOnTile = true;
        public bool LockRightClick = true;

        public InputController(OutlineController outlineController)
        {
            _outlineController = outlineController;
        }


        public void OnUpdate(float deltaTime)
        {
            if (!LockRightClick)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    foreach (var selector in _loadInfoToTheUis)
                        if (!_isSpecialMode)
                        {
                            selector.Cancel();
                            _outlineController.DisableOutLine(_tile.Renderer);
                        }
    
                    if (_isSpecialMode) _tileSelector.Cancel();

                    IsOnTile = !_isSpecialMode;
                    _tile = null;
                }
            }
            if (!Input.GetMouseButtonDown(0)) return;

            var bitmask = 1 << 6;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, bitmask)) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            var tile = hit.collider.GetComponentInParent<TileView>();
            
            if (!tile) return;
            _tile = tile;

            if (_isSpecialMode)
            {
                _tileSelector.SelectTile(tile);
            }
            else if (IsOnTile)
            {
                foreach (var selector in _loadInfoToTheUis)
                {
                    selector.LoadInfoToTheUI(tile);
                    _outlineController.EnableOutLine(tile.Renderer);
                    IsOnTile = false;
                }
            }
        }

        public void SetSpecialTileSelector(ITileSelector tileSelector)
        {
            _tileSelector = tileSelector;
            _isSpecialMode = _tileSelector != null;
        }

        public void Add(IOnTile tile)
        {
            if (tile is ITileLoadInfo loadInfoToTheUI)
            {
                _loadInfoToTheUis.Add(loadInfoToTheUI);
            }
            
        }
        public void HardOffTile()
        {
            foreach (var selector in _loadInfoToTheUis) 
                if (!_isSpecialMode) selector.Cancel();
    
            if (_isSpecialMode) _tileSelector.Cancel();

            IsOnTile = !_isSpecialMode;
        }
    }
}