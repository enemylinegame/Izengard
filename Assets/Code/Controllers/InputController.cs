using Controllers.BaseUnit;
using UnityEngine;
using UnityEngine.EventSystems;
using ResurseSystem;
using Views.Outpost;
using BuildingSystem;
using Controllers.OutPost;

namespace Controllers
{
    public class InputController : IOnController, IOnUpdate
    {
        private BaseUnitSpawner _spawner;
        private OutpostSpawner _outpostSpawner;
        private BuildingResursesUIController _rescontoller;   

        public InputController(BaseUnitSpawner baseUnitSpawner, BuildingResursesUIController rescontoller, OutpostSpawner spawner)
        {
            _spawner = baseUnitSpawner;
            _rescontoller = rescontoller;
            _outpostSpawner = spawner;
        }

        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var bitmask = ~(1 << 3);
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100, bitmask))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;
                    var outpost = hit.collider.gameObject.GetComponentInParent<OutpostUnitView>();
                    var currBuild = hit.collider.gameObject.GetComponentInParent<BuildingView>();
                    var currMine = hit.collider.gameObject.GetComponentInParent<Mineral>();
                    // if (_spawner.SpawnIsActiveIndex != -1)
                    // {
                    //     
                    // }
                    if (currBuild)
                    {                        
                        _rescontoller.SetActiveUI(currBuild); 
                    }
                    else
                    {
                        if (currMine)
                        {
                            _rescontoller.SetActiveUI(currMine);
                            
                                                       
                        }
                        else
                        {
                            _rescontoller.DisableMenu();
                        }
                    }

                    if (outpost)
                    {
                        _outpostSpawner.SpawnLogic(outpost);
                        _spawner.ShowMenu(outpost);
                    }
                    else
                    {
                        _spawner.UnShowMenu();
                    }
                }

            }

        }
    }
}