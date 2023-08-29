using LevelGenerator;
using LevelGenerator.Interfaces;
using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.Game;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using UnityEngine;
using Object = UnityEngine.Object;


public class GeneratorLevelController : IOnController, IOnStart, IOnLateUpdate, IDisposable
{
    public event Action<VoxelTile> SpawnResources;
    public event Action<Dictionary<Vector2Int, VoxelTile>, ITileSetter, Transform> SpawnTower;
    public Action OnCombatPhaseStart;

    private readonly List<VoxelTile> _voxelTiles;
    private readonly GameConfig _gameConfig;
    private readonly RightPanelController _rightpanel;
    private readonly BuildingFactory _buildingFactory;
    private readonly Dictionary<Vector2Int, VoxelTile> _spawnedTiles;
    private readonly TileView _tileView;
    private readonly GlobalTileSettings _tileSettings;
    private readonly IButtonsSetter _buttonsSetter;
    private ITileSetter _tileSetter;
    private int _numZone;
    
    public IReadOnlyDictionary<Vector2Int, VoxelTile> SpawnedTiles => _spawnedTiles;
    public Transform PointSpawnUnits;


    public GeneratorLevelController(List<VoxelTile> tiles, ConfigsHolder configs, RightPanelController rightPanel)
    {
        _voxelTiles = tiles;
        _gameConfig = configs.GameConfig;
        _tileSettings = configs.GlobalTileSettings;
        _rightpanel = rightPanel;
        _spawnedTiles = new Dictionary<Vector2Int, VoxelTile>();
        
        _buttonsSetter = new ButtonsSetter(SpawnTile, rightPanel.GetButtonParents(), tiles[0].SizeTile, _spawnedTiles, _gameConfig.ButtonSetterView);
        rightPanel.StartSpawnTiles(configs.GameConfig);
    }

    public void OnStart()
    {
        _rightpanel.SubscribeTileSelButtons();
        _rightpanel.TileSelected += SelectFirstTile;
    }
    private void SpawnTile(TileSpawnInfo tileSpawnInfo)
    {
        _tileSetter.SetTile(tileSpawnInfo, _tileSettings);
        _buttonsSetter.SetButtons(tileSpawnInfo.GridSpawnPosition);
        SetTileNumZone(tileSpawnInfo.GridSpawnPosition);
        SpawnResources?.Invoke(_spawnedTiles[tileSpawnInfo.GridSpawnPosition]);
      
    }
    private void SelectFirstTile(int numTile)
    {
        _tileSetter = new TileSetter(_voxelTiles, _spawnedTiles, _voxelTiles[numTile], _tileSettings);
        _buttonsSetter.SetButtons(_tileSetter.FirstTileGridPosition);
        
        _rightpanel.TileSelected -= SelectFirstTile;
        _rightpanel.DeactivateTileSelButtons();

        SpawnTower?.Invoke(_spawnedTiles, _tileSetter, PointSpawnUnits);
        SetTileNumZone(_tileSetter.FirstTileGridPosition);
        SpawnResources?.Invoke(_spawnedTiles[_tileSetter.FirstTileGridPosition]);
    }
    private void SetTileNumZone(Vector2Int tileGridPosition)
    {
        _numZone++;
        _spawnedTiles[tileGridPosition].NumZone = _numZone;
    }
    public void OnLateUpdate(float deltaTime)
    {
        _buttonsSetter.OnLateUpdate(deltaTime);
    }

    public void Dispose()
    {
        _buildingFactory?.Dispose();
        _buttonsSetter?.Dispose();
        _rightpanel?.UnSubscribeTileSelButtons();
    }
}