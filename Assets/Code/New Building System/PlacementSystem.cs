using System.Collections.Generic;
using System.Linq;
using Configs;
using Unity.VisualScripting;
using UnityEngine;
using UserInputSystem;

namespace NewBuildingSystem
{
    public class PlacementSystem
    {
        private readonly ObjectsHolder _objects;
        private readonly RayCastController _rayCastController;
        private readonly Grid _grid;
        private readonly BuildingDataBase _dataBase;
        private readonly GameObject _plane;

        private int _selectedObjectID = -1;
        private BuildingView SelectedBuild;
        private List<Material> OldMaterials = new();
        private Material _previewMaterial;

        public PlacementSystem(ObjectsHolder objects, RayCastController rayCastController, Grid grid, BuildingDataBase dataBase, GameObject plane)
        {
            _objects = objects;
            _rayCastController = rayCastController;
            _grid = grid;
            _dataBase = dataBase;
            _plane = plane;
            _previewMaterial = Object.Instantiate(objects.PreveiwMaterial);
            
            StopPlacement();
            rayCastController.KeyDownOne += () => StartPlacement(1);
        }
        
        private void StartPlacement(int ID)
        {
            StopPlacement();
            _selectedObjectID = _dataBase.ObjectsData.FindIndex(data => data.ID == ID);
            if(_selectedObjectID < 0) return;
            _plane.SetActive(true);
            PlaceStructure();
            _rayCastController.LeftClick += StopPlacement;
        }
        private void PlaceStructure()
        {
            if (!_rayCastController.IsPointerOverUI())
            {
                BuildingView build = Object.Instantiate(_dataBase.ObjectsData[_selectedObjectID].Prefab.GetComponent<BuildingView>());
                build.SearchCenterTile((Vector3Int)_dataBase.ObjectsData[_selectedObjectID].Size);
                build.CreateIndicators(_dataBase.ObjectsData[_selectedObjectID].Size, _objects.CellIndicator);
                var materials = build.BuildingRenderer.materials.ToList();
                OldMaterials.AddRange(materials);
                materials.Clear();
                materials.Add(_previewMaterial);
                build.BuildingRenderer.materials = materials.ToArray();
                SelectedBuild = build;
                _rayCastController.MousePosition += RayCastControllerOnMousePosition;
               
            }
        }
        private void StopPlacement()
        {
            _selectedObjectID = -1;
            _plane.SetActive(false);
            if (SelectedBuild != null)
            {
                var materials = SelectedBuild.BuildingRenderer.materials.ToList();
                materials.Clear();
                materials.AddRange(OldMaterials);
                SelectedBuild.BuildingRenderer.materials = materials.ToArray();
            }
            _rayCastController.LeftClick -= StopPlacement;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;
            SelectedBuild = null;
        }

        private void RayCastControllerOnMousePosition(Vector3 vector3)
        {
            Vector3Int gridPos = _grid.WorldToCell(vector3);
            if(gridPos.z != 0) return;
            SelectedBuild.transform.position = _grid.CellToWorld(gridPos);
        }
    }
}