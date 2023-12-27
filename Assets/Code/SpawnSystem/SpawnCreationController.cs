using Configs;
using NewBuildingSystem;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEditor;
using UnityEngine;
using UserInputSystem;
using Object = UnityEngine.Object;

namespace SpawnSystem
{
    public class SpawnCreationController : IOnController
    {
        private readonly CheckForBorders _checkForBorders;

        private readonly SpawnPanelUI _spawnUI;
        private readonly GameObject _spawnerPrefab;
        private readonly RayCastController _rayCastController;
        private readonly Material _previewMaterial;
        private readonly GameObject _plane;
        private readonly Grid _grid;

        private List<Material> _oldMaterials = new();
        private Spawner _selectedSpawner;
        private Vector2Int _mousePos;

        private int _spawnerCount;

        public SpawnCreationController(
            SpawnPanelUI spawnUI,
            GameObject spawnerPrefab,
            RayCastController rayCastController,
            ObjectsHolder objects,
            GameObject plane, 
            Grid grid)
        {
            _checkForBorders = new CheckForBorders(plane);

            _spawnUI = spawnUI;
            _spawnerPrefab = spawnerPrefab;
            
            _rayCastController = rayCastController;

            _previewMaterial = Object.Instantiate(objects.PreveiwMaterial);

            _plane = plane;
            _grid = grid;

            _spawnUI.OnCreateSpawnerClick += CreateSpawner;

            _spawnerCount = 0;
        }


        private void CreateSpawner()
        {
            _plane.SetActive(true);
            _spawnerCount++;

            CancelCurrentPlacement();

            _spawnUI.AddHUD(_spawnerCount);

            var spawner = Object.Instantiate(_spawnerPrefab).GetComponent<Spawner>();

            _selectedSpawner = spawner;
            _selectedSpawner.BuildingsType = EnumBuildings.None;
            _selectedSpawner.Name = $"Spawner[{_spawnerCount - 1}]";
            _selectedSpawner.Size = new Vector2Int(1, 1);

            InstallInCenterTile(_selectedSpawner);

            _selectedSpawner.OnTriggered += _checkForBorders.CheckPlaneForBuilding;

            _selectedSpawner.ObjectBuild.transform.position = _plane.transform.position;

            ChangeMaterial(_selectedSpawner);

            _rayCastController.LeftClick += PlaceSpawner;
            _rayCastController.MousePosition += RayCastControllerOnMousePosition;
        }


        private void CancelCurrentPlacement()
        {
            if (_selectedSpawner == null) return;
            
            ChangeMaterial(_selectedSpawner, false);

            _selectedSpawner.OnTriggered -= _checkForBorders.CheckPlaneForBuilding;

            _rayCastController.LeftClick -= PlaceSpawner;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;
      
            Object.Destroy(_selectedSpawner.ObjectBuild);
            _selectedSpawner = null;
        }

        private void InstallInCenterTile(Spawner spawner)
        {
            var size = spawner.Size;

            spawner.Size = size;
            var sizeX = size.x / 2f;
            var sizeY = size.y / 2f;

            spawner.PositionBuild.position = new Vector3(sizeX, 0, sizeY);
            spawner.Collider.center = new Vector3(sizeX, .2f, sizeY);
            spawner.Collider.size = new Vector3(size.x - .5f, .2f, size.y - .5f);
        }

        private void ChangeMaterial(Spawner spawner, bool mode = true)
        {
            var materials = spawner.BuildingRenderer.materials.ToList();
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
            spawner.BuildingRenderer.materials = materials.ToArray();
        }


        private void PlaceSpawner(string ID)
        {
            if (_selectedSpawner == null) 
                return;

            _plane.SetActive(false);

            ChangeMaterial(_selectedSpawner, false);
            _selectedSpawner.ID = GUID.Generate().ToString();

            _selectedSpawner.OnTriggered -= _checkForBorders.CheckPlaneForBuilding;

            _rayCastController.LeftClick -= PlaceSpawner;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;
         
            _selectedSpawner = null;
        }

        private void RayCastControllerOnMousePosition(Vector3 vector3)
{
            Vector3Int gridPosCell = _grid.WorldToCell(vector3);
            
            if (gridPosCell.z != 0) 
                return;
            
            var gridPos = _grid.CellToWorld(gridPosCell);

            _mousePos.x = Mathf.RoundToInt(gridPos.x);
            _mousePos.y = Mathf.RoundToInt(gridPos.z);

            _selectedSpawner.ObjectBuild.transform.position = gridPos;
        }
    }
}
