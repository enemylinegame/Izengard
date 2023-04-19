using LevelGenerator;
using LevelGenerator.Interfaces;
using System;
using System.Collections.Generic;
using Code.TowerShot;
using Code.UI;
using UnityEngine;


public class GeneratorLevelController : IOnController, IOnStart, IOnLateUpdate
{
    public event Action<VoxelTile> SpawnResources;
    public Action OnCombatPhaseStart;
    public Damageable MainBuilding { get; private set; }

    private readonly List<VoxelTile> _voxelTiles;
    private readonly GameConfig _gameConfig;
    private readonly RightUI _rightUI;
    private readonly BottonUI _bottonUI;
    private readonly BtnUIController _btnUIController;
    private readonly Dictionary<Vector2Int, VoxelTile> _spawnedTiles = new Dictionary<Vector2Int, VoxelTile>();
    public IReadOnlyDictionary<Vector2Int, VoxelTile> SpawnedTiles => _spawnedTiles;
    private ITileSetter _tileSetter;
    private readonly IButtonsSetter _buttonsSetter;
    private int _numZone;
    public TowerShotBehavior TowerShot;
    public Damageable MainTowerDamageable;
    public Transform PointSpawnUnits;


    public GeneratorLevelController(List<VoxelTile> tiles, GameConfig gameConfig, RightUI rightUI,
        BtnUIController btnUIController, Transform btnParents,  BottonUI BottonUI)
    {
        _voxelTiles = tiles;
        _gameConfig = gameConfig;
        _rightUI = rightUI;
        _btnUIController = btnUIController;
        
        _bottonUI = BottonUI;
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
      
    }
    private void SelectFirstTile(int numTile)
    {
        _tileSetter = new TileSetter(_voxelTiles, _spawnedTiles, _voxelTiles[numTile], _gameConfig.TestBuilding);
        _buttonsSetter.SetButtons(_tileSetter.FirstTileGridPosition);
        
        _btnUIController.TileSelected -= SelectFirstTile;
        _rightUI.ButtonSelectTileFirst.gameObject.SetActive(false);
        _rightUI.ButtonSelectTileSecond.gameObject.SetActive(false);
        _rightUI.ButtonSelectTileThird.gameObject.SetActive(false);
        _rightUI.ButtonHireUnits.gameObject.SetActive(true);
        // _bottonUI.BuildingMenu.OpenMenuButton.gameObject.SetActive(true);

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
        PointSpawnUnits = mainBuilding.transform;
        MainBuilding = mainBuilding.GetComponent<Damageable>();
        if (mainBuilding != null)
        {
            TowerShot = mainBuilding.GetComponentInChildren<TowerShotBehavior>();
        }
        MainBuilding.Init(1000);
    }
    public void OnLateUpdate(float deltaTime)
    {
        _buttonsSetter.OnLateUpdate(deltaTime);
        //if(TowerShot.GetComponent<Damageable>().Health <= 0) 
    }
}