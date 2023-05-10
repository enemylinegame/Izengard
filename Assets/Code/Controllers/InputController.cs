using Controllers.BaseUnit;
using UnityEngine;
using UnityEngine.EventSystems;
using BuildingSystem;
using Code.TileSystem;
using Code.UI;
using Controllers.OutPost;
using Views.BuildBuildingsUI;
using Interfaces;


namespace Controllers
{
    public class InputController : IOnController, IOnUpdate
    {
        private TileController _tileController;
        private UIController _uiController;
        private ITileSelector _tileSelector;

        private bool _isOnTile = true;
        private bool _isSpecialMode;


        public InputController(TileController tileController, UIController uiController)
        {
            _tileController = tileController;
            _uiController = uiController;
            _uiController.WarsView.SetInputController(this);
        }


        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!_isSpecialMode)
                {
                    _uiController.IsWorkUI(UIType.All, false);
                    _isOnTile = true;
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
                                _uiController.IsWorkUI(UIType.Tile, true);
                                _tileController.LoadInfo(tile);
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
    }
}