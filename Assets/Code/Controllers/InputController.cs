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
        private UIController _uiController;
        private ITileSelector _tileSelector;
        private List<ITileLoadInfo> _loadInfoToTheUis = new List<ITileLoadInfo>();

        private bool _isOnTile = true;
        private bool _isSpecialMode;

        public InputController()
        {
           
        }


        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!_isSpecialMode)
                {
                    foreach (var selector in _loadInfoToTheUis)
                    {
                        selector.Cancel();
                        _isOnTile = true;
                    }
                    
                }
                else
                {
                    _tileSelector.Cancel();
                }

            }
            if (Input.GetMouseButtonDown(0))
            {
                var bitmask = ~(1 << 3);
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100, bitmask))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;
                    var currBuild = hit.collider.gameObject.GetComponentInParent<BuildingView>();
                    var currMine = hit.collider.gameObject.GetComponentInParent<Mineral>();
                    var tile = hit.collider.gameObject.GetComponentInParent<TileView>();

                    if (tile)
                    {
                        if (!_isSpecialMode)
                        {
                            if (_isOnTile)
                            {
                                foreach (var selector in _loadInfoToTheUis)
                                {
                                    selector.LoadInfoToTheUI(tile);
                                }
                                _isOnTile = false;
                            }
                        }
                        else
                        {
                            _tileSelector.SelectTile(tile);
                        }
                    }
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