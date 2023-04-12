using LevelGenerator;
using LevelGenerator.Interfaces;
using System;
using System.Collections.Generic;
using Code.TowerShot;
using UnityEngine;
using UnityEngine.AI;


public class GeneratorLevelController : IOnController, IOnStart, IOnLateUpdate
{
    public event Action<VoxelTile> SpawnResources;
    public Damageable MainBuilding { get; private set; }

    private readonly List<VoxelTile> _voxelTiles;
    private readonly GameConfig _gameConfig;
    private readonly RightUI _rightUI;
    private readonly NavMeshSurface _navMesh;
    private readonly BtnUIController _btnUIController;
    private readonly Dictionary<Vector2Int, VoxelTile> _spawnedTiles = new Dictionary<Vector2Int, VoxelTile>();
    public IReadOnlyDictionary<Vector2Int, VoxelTile> SpawnedTiles => _spawnedTiles;
    private ITileSetter _tileSetter;
    private readonly IButtonsSetter _buttonsSetter;
    private int _numZone;
    public TowerShotBehavior TowerShot;


    public GeneratorLevelController(List<VoxelTile> tiles, GameConfig gameConfig, RightUI rightUI,
        BtnUIController btnUIController, Transform btnParents, NavMeshSurface navMesh)
    {
        _voxelTiles = tiles;
        _gameConfig = gameConfig;
        _rightUI = rightUI;
        _btnUIController = btnUIController;
        _navMesh = navMesh;

        _buttonsSetter = new ButtonsSetter(SpawnTile, btnParents, tiles[0].SizeTile, _spawnedTiles, gameConfig.ButtonSpawn);
    }

    public void OnStart()
    {
        _btnUIController.TileSelected += SelectFirstTile;
    }
    
    private void SpawnTile(TileSpawnInfo tileSpawnInfo)
    {
        _tileSetter.SetTile(tileSpawnInfo);
        _buttonsSetter.SetButtons(tileSpawnInfo.GridSpawnPosition);

        SetTileNumZone(tileSpawnInfo.GridSpawnPosition);

        SpawnResources?.Invoke(_spawnedTiles[tileSpawnInfo.GridSpawnPosition]);
        _navMesh.BuildNavMesh();
    }

    private void SelectFirstTile(int numTile)
    {
        _tileSetter = new TileSetter(_voxelTiles, _spawnedTiles, _voxelTiles[numTile]);
        _buttonsSetter.SetButtons(_tileSetter.FirstTileGridPosition);
        
        _btnUIController.TileSelected -= SelectFirstTile;
        _rightUI.gameObject.SetActive(false);

        PlaceMainTower();
        SetTileNumZone(_tileSetter.FirstTileGridPosition);
        SpawnResources?.Invoke(_spawnedTiles[_tileSetter.FirstTileGridPosition]);
    }

    private void SetTileNumZone(Vector2Int tileGridPosition)
    {
        _numZone++;
        _spawnedTiles[tileGridPosition].NumZone = _numZone;
    }

    private void PlaceMainTower()
    {
        var firstTile = _spawnedTiles[_tileSetter.FirstTileGridPosition];
        var mainBuilding = UnityEngine.Object.Instantiate(_gameConfig.MainTower, firstTile.transform.position, Quaternion.identity);
        MainBuilding = mainBuilding.GetComponent<Damageable>();
        if(mainBuilding != null)TowerShot = mainBuilding.GetComponentInChildren<TowerShotBehavior>();
        MainBuilding.Init(1000);
    }

    public void OnLateUpdate(float deltaTime)
    {
        _buttonsSetter.OnLateUpdate(deltaTime);
    }
}