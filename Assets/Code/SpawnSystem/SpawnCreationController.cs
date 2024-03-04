using Abstraction;
using Code.SceneConfigs;
using Configs;
using System;
using System.Collections.Generic;
using UI;
using UnitSystem;
using UnitSystem.Enum;
using UnityEditor;
using UnityEngine;
using UserInputSystem;
using Object = UnityEngine.Object;

namespace SpawnSystem
{
    public class SpawnCreationController : IOnController, IDisposable
    {
        private readonly SpawnPanelUI _view;
        private readonly UnitSettingsPanel _unitSettingsPanel;

        private readonly SpawnerTypeSelectionPanel _typeSelectionPanel;

        private readonly SpawnerView _enemySpawners;
        private readonly SpawnerView _defenderSpawners;

        private readonly GameObject _spawnerPrefab;
        private readonly RayCastController _rayCastController;
        private readonly GameObject _plane;
        private readonly Grid _grid;

        private List<Spawner> _createdSpawnersCollection = new();

        private Spawner _buildingSpawner;
        private Spawner _selectedSpawner;
        private Vector2Int _mousePos;

        private int _spawnerCount;

        public Spawner SelectedSpawner => _selectedSpawner;

        public event Action<Spawner> OnSpawnerCreated;
        public event Action<Spawner> OnSpawnerRemoved;

        private List<IUnitData> _defenderUnitsData;
        private List<IUnitData> _enemyUnitsData;

        public SpawnCreationController(
            BattleUIController battleUIController,
            SceneObjectsHolder sceneObjects,
            GameObject spawnerPrefab,
            RayCastController rayCastController,
            ObjectsHolder objects,
            GameObject plane,
            Grid grid)
        {
            _view = battleUIController.View.SpawnPanel;
            _unitSettingsPanel = battleUIController.View.UnitSettingsPanel;

            _typeSelectionPanel = battleUIController.View.SpawnerTypeSelection;

            _enemySpawners = sceneObjects.EnemySpawner;
            _defenderSpawners = sceneObjects.DefendersSpawner;

            _defenderUnitsData = GetAvailableUnitData(_defenderSpawners.SpawnSettings); 
            _enemyUnitsData = GetAvailableUnitData(_enemySpawners.SpawnSettings);

            _spawnerPrefab = spawnerPrefab;

            _rayCastController = rayCastController;

            _plane = plane;
            _grid = grid;

            Subscribe();

            _view.UnselectAll();

            _typeSelectionPanel.Hide();

            _spawnerCount = 0;
        }

        private void Subscribe()
        {
            _view.OnSpawnerSelectAction += SelectSpawner;
            _view.OnCreateSpawnerClick += CreateSpawner;
            _view.OnRemoveSpawnerClick += RemoveSpawner;

            _rayCastController.RightClick += SelectSpawner;
            _rayCastController.LeftClick += RemoveSelection;

            _unitSettingsPanel.Parametrs.OnUnitTypeChange += UnitTypeChanged;
            _unitSettingsPanel.OnSaveUnitData += SaveUnitData;
            _unitSettingsPanel.OnRestoreUnitData += RestoreUnitData;
        }

        private void Unsubscribe()
        {
            _view.OnSpawnerSelectAction -= SelectSpawner;
            _view.OnCreateSpawnerClick -= CreateSpawner;
            _view.OnRemoveSpawnerClick -= RemoveSpawner;

            _rayCastController.RightClick -= SelectSpawner;
            _rayCastController.LeftClick -= RemoveSelection;

            _unitSettingsPanel.Parametrs.OnUnitTypeChange -= UnitTypeChanged;
            _unitSettingsPanel.OnSaveUnitData -= SaveUnitData;
            _unitSettingsPanel.OnRestoreUnitData -= RestoreUnitData;
        }


        private void SelectSpawner(string spawnerId)
        {

            if (spawnerId == null)
                return;

            UnselectCurrentSpawner();

            _selectedSpawner = _createdSpawnersCollection.Find(spw => spw.Id == spawnerId);

            if (_selectedSpawner == null)
                return;

            _selectedSpawner.Select();

            UpdateUnitSettingsPanel(_selectedSpawner);

            _view.SelectSpawner(spawnerId);
        }

        private void UpdateUnitSettingsPanel(Spawner selectedSpawner)
        {
            _unitSettingsPanel.Show();
            
            _unitSettingsPanel.SetFaction(selectedSpawner.FactionType);

            switch (selectedSpawner.FactionType)
            {
                case FactionType.Defender:
                    {
                        var availableUnitTypes = GetAvailableUnitTypes(_defenderSpawners.SpawnSettings);
                        _unitSettingsPanel.SetUnitTypes(availableUnitTypes);

                        _unitSettingsPanel.ChangeData(_defenderUnitsData[0]);
                        break;
                    }
                case FactionType.Enemy:
                    {
                        var availableUnitTypes = GetAvailableUnitTypes(_enemySpawners.SpawnSettings);
                        _unitSettingsPanel.SetUnitTypes(availableUnitTypes);

                        _unitSettingsPanel.ChangeData(_enemyUnitsData[0]);
                        break;
                    }
            }
        }

        private IList<UnitType> GetAvailableUnitTypes(SpawnSettings settings)
        {
            var result = new List<UnitType>();

            var unitsCreationData = settings.UnitsCreationData;
            
            for (int i = 0; i < unitsCreationData.Count; i++)
            {
                result.Add(unitsCreationData[i].Type);
            }

            return result;
        }

        private List<IUnitData> GetAvailableUnitData(SpawnSettings settings)
        {
            var result = new List<IUnitData>();

            var unitsCreationData = settings.UnitsCreationData;

            for (int i = 0; i < unitsCreationData.Count; i++)
            {
                result.Add(unitsCreationData[i].UnitSettings);
            }

            return result;
        }

        private void CreateSpawner()
        {
            _plane.SetActive(true);
            _spawnerCount++;

            _view.UnselectAll();

            _unitSettingsPanel.SetFaction(FactionType.None);
            _unitSettingsPanel.Hide();

            UnselectCurrentSpawner();

            CancelCurrentPlacement();

            var spawner = Object.Instantiate(_spawnerPrefab).GetComponent<Spawner>();

            spawner.Init(GUID.Generate().ToString(), $"Spawner[{_spawnerCount - 1}]");

            _buildingSpawner = spawner;

            InstallInCenterTile(_buildingSpawner);

            _buildingSpawner.Select();

            _buildingSpawner.ObjectBuild.transform.position = _plane.transform.position;

            _rayCastController.LeftClick += PlaceSpawner;
            _rayCastController.MousePosition += RayCastControllerOnMousePosition;
        }

        private void CancelCurrentPlacement()
        {
            if (_buildingSpawner == null) return;

            _buildingSpawner.Unselect();

            _rayCastController.LeftClick -= PlaceSpawner;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;

            _createdSpawnersCollection.Remove(_buildingSpawner);

            Object.Destroy(_buildingSpawner.ObjectBuild);

            _buildingSpawner = null;
        }

        private void InstallInCenterTile(Spawner spawner)
        {
            var size = spawner.Size;

            var sizeX = size.x / 2f;
            var sizeY = size.y / 2f;

            spawner.PositionBuild.position = new Vector3(sizeX, 0, sizeY);
            spawner.Collider.center = new Vector3(sizeX, .2f, sizeY);
            spawner.Collider.size = new Vector3(size.x - .5f, .2f, size.y - .5f);

            spawner.SpawnLocation.position = new Vector3(sizeX, 0, sizeY);
        }

        private void PlaceSpawner(string ID)
        {
            if (_buildingSpawner == null)
                return;
  
            _rayCastController.LeftClick -= PlaceSpawner;
            _rayCastController.MousePosition -= RayCastControllerOnMousePosition;
            
            _typeSelectionPanel.Enable(SpawnerFinalizePlacment);           
        }

        private void SpawnerFinalizePlacment(FactionType faction) 
        {
            _typeSelectionPanel.Disable();

            _plane.SetActive(false);

            _buildingSpawner.Unselect();

            _createdSpawnersCollection.Add(_buildingSpawner);
            _view.AddHUD(_buildingSpawner.Id, faction);

            _buildingSpawner.SetFaction(faction);

            switch (faction)
            {
                case FactionType.Enemy:
                    _buildingSpawner.ObjectBuild.transform.SetParent(_enemySpawners.SpawnersContainer);
                    break;
                case FactionType.Defender:
                    _buildingSpawner.ObjectBuild.transform.SetParent(_defenderSpawners.SpawnersContainer);
                    break;
            }

            OnSpawnerCreated?.Invoke(_buildingSpawner);

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

            _selectedSpawner.Unselect();

            _view.RemoveHUD(_selectedSpawner.Id);
            
            _unitSettingsPanel.SetFaction(FactionType.None);
            _unitSettingsPanel.Hide();

            _createdSpawnersCollection.Remove(_selectedSpawner);

            OnSpawnerRemoved?.Invoke(_selectedSpawner);

            Object.Destroy(_selectedSpawner.ObjectBuild);

            _selectedSpawner = null;
        }


        private void RemoveSelection(string spawnerId)
        {
            _view.UnselectAll();

            UnselectCurrentSpawner();

            _unitSettingsPanel.SetFaction(FactionType.None);
            _unitSettingsPanel.Hide();
        }


        private void UnselectCurrentSpawner()
        {
            if (_selectedSpawner != null)
            {
                _selectedSpawner.Unselect();

                _selectedSpawner = null;
            }
        }

        private void UnitTypeChanged(int index)
        {
            if (_selectedSpawner == null)
                return;

            switch (_selectedSpawner.FactionType)
            {
                case FactionType.Defender:
                    {
                        _unitSettingsPanel.ChangeData(_defenderUnitsData[index]);
                        break;
                    }
                case FactionType.Enemy:
                    {
                        _unitSettingsPanel.ChangeData(_enemyUnitsData[index]);
                        break;
                    }
            }
        }


        private void SaveUnitData(IUnitData unitData)
        {
            if (_selectedSpawner == null)
                return;

            switch (_selectedSpawner.FactionType)
            {
                case FactionType.Defender:
                    {
                        var unitIndex = _defenderUnitsData.FindIndex(u => u.Type == unitData.Type);
                        _defenderUnitsData[unitIndex] = unitData;
                        break;
                    }
                case FactionType.Enemy:
                    {
                        var unitIndex = _enemyUnitsData.FindIndex(u => u.Type == unitData.Type);
                        _enemyUnitsData[unitIndex] = unitData;
                        break;
                    }
            }
        }

        private void RestoreUnitData(UnitType unitType)
        {
            if (_selectedSpawner == null)
                return;

            IUnitData unitData = null;

            switch (_selectedSpawner.FactionType)
            {
                case FactionType.Defender:
                    {
                        var unitsDataCollection = GetAvailableUnitData(_defenderSpawners.SpawnSettings);
                        
                        unitData = unitsDataCollection.Find(u => u.Type == unitType);

                        var restoreIndex = _defenderUnitsData.FindIndex(u => u.Type == unitType);
                        
                        _defenderUnitsData[restoreIndex] = unitData;

                        break;
                    }
                case FactionType.Enemy:
                    {
                        var unitsDataCollection = GetAvailableUnitData(_enemySpawners.SpawnSettings);

                        unitData = unitsDataCollection.Find(u => u.Type == unitType);

                        var restoreIndex = _enemyUnitsData.FindIndex(u => u.Type == unitType);

                        _enemyUnitsData[restoreIndex] = unitData;

                        break;
                    }
            }

            _unitSettingsPanel.ChangeData(unitData);
        }


        public void Reset()
        {
            _view.ClearHUD();

            _unitSettingsPanel.SetFaction(FactionType.None);
            _unitSettingsPanel.Hide();

            if (_buildingSpawner != null)
            {
                OnSpawnerRemoved?.Invoke(_buildingSpawner);

                Object.Destroy(_buildingSpawner.ObjectBuild);
                _buildingSpawner = null;
            }

            if (_selectedSpawner != null)
            {
                OnSpawnerRemoved?.Invoke(_selectedSpawner);

                Object.Destroy(_selectedSpawner.ObjectBuild);
                _selectedSpawner = null;
            }

            for (int i =0; i< _createdSpawnersCollection.Count; i++)
            {
                var spawner = _createdSpawnersCollection[i];
                OnSpawnerRemoved?.Invoke(spawner);
                Object.Destroy(spawner.ObjectBuild);
            }

            _createdSpawnersCollection.Clear();
        }


        #region IDisposable

        private bool _isDisposed = false;
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            Unsubscribe();
        }

        #endregion
    }
}
