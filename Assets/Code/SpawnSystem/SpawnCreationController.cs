using Code.SceneConfigs;
using Configs;
using NewBuildingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnitSystem.Enum;
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
        private readonly SpawnerTypeSelectionPanel _typeSelectionPanel;

        private readonly SpawnerView _enemySpawners;
        private readonly SpawnerView _defenderSpawners;

        private readonly GameObject _spawnerPrefab;
        private readonly RayCastController _rayCastController;
        private readonly Material _previewMaterial;
        private readonly GameObject _plane;
        private readonly Grid _grid;

        private List<Spawner> _createdSpawnersCollection = new();

        private List<Material> _oldMaterials = new();
        private Spawner _buildingSpawner;
        private Spawner _selectedSpawner;
        private Vector2Int _mousePos;

        private int _spawnerCount;

        public event Action<Spawner> OnSpawnerCreated;
        public event Action<Spawner> OnSpawnerRemoved;

        public SpawnCreationController(
            SceneObjectsHolder sceneObjects,
            GameObject spawnerPrefab,
            RayCastController rayCastController,
            ObjectsHolder objects,
            GameObject plane,
            Grid grid)
        {
            _checkForBorders = new CheckForBorders(plane);

            _spawnUI = sceneObjects.BattleUI.SpawnPanel;
            _typeSelectionPanel = sceneObjects.BattleUI.SpawnerTypeSelection;

            _enemySpawners = sceneObjects.EnemySpawner;
            _defenderSpawners = sceneObjects.DefendersSpawner;

            _spawnerPrefab = spawnerPrefab;

            _rayCastController = rayCastController;

            _previewMaterial = Object.Instantiate(objects.PreveiwMaterial);

            _plane = plane;
            _grid = grid;

            _spawnUI.OnSpawnerSelectAction += SelectSpawner;
            _spawnUI.OnCreateSpawnerClick += CreateSpawner;
            _spawnUI.OnRemoveSpawnerClick += RemoveSpawner;

            _typeSelectionPanel.Hide();

            _spawnerCount = 0;
        }

        private void SelectSpawner(string spawnerId)
        {
            if (_selectedSpawner != null)
            {
                ChangeMaterial(_selectedSpawner, false);
            }

            _selectedSpawner = _createdSpawnersCollection.Find(spw => spw.ID == spawnerId);

            ChangeMaterial(_selectedSpawner);
        }

        private void CreateSpawner()
        {
            _plane.SetActive(true);
            _spawnerCount++;

            if (_selectedSpawner != null)
            {
                ChangeMaterial(_selectedSpawner, false);
            }

            CancelCurrentPlacement();

            var spawner = Object.Instantiate(_spawnerPrefab).GetComponent<Spawner>();

            spawner.ID = GUID.Generate().ToString();
            spawner.BuildingsType = EnumBuildings.None;
            spawner.Name = $"Spawner[{_spawnerCount - 1}]";
            spawner.Size = new Vector2Int(1, 1);

            _buildingSpawner = spawner;

            InstallInCenterTile(_buildingSpawner);
            ChangeMaterial(_buildingSpawner);

            _buildingSpawner.OnTriggered += _checkForBorders.CheckPlaneForBuilding;
            _buildingSpawner.ObjectBuild.transform.position = _plane.transform.position;

            _rayCastController.LeftClick += PlaceSpawner;
            _rayCastController.MousePosition += RayCastControllerOnMousePosition;
        }


        private void CancelCurrentPlacement()
        {
            if (_buildingSpawner == null) return;

            ChangeMaterial(_buildingSpawner, false);

            _buildingSpawner.OnTriggered -= _checkForBorders.CheckPlaneForBuilding;

            _rayCastController.LeftClick -= PlaceSpawner;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;

            _createdSpawnersCollection.Remove(_buildingSpawner);

            Object.Destroy(_buildingSpawner.ObjectBuild);

            _buildingSpawner = null;
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

            spawner.SpawnLocation.position = new Vector3(sizeX, 0, sizeY);
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
            if (_buildingSpawner == null)
                return;
  
            _rayCastController.LeftClick -= PlaceSpawner;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;
            
            _typeSelectionPanel.Enable(SpawnerFinalizePlacment);           
        }

        private void SpawnerFinalizePlacment(UnitFactionType faction) 
        {
            _typeSelectionPanel.Disable();

            _plane.SetActive(false);

            ChangeMaterial(_buildingSpawner, false);

            _createdSpawnersCollection.Add(_buildingSpawner);
            _spawnUI.AddHUD(_buildingSpawner.ID, faction);

            _buildingSpawner.SetFaction(faction);

            switch (faction)
            {
                case UnitFactionType.Enemy:
                    _buildingSpawner.ObjectBuild.transform.SetParent(_enemySpawners.SpawnersContainer);
                    break;
                case UnitFactionType.Defender:
                    _buildingSpawner.ObjectBuild.transform.SetParent(_defenderSpawners.SpawnersContainer);
                    break;
            }

            OnSpawnerCreated?.Invoke(_buildingSpawner);

            _buildingSpawner.OnTriggered -= _checkForBorders.CheckPlaneForBuilding;

            _buildingSpawner = null;
        }

        private void RayCastControllerOnMousePosition(Vector3 vector3)
        {
            Vector3Int gridPosCell = _grid.WorldToCell(vector3);

            if (gridPosCell.z != 0)
                return;

            var gridPos = _grid.CellToWorld(gridPosCell);

            _mousePos.x = Mathf.RoundToInt(gridPos.x);
            _mousePos.y = Mathf.RoundToInt(gridPos.z);

            _buildingSpawner.ObjectBuild.transform.position = gridPos;
        }


        private void RemoveSpawner()
        {
            if (_selectedSpawner == null)
                return;

            ChangeMaterial(_selectedSpawner, false);

            _spawnUI.RemoveHUD(_selectedSpawner.ID);

            _createdSpawnersCollection.Remove(_selectedSpawner);

            OnSpawnerRemoved?.Invoke(_selectedSpawner);

            Object.Destroy(_selectedSpawner.ObjectBuild);

            _selectedSpawner = null;
        }
    }
}
