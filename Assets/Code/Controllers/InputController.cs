using System.Collections.Generic;
using Controllers.BaseUnit;
using UnityEngine;
using UnityEngine.EventSystems;
using BuildingSystem;
using Code.TileSystem;
using Code.TileSystem.Interfaces;
using Code.UI;
using Controllers.OutPost;
using Views.BuildBuildingsUI;
using Interfaces;


namespace Controllers
{
    public class InputController : IOnController, IOnUpdate
    {
        private CenterUI _centerUI;
        private ITileSelector _tileSelector;
        private List<ITileLoadInfo> _loadInfoToTheUis = new List<ITileLoadInfo>();

        private bool _isOnTile = true;
        private bool _isSpecialMode;

        public bool LockRightClick = false;


        public void OnUpdate(float deltaTime)
        {
            if(LockRightClick == true) return;
            
            if (Input.GetMouseButtonDown(1))
            {
                foreach (var selector in _loadInfoToTheUis) 
                    if (!_isSpecialMode) selector.Cancel();
    
                if (_isSpecialMode) _tileSelector.Cancel();

                _isOnTile = !_isSpecialMode;
            }

            
            if (!Input.GetMouseButtonDown(0)) return;

            var bitmask = (1 << 6);
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, bitmask)) return;

            if (EventSystem.current.IsPointerOverGameObject()) return;

            var currBuild = hit.collider.GetComponentInParent<BuildingView>();
            var currMine = hit.collider.GetComponentInParent<Mineral>();
            var tile = hit.collider.GetComponentInParent<TileView>();

            if (!tile) return;

            if (_isSpecialMode)
            {
                _tileSelector.SelectTile(tile);
            }
            else if (_isOnTile)
            {
                foreach (var selector in _loadInfoToTheUis)
                {
                    selector.LoadInfoToTheUI(tile);
                    _isOnTile = false;
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
    }
}