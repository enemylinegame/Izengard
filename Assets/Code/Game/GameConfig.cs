using Code.Level_Generation;
using Code.Scriptable;
using CombatSystem;
using Core;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Wave;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _mapSizeX;
    [SerializeField] private int _mapSizeY;
    [SerializeField] private VoxelTile[] _tilePrefabs;
    [SerializeField] private VoxelTile _firstTile;
    [SerializeField] private VoxelTile _secondTile;
    [SerializeField] private VoxelTile _thirdTile;
    [SerializeField] private ButtonSetterView _buttonSetterView;

    [Range(0f, 1f)]
    [SerializeField] private float _tearOneWeightVariantNik = 0.75f;
    [Range(0f, 1f)]
    [SerializeField] private float _tearTwoWeightVariantNik = 0.23f;
    [Range(0f, 1f)]
    [SerializeField] private float _tearThirdWeightVariantNik = 0.02f;
    
    [SerializeField] private bool _changeVariant;

    [TextArea(3, 5)] [SerializeField] private string _annotation =
        "Если выключено, то вариант Николая и сумма весов должна равняться 1, если включено, то вариант Иоанна, " +
        " 1 - сумма всех весов = вероятности спавна пустоты";
    public bool ChangeVariant => _changeVariant;

    public float TearOneWeightVariantNik => _tearOneWeightVariantNik;

    public float TearTwoWeightVariantNik => _tearTwoWeightVariantNik;

    public float TearThirdWeightVariantNik => _tearThirdWeightVariantNik;
    public ButtonSetterView ButtonSetterView => _buttonSetterView;

    public VoxelTile FirstTile => _firstTile;

    public VoxelTile SecondTile => _secondTile;

    public VoxelTile ThirdTile => _thirdTile;

    public int MapSizeX => _mapSizeX;

    public int MapSizeY => _mapSizeY;

    public VoxelTile[] TilePrefabs => _tilePrefabs;
}
