using Controllers.BaseUnit;
using UnityEngine;
using UnityEngine.EventSystems;
using BuildingSystem;
using Code.TileSystem;
using Code.UI.Controllers;
using Controllers.OutPost;
using Views.BuildBuildingsUI;

namespace Controllers
{
    public class InputController : IOnController, IOnUpdate
    {
        private TileController _tileController;
        private UIController _uiController;
        private bool _isOnTile = true;

        public InputController(TileController tileController, UIController uiController)
        {
            _tileController = tileController;
            _uiController = uiController;
        }


        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _uiController.IsWorkUI(UIType.All, false);
                _isOnTile = true;
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
                        if (_isOnTile)
                        {
                            _uiController.IsWorkUI(UIType.Tile, true);
                            _tileController.LoadInfo(tile);
                            _isOnTile = false;
                        }
                    }
                }

            }

        }
    }
}