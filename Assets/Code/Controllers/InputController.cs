using Controllers.BaseUnit;
using UnityEngine;
using UnityEngine.EventSystems;
using BuildingSystem;
using Code.TileSystem;
using Controllers.BuildBuildingsUI;
using Controllers.OutPost;
using Views.BuildBuildingsUI;

namespace Controllers
{
    public class InputController : IOnController, IOnUpdate
    {
        // private BuildingResursesUIController _rescontoller;
        private TileUIController _tileUIController;

        public InputController(TileUIController tileUIController)
        {
            // _rescontoller = rescontoller;
            _tileUIController = tileUIController;
        }


        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _tileUIController.OpenMenu(false);
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
                        _tileUIController.OpenMenu(true);
                        _tileUIController.LoadInfo(tile);
                    }
                }

            }

        }
    }
}