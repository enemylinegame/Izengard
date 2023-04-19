using UnityEngine;
using ResourceSystem;
using BuildingSystem;
using Code;
using Code.BuildingSystem;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using EquipmentSystem;
using UnityEngine.Serialization;
using Views.BaseUnit.UI;

public class Main : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private BuildingList _buildingList;
    [SerializeField] private TileList _tileList;
    [SerializeField] private GlobalResorceStock GlobalResStock;
    [SerializeField] private TowerShotConfig _towerShotConfig;
    [Header("UI")]
    [SerializeField] private RightUI _rightUI;
    [SerializeField] private LeftUI _leftUI;
    [SerializeField] private CenterUI _centerUI;
    [SerializeField] private BottonUI _bottonUI;
    [SerializeField] private TopResUiVew TopResUI;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private BuildingsUI buildingsUI;
    [SerializeField] private BaseCenterText _centerText;
    [SerializeField] private TileUIView tileUIView;
    [Header("Equip")]
    [SerializeField] private BuyItemScreenView _buyItemScreenView;
    [SerializeField] private HireSystemView _hireSystemView;
    [SerializeField] private EquipScreenView _equipScreenView;
    [Header("Other")]
    [SerializeField] private Transform _btnParents;
    [SerializeField] private Camera _screenCamera;
    
    private Controller _controllers;

    private void Start()
    {
        GlobalResStock.ResetGlobalRes();
        _controllers = new Controller();
        new GameInit(_controllers, _gameConfig, _rightUI,  _btnParents, _leftUI, _centerUI,_bottonUI ,
            buildingsUI, GlobalResStock, TopResUI, _buildingList, _endGameScreen, _towerShotConfig,
            _buyItemScreenView, _hireSystemView, _equipScreenView, _screenCamera, _centerText, tileUIView, _tileList);
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