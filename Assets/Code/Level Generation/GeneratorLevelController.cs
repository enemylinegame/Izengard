using LevelGenerator;
using LevelGenerator.Interfaces;
using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using UnityEngine;
using Object = UnityEngine.Object;


public class GeneratorLevelController : IOnController, IOnStart, IOnLateUpdate
{
    public event Action<VoxelTile> SpawnResources;
    public event Action<Dictionary<Vector2Int, VoxelTile>, ITileSetter, Transform> SpawnTower;
    public Action OnCombatPhaseStart;
    public Damageable MainBuilding { get; private set; }

    private readonly List<VoxelTile> _voxelTiles;
    private readonly GameConfig _gameConfig;
    private readonly RightUI _rightUI;
    private readonly BtnUIController _btnUIController;
    private readonly BuildingFactory _buildingFactory;
    private readonly Dictionary<Vector2Int, VoxelTile> _spawnedTiles = new Dictionary<Vector2Int, VoxelTile>();
    private TileView _tileView;
    private GlobalTileSettings _tileSettings;
    public IReadOnlyDictionary<Vector2Int, VoxelTile> SpawnedTiles => _spawnedTiles;
    private ITileSetter _tileSetter;
    private readonly IButtonsSetter _buttonsSetter;
    private int _numZone;
    public TowerShotBehavior TowerShot;
    public Transform PointSpawnUnits;


    public GeneratorLevelController(List<VoxelTile> tiles, GameConfig gameConfig, BtnUIController btnUIController, 
        Transform btnParents, UIController uiController, GlobalTileSettings tileSettings)
    {
        _voxelTiles = tiles;
        _gameConfig = gameConfig;
        _btnUIController = btnUIController;
        _tileSettings = tileSettings;
        _rightUI = uiController.RightUI;
        
        _buttonsSetter = new ButtonsSetter(SpawnTile, btnParents, tiles[0].SizeTile, _spawnedTiles, gameConfig.ButtonSetterView);
    }

    public void OnStart()
    {
        _btnUIController.TileSelected += SelectFirstTile;
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
        
        _btnUIController.TileSelected -= SelectFirstTile;
        _rightUI.ButtonSelectTileFirst.gameObject.SetActive(false);
        _rightUI.ButtonSelectTileSecond.gameObject.SetActive(false);
        _rightUI.ButtonSelectTileThird.gameObject.SetActive(false);
        // _rightUI.ButtonHireUnits.gameObject.SetActive(true);

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
        // if (MainBuilding != null)
        // {
        //     _tileView.TileModel.MaxHealth = (int)MainBuilding.MaxHealth;
        //     _tileView.TileModel.CurrentHealth = (int)MainBuilding.CurrentHealth;
        // }
        //if(TowerShot.GetComponent<Damageable>().Health <= 0) 
    }
}