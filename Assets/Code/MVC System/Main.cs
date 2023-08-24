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
    [SerializeField] private Canvas _canvas;
    [FormerlySerializedAs("_endGameScreen")] [SerializeField] private EndGameScreenPanel endGameScreenPanel;
    [FormerlySerializedAs("_inGameMenuUI")] [SerializeField] private InGameMenuPanel inGameMenuPanel;
    [Header("Equip")]

    [Header("Other")]
    [SerializeField] private Transform _btnParents;
    [SerializeField] private AudioSource _clickAudioSource;
    
    private Controller _controllers;

    private void Start()
    {
        _controllers = new Controller();

        new GameInit(_controllers, _gameConfig, _workersTeamConfig, _btnParents, endGameScreenPanel, 
            _towerShotConfig, _globalTileSettings, _globalResourceList, _outLineSettings,
            _marketDataConfig, inGameMenuPanel, _clickAudioSource, _canvas);

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