using Controllers.OutPost;
using UnityEngine;
using UnityEngine.AI;
using ResurseSystem;
using BuildingSystem;
using Code.TowerShot;
using Views.BaseUnit.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private RightUI _rightUI;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private Transform _btnParents;
    [SerializeField] private LeftUI _leftUI;
    [SerializeField] private LayerMask _layerMaskTiles;
    [SerializeField] private UnitUISpawnerTest _unitUISpawnerTest;   
    [SerializeField] private BuildingsUI buildingsUI;
    [SerializeField] private GlobalResurseStock GlobalResStock;
    [SerializeField] private TopResUiVew TopResUI;
    [SerializeField] private GlobalBuildingsModels _GlobalBuildingModel;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private TowerShotConfig _towerShotConfig;
    private Controller _controllers;

    private void Start()
    {
        GlobalResStock.ResetGlobalRes();
        _controllers = new Controller();
        new GameInit(_controllers, _gameConfig, _rightUI, _navMeshSurface, _btnParents, _leftUI, _layerMaskTiles,
            _unitUISpawnerTest, buildingsUI, GlobalResStock, TopResUI, _GlobalBuildingModel, _endGameScreen, _towerShotConfig);
        _controllers.OnStart();
    }

    private void Update()
    {
        _controllers.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _controllers.OnFixedUpdate(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        _controllers.OnLateUpdate(Time.deltaTime);
    }    
}