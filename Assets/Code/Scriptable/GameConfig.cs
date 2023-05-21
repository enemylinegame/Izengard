using Code.Scriptable;
using CombatSystem;
using UnityEngine;
using UnityEngine.UI;
using Wave;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _mapSizeX;
    [SerializeField] private int _mapSizeY;
    [SerializeField] private VoxelTile[] _tilePrefabs;
    [SerializeField] private GameObject _mainTower;
    [SerializeField] private GameObject _baseUnit;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private Building _buildFirst;
    [SerializeField] private Building _buildSecond;
    [SerializeField] private VoxelTile _firstTile;
    [SerializeField] private VoxelTile _secondTile;
    [SerializeField] private VoxelTile _thirdTile;
    [SerializeField] private GameObject _prefabWarehouse;
    [SerializeField] private Button _buttonSpawn;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private BattlePhaseConfig _battlePhaseConfig;
    [SerializeField] private GlobalMineralsList _mineralConfigs;
    [SerializeField] private PhasesSettings _phasesSettings;
    [SerializeField] private GameObject _defender;
   
   [Range(0f, 1f)]
    [SerializeField] private float _tearOneWeightVariantNik = 0.75f;
    [Range(0f, 1f)]
    [SerializeField] private float _tearTwoWeightVariantNik = 0.23f;
    [Range(0f, 1f)]
    [SerializeField] private float _tearThirdWeightVariantNik = 0.02f;
    
    [SerializeField] private bool _changeVariant;
    
    [Range(0f, 1f)]
    [SerializeField] private float _tearOneWeightSecondVariant = 0.75f;
    [Range(0f, 1f)]
    [SerializeField] private float _tearTwoWeightSecondVariant = 0.23f;
    [Range(0f, 1f)]
    [SerializeField] private float _tearThirdWeightSecondVariant = 0.02f;
    
    [TextArea(3, 5)] [SerializeField] private string _annotation =
        "Если выключено, то вариант Николая и сумма весов должна равняться 1, если включено, то вариант Иоанна, " +
        " 1 - сумма всех весов = вероятности спавна пустоты";

    public GameObject Defender => _defender;
    public GameObject Enemy => _enemy;
    public GameObject PrefabWarehouse => _prefabWarehouse;

    public GameObject TestBuilding;
    public BattlePhaseConfig BattlePhaseConfig => _battlePhaseConfig;
    public PhasesSettings PhasesSettings => _phasesSettings;
    public float TearOneWeightSecondVariant => _tearOneWeightSecondVariant;
    public float TearTwoWeightSecondVariant => _tearTwoWeightSecondVariant;
    public float TearThirdWeightSecondVariant => _tearThirdWeightSecondVariant;
    public bool ChangeVariant => _changeVariant;

    public float TearOneWeightVariantNik => _tearOneWeightVariantNik;

    public float TearTwoWeightVariantNik => _tearTwoWeightVariantNik;

    public float TearThirdWeightVariantNik => _tearThirdWeightVariantNik;

    public GlobalMineralsList MineralConfigs => _mineralConfigs;

    public Button ButtonSpawn => _buttonSpawn;

    public Building BuildFirst => _buildFirst;

    public Building BuildSecond => _buildSecond;

    public VoxelTile FirstTile => _firstTile;

    public VoxelTile SecondTile => _secondTile;

    public VoxelTile ThirdTile => _thirdTile;

    public int MapSizeX => _mapSizeX;

    public int MapSizeY => _mapSizeY;

    public VoxelTile[] TilePrefabs => _tilePrefabs;

    public GameObject MainTower => _mainTower;

    public GameObject BaseUnit => _baseUnit;
    public GameObject Bullet => _bullet;

}
