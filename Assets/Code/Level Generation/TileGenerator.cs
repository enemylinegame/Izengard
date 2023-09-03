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


public class TileGenerator : IOnController, IOnStart, IOnLateUpdate, IDisposable
{
    public event Action<VoxelTile, TileModel> SpawnResources;
    public event Action<Dictionary<Vector2Int, VoxelTile>, ITileSetter, Transform, TileModel> SpawnTower;
    public Action OnCombatPhaseStart;
    
    public List<TileModel> Tiles;

    private readonly List<VoxelTile> _voxelTiles;
    private readonly RightPanelController _rightpanel;
    private readonly Dictionary<Vector2Int, VoxelTile> _spawnedTiles;
    private readonly GlobalTileSettings _tileSettings;
    private readonly IButtonsSetter _buttonsSetter;
    private ITileSetter _tileSetter;
    private int _numZone;
    
    public IReadOnlyDictionary<Vector2Int, VoxelTile> SpawnedTiles => _spawnedTiles;
    public Transform PointSpawnUnits;


    public TileGenerator(List<VoxelTile> tiles, ConfigsHolder configs, RightPanelController rightPanel)
    {
        _voxelTiles = tiles;
        _tileSettings = configs.GlobalTileSettings;
        _rightpanel = rightPanel;
        _spawnedTiles = new Dictionary<Vector2Int, VoxelTile>();
        Tiles = new List<TileModel>();
        
        _buttonsSetter = new ButtonsSetter(SpawnTile, rightPanel.GetButtonParents(), tiles[0].SizeTile, _spawnedTiles, configs.GameConfig.ButtonSetterView);
        rightPanel.StartSpawnTiles(configs.GameConfig);
        
    }
    
    private TileModel NewTile(TileView view)
    {
        if (Tiles.Exists(tile => view.ID != tile.ID) || Tiles.Count == 0)
        {
            TileModel model = new TileModel
            {
                DotSpawns = view.DotSpawns,
                ID = view.ID,
                TilePosition = view.transform.position
            };

            model.Init(_tileSettings);
            Tiles.Add(model);
            return model;
        }

        return null;
    }
        
    public TileModel LoadTile(TileView view)
    {
        var model = Tiles.Find(tile => view.ID == tile.ID);
        if (model == null) return null;

        return model;
    }

    public void OnStart()
    {
        _rightpanel.SubscribeTileSelButtons();
        _rightpanel.TileSelected += SelectFirstTile;
    }
    private void SpawnTile(TileSpawnInfo tileSpawnInfo)
    {
        _tileSetter.SetTile(tileSpawnInfo);
        _buttonsSetter.SetButtons(tileSpawnInfo.GridSpawnPosition);
        SetTileNumZone(tileSpawnInfo.GridSpawnPosition);
        TileModel model = NewTile(_spawnedTiles[tileSpawnInfo.GridSpawnPosition].TileView);
        SpawnResources?.Invoke(_spawnedTiles[tileSpawnInfo.GridSpawnPosition], model);
      
    }
    private void SelectFirstTile(int numTile)
    {
        _tileSetter = new TileSetter(_voxelTiles, _spawnedTiles, _voxelTiles[numTile], _tileSettings);
        _tileSetter.PlaceFirstTile(_voxelTiles[numTile]);
        _buttonsSetter.SetButtons(_tileSetter.FirstTileGridPosition);
        
        _rightpanel.TileSelected -= SelectFirstTile;
        _rightpanel.DeactivateTileSelButtons();

        TileModel model = NewTile(_spawnedTiles[_tileSetter.FirstTileGridPosition].TileView);
        SpawnTower?.Invoke(_spawnedTiles, _tileSetter, PointSpawnUnits, model);
        SetTileNumZone(_tileSetter.FirstTileGridPosition);
        SpawnResources?.Invoke(_spawnedTiles[_tileSetter.FirstTileGridPosition], model);
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
        _buttonsSetter?.Dispose();
        _rightpanel?.UnSubscribeTileSelButtons();
    }
}