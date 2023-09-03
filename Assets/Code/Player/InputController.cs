using System.Collections.Generic;
using Code.Level_Generation;
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
        private readonly TileGenerator _tileGenerator;
        private CenterPanel _centerPanel;
        private ITileSelector _tileSelector;
        private List<ITileLoadInfo> _loadInfoToTheUis;
        private TileView _tile;

        private bool _isSpecialMode;

        private bool LockLeftClick = false;
        private bool LockRightClick = true;

        public InputController(OutlineController outlineController, TileGenerator tileGenerator)
        {
            _outlineController = outlineController;
            _tileGenerator = tileGenerator;
            _loadInfoToTheUis = new List<ITileLoadInfo>();
        }


        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(0)) LeftMouseButton();
            else if(Input.GetMouseButtonDown(1)) RightMouseButton();
        }

        private void LeftMouseButton()
        {
            var bitmask = 1 << 6;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, bitmask)) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            var tile = hit.collider.GetComponentInParent<TileView>();
            var buttonSetterView = hit.collider.GetComponent<ButtonSetterView>();
            
            if(buttonSetterView != null && !LockLeftClick) buttonSetterView.OnClick();
            if (!tile) return;

            if (_isSpecialMode)
            {
                var model = _tileGenerator.LoadTile(tile);
                _tileSelector.SelectTile(tile, model);
            }
            
            if (_tile != null)
            {
                _loadInfoToTheUis.ForEach(selector =>
                {
                    selector.Cancel();
                    _outlineController.DisableOutLine(_tile.Renderer);
                });
                
                _tile = null;
                LockLeftClick = false;
            }
            
            if(LockLeftClick) return;
            
            _loadInfoToTheUis.ForEach(selector =>
            {
                var model = _tileGenerator.LoadTile(tile);
                selector.LoadInfoToTheUI(tile, model);
                _outlineController.EnableOutLine(tile.Renderer);
                LockLeftClick = true;
                LockRightClick = false;
            });
            _tile = tile;
        }
        private void RightMouseButton()
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
                            LockRightClick = true;
                        }
    
                    if (_isSpecialMode) _tileSelector.Cancel();

                    LockLeftClick = false;
                    _tile = null;
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
                if (!_isSpecialMode)
                {
                    selector.Cancel();
                    _outlineController.DisableOutLine(_tile.Renderer);
                    LockRightClick = true;
                }
    
            if (_isSpecialMode) _tileSelector.Cancel();

            LockLeftClick = false;
            _tile = null;
        }
    }
}