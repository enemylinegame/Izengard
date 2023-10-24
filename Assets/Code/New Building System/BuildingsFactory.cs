using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private Color _notConstructed = new(1, 0, 0 , 0.39f);
        private Color _Constructed = new(1, 1, 1 , 0.39f);
        
        private BuildingView _flyingBuilding;
        private Vector2Int _mousePos;
        
        public event Action<BuildingView> OnConstructed;
        public event Action<BuildingView> OnDestroyed;
        
        private readonly List<BuildingView> _buildingsConstructed;
        private readonly RayCastController _rayCastController;
        private readonly BuildingsSettingsSO _dataBase;
        private readonly GameObject _buildingsHolder;
        private readonly BuildingView[,] _buildings;
        private readonly Material _previewMaterial;
        private readonly ObjectsHolder _objects;
        private readonly GameObject _plane;
        private readonly Grid _grid;

        private bool _isBuild = true;
        public BuildingsFactory(BuildingsSettingsSO dataBase, RayCastController rayCastController, ObjectsHolder objects, 
            GameObject plane, Grid grid, GameObject buildingsHolder)
        {
            _dataBase = dataBase;
            _rayCastController = rayCastController;
            _objects = objects;
            _plane = plane;
            _grid = grid;
            _buildingsHolder = buildingsHolder;
            _buildingsConstructed = new ListOfBuildingsConstructed().ConstructedBuildings;
            
            _previewMaterial = Object.Instantiate(objects.PreveiwMaterial);
            
            var scale = _plane.transform.localScale;
            
            var x = Mathf.RoundToInt(TheRestOfTheSpaceMap().x  + scale.x * 10);
            var y = Mathf.RoundToInt(TheRestOfTheSpaceMap().y + scale.z * 10);
            _buildings = new BuildingView[x, y];

            rayCastController.KeyDownOne += () => PlaceBuilding(1);
            _rayCastController.RightClick += SelectBuild;
            PlaceFlyingBuilding();
        }
        
        private void PlaceBuilding(int ID)
        {
            var index = _dataBase.ObjectsData.FindIndex(data => data.ID == ID);
            
            if(index < 0) return;
            PlaceStructure(index);
            _plane.SetActive(true);
            _rayCastController.LeftClick += PlaceFlyingBuilding;
        }
        private void PlaceStructure(int ID)
        {
            var build = Object.Instantiate(_dataBase.ObjectsData[ID].Prefab.GetComponent<BuildingView>(), _buildingsHolder.transform);

            var size = _dataBase.ObjectsData[ID].Size;
            
            build.InstallInCenterTile(size);
            build.CreateIndicators(size, _objects.CellIndicator);
                
            _flyingBuilding = build;
            
            ChangeMaterial(true);
                
            _rayCastController.MousePosition += RayCastControllerOnMousePosition;
            //_selectedBuild.OnTriggered += _checkingForBorders.CheckingForBordersOnBuildingColliders;
            _flyingBuilding.transform.position = _plane.transform.position;
            OnConstructed?.Invoke(build);
        }
        
        private void PlaceFlyingBuilding()
        {
            if(!_isBuild || _flyingBuilding == null) return;
            
            for (int x = 0; x < _flyingBuilding.Size.x; x++)
            {
                for (int y = 0; y < _flyingBuilding.Size.y; y++)
                {
                    _buildings[_mousePos.x + x, _mousePos.y + y] = _flyingBuilding;
                }
            }
            
            ChangeMaterial(false);
            _flyingBuilding.ID = GUID.Generate().ToString();
            //_buildingsConstructed.Add(_flyingBuilding);
                
            //_selectedBuild.OnTriggered -= _checkingForBorders.CheckingForBordersOnBuildingColliders;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;
            _rayCastController.LeftClick -= PlaceFlyingBuilding;
            _flyingBuilding = null;
            
            _plane.SetActive(false);
        }

        public void CanceledPlacement()
        {
            ChangeMaterial(false);
            
            Object.Destroy(_flyingBuilding.gameObject);
            _flyingBuilding = null;
            
            _plane.SetActive(false);
        }
        private void DestroyBuilding()
        {
            _buildingsConstructed.Remove(_flyingBuilding);
            
            for (int x = 0; x < _flyingBuilding.Size.x; x++)
            {
                for (int y = 0; y < _flyingBuilding.Size.y; y++)
                {
                    _buildings[_mousePos.x + x, _mousePos.y + y] = null;
                }
            }

            _rayCastController.Delete -= DestroyBuilding;
            _rayCastController.RightClick += SelectBuild;
            OnDestroyed?.Invoke(_flyingBuilding);
            
            Object.Destroy(_flyingBuilding.gameObject);
            
            _flyingBuilding = null;
        }

        private void SelectBuild(BuildingView build)
        {
            _plane.SetActive(true);
            
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
        private bool IsPlaceTakenOnMap()
        {
            var scale = _plane.transform.localScale;
            
            if (_mousePos.x - TheRestOfTheSpaceMap().x < 0 || 
                _mousePos.x - TheRestOfTheSpaceMap().x> scale.x * 10f - _flyingBuilding.Size.x) return false;
            
            if (_mousePos.y - TheRestOfTheSpaceMap().y< 0 ||
                _mousePos.y - TheRestOfTheSpaceMap().y> scale.z * 10f - _flyingBuilding.Size.y) return false;

            return true;
        }
        
        private bool IsPlaceTaken()
        {
            for (int x = 0; x < _flyingBuilding.Size.x; x++)
            {
                for (int y = 0; y < _flyingBuilding.Size.y; y++)
                {
                    if (_buildings[_mousePos.x + x, _mousePos.y + y] != null) return false;
                        
                }
            }
            return true;
        }

        private Vector2 TheRestOfTheSpaceMap()
        {
            var position = _plane.transform.position;
            var scale = _plane.transform.localScale;
            
            var MaPositionX = Mathf.RoundToInt(position.x - (scale.x * 10f) / 2f);
            var MaPositionY = Mathf.RoundToInt(position.z - (scale.z * 10f) / 2f);

            return new Vector2(MaPositionX, MaPositionY);
        }
        private void RayCastControllerOnMousePosition(Vector3 vector3)
        {
            Vector3Int gridPosCell = _grid.WorldToCell(vector3);
            if(gridPosCell.z != 0) return;
            var gridPos = _grid.CellToWorld(gridPosCell);

            _mousePos.x = Mathf.RoundToInt(gridPos.x);
            _mousePos.y = Mathf.RoundToInt(gridPos.z);
            
            _flyingBuilding.transform.position = gridPos;
            
            if (IsPlaceTakenOnMap() && IsPlaceTaken())
            {
                _flyingBuilding.ChangePrewievColor(_Constructed);
                _isBuild = true;
            }
            else
            {
                _isBuild = false;
                _flyingBuilding.ChangePrewievColor(_notConstructed);
            }
                
        }

        
    }
}