using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using Configs;
using UnityEditor;
using UnityEngine;
using UserInputSystem;
using Object = UnityEngine.Object;

namespace NewBuildingSystem
{
    public class BuildingsFactory
    {
        private List<Material> _oldMaterials = new();
        private List<Building> _buildings = new();
        
        private Building _flyingBuilding;
        private Vector2Int _mousePos;
        
        public event Action<Building> OnConstructed;
        public event Action<Building> OnDestroyed;
        
        private readonly BuildingsController _buildingsController;
        private readonly RayCastController _rayCastController;
        private readonly BuildingsSettings _dataBase;
        private readonly GameObject _buildingsHolder;
        private readonly CheckForBorders _checkForBorders;
        //private readonly Building[,] _buildings;
        private readonly Material _previewMaterial;
        private readonly GameObject _plane;
        private readonly Grid _grid;

        private bool _isBuild = true;
        public BuildingsFactory(BuildingsSettings dataBase, RayCastController rayCastController, ObjectsHolder objects, 
            GameObject plane, Grid grid, GameObject buildingsHolder, UIPanelsInitialization controller)
        {
            _checkForBorders = new CheckForBorders(plane);
            _buildingsController = new BuildingsController(dataBase, _buildings, objects.CellIndicator, controller.BuildingInfoPanelController, rayCastController);
            _dataBase = dataBase;
            _rayCastController = rayCastController;
            _plane = plane;
            _grid = grid;
            _buildingsHolder = buildingsHolder;
            
            _previewMaterial = Object.Instantiate(objects.PreveiwMaterial);

            /* var scale = _plane.transform.localScale;

             var x = Mathf.RoundToInt(_checkForBorders.TheRestOfTheSpaceMap().x  + scale.x * 10);
             var y = Mathf.RoundToInt(_checkForBorders.TheRestOfTheSpaceMap().y + scale.z * 10);
             //_buildings = new Building[x, y];*/

            for (int i = 0; i < dataBase.objectsData.Count; i++) 
            {
                var placeId = i + 1;
                controller.BuildingPanelController.NewBuilds(_dataBase.objectsData[i], () => PlaceBuilding(placeId));
            }
            rayCastController.KeyDownOne += () => PlaceBuilding(1);
            rayCastController.KeyDownTwo += () => PlaceBuilding(2);
            _rayCastController.RightClick += SelectBuild;
            PlaceFlyingBuilding(null);
        }
        
        private void PlaceBuilding(int ID)
        {
            CanceledPlacement();
            var index = _dataBase.objectsData.FindIndex(data => data.ID == ID);
            
            if(index < 0) return;
            PlaceStructure(index);
            _plane.SetActive(true);
            _rayCastController.LeftClick += PlaceFlyingBuilding;
        }

        private void PlaceStructure(int ID)
        {
            var build = Object.Instantiate(_dataBase.objectsData[ID].Prefab, _buildingsHolder.transform).GetComponent<Building>();
            
            _flyingBuilding = build;
            _flyingBuilding.BuildingsType = _dataBase.objectsData[ID].BuildingType;
            _flyingBuilding._name = _dataBase.objectsData[ID].Name;
            //_flyingBuilding.Image.sprite = _dataBase.objectsData[ID].Image;
            _flyingBuilding.MaxCountWorkers = _dataBase.objectsData[ID].MaxWorkers;
            _flyingBuilding.Size = _dataBase.objectsData[ID].Size;
            
            _buildingsController.Init(_flyingBuilding);
                
            
            ChangeMaterial(true);
                
            _rayCastController.MousePosition += RayCastControllerOnMousePosition;
            _flyingBuilding.OnTriggered += _checkForBorders.CheckPlaneForBuilding;
            _flyingBuilding.ObjectBuild.transform.position = _plane.transform.position;
            OnConstructed?.Invoke(build);
            
            _buildings.ForEach(build =>
            {
                if (build.BuildingsType == EnumBuildings.Res) build.ColliderArea.enabled = false;
            });
        }
        
        private void PlaceFlyingBuilding(string ID)
        {
            if(!_isBuild || _flyingBuilding == null) return;
            
            /*for (int x = 0; x < _flyingBuilding.size.x; x++)
            {
                for (int y = 0; y < _flyingBuilding.size.y; y++)
                {
                    _buildings[_mousePos.x + x, _mousePos.y + y] = _flyingBuilding;
                }
            }*/
            
            ChangeMaterial(false);
            
            _flyingBuilding.Init(GUID.Generate().ToString());

            _buildings.Add(_flyingBuilding);
                
            _flyingBuilding.OnTriggered -= _checkForBorders.CheckPlaneForBuilding;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;
            _rayCastController.LeftClick -= PlaceFlyingBuilding;
            _flyingBuilding = null;
            
            _plane.SetActive(false);
            
            _buildings.ForEach(build =>
            {
                if (build.BuildingsType == EnumBuildings.Res) build.ColliderArea.enabled = true;
            });
        }

        private void CanceledPlacement()
        {
            if(_flyingBuilding == null) return;
            ChangeMaterial(false);
            _flyingBuilding.OnTriggered -= _checkForBorders.CheckPlaneForBuilding;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;
            _rayCastController.LeftClick -= PlaceFlyingBuilding;
            
            Object.Destroy(_flyingBuilding.ObjectBuild);
            _flyingBuilding = null;
        }
        private void DestroyBuilding()
        {
            /*for (int x = 0; x < _flyingBuilding.size.x; x++)
            {
                for (int y = 0; y < _flyingBuilding.size.y; y++)
                {
                    _buildings[_mousePos.x + x, _mousePos.y + y] = null;
                }
            }*/

            _rayCastController.Delete -= DestroyBuilding;
            _rayCastController.RightClick += SelectBuild;
            OnDestroyed?.Invoke(_flyingBuilding);
            
            Object.Destroy(_flyingBuilding.ObjectBuild);
            
            _flyingBuilding = null;
        }

        private void SelectBuild(string ID)
        {
            _plane.SetActive(true);

            var build = _buildings.Find(x => ID == x.Id);
            if(build == null) return;
            
            var materials = build.BuildingRenderer.materials.ToList();
            materials.Clear();
            materials.Add(_previewMaterial);
            build.BuildingRenderer.materials = materials.ToArray();

            _flyingBuilding = build;
            _rayCastController.RightClick -= SelectBuild;
            _rayCastController.Delete += DestroyBuilding;
        }

        private void ChangeMaterial(bool mode)
        {
            var materials = _flyingBuilding.BuildingRenderer.materials.ToList();
            if (!mode)
            {
                materials.Clear();
                materials.AddRange(_oldMaterials);
            }
            else
            {
                _oldMaterials.Clear();
                _oldMaterials.AddRange(materials);
                materials.Clear();
                materials.Add(_previewMaterial);
            }
            _flyingBuilding.BuildingRenderer.materials = materials.ToArray();
        }
        
        private void RayCastControllerOnMousePosition(Vector3 vector3)
        {
            Vector3Int gridPosCell = _grid.WorldToCell(vector3);
            if(gridPosCell.z != 0) return;
            var gridPos = _grid.CellToWorld(gridPosCell);

            _mousePos.x = Mathf.RoundToInt(gridPos.x);
            _mousePos.y = Mathf.RoundToInt(gridPos.z);
            
            _flyingBuilding.ObjectBuild.transform.position = gridPos;
            
            _isBuild = _checkForBorders.IsPlaceTaken(_mousePos, _flyingBuilding);
                
        }

        
    }
}