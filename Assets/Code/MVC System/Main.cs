using UnityEngine;
using ResourceSystem;
using Code.QuickOutline.Scripts;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using EquipmentSystem;
using ResourceMarket;
using UnityEngine.Serialization;

public class Main : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private GlobalTileSettings _globalTileSettings;
    [SerializeField] private GlobalResourceData _globalResourceList;
    [SerializeField] private WorkersTeamConfig _workersTeamConfig;
    [SerializeField] private OutLineSettings _outLineSettings;
    [SerializeField] private TowerShotConfig _towerShotConfig;
    [SerializeField] private MarketDataConfig _marketDataConfig;

    [Header("UI")]
    [SerializeField] private RightUI _rightUI;
    [SerializeField] private CenterUI _centerUI;
    [SerializeField] private BottomUI _bottonUI;
    [SerializeField] private TopResUiVew _topUI;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private BuildingsUI buildingsUI;
    [SerializeField] private MarketView _marketUI;
    [SerializeField] private InGameMenuUI _inGameMenuUI;
    [Header("Equip")]
    [SerializeField] private BuyItemScreenView _buyItemScreenView;
    [SerializeField] private HireSystemView _hireSystemView;
    [SerializeField] private EquipScreenView _equipScreenView;

    [Header("Other")]
    [SerializeField] private Transform _btnParents;
    [SerializeField] private Camera _screenCamera;
    [SerializeField] private AudioSource _clickAudioSource;
    
    private Controller _controllers;

    private void Start()
    {
        _controllers = new Controller();

        new GameInit(_controllers, _gameConfig, _workersTeamConfig, 
            _rightUI,  _btnParents,_centerUI ,_bottonUI,
            _topUI, _endGameScreen, _towerShotConfig,
            _buyItemScreenView, _hireSystemView, _equipScreenView, 
            _screenCamera, _globalTileSettings, _globalResourceList, _outLineSettings,
            _marketDataConfig, _marketUI, _inGameMenuUI, _clickAudioSource);

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