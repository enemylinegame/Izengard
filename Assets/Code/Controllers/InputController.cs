using Controllers.BaseUnit;
using UnityEngine;
using UnityEngine.EventSystems;
using BuildingSystem;
using Code.TileSystem;
using Controllers.OutPost;
using Views.BuildBuildingsUI;

namespace Controllers
{
    public class InputController : IOnController, IOnUpdate
    {
        // private BuildingResursesUIController _rescontoller;
        private TileController _tileController;

        public InputController(TileController tileController)
        {
            // _rescontoller = rescontoller;
            _tileController = tileController;
        }


        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _tileController.BuildingsUIView.OpenMenu(false);
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
                        _tileController.BuildingsUIView.OpenMenu(true);
                        _tileController.LoadInfo(tile);
                    }
                }

            }

        }
    }
}