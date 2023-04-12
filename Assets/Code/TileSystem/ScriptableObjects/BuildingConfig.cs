using System.Collections.Generic;
using Controllers.BuildBuildingsUI;
using ResourceSystem.SupportClases;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(BuildingConfig), menuName = "Tile System/" + nameof(BuildingConfig))]
public class BuildingConfig : ScriptableObject, IBuildingModel
{
    [SerializeField]private Sprite _icon;

    private float _currentHealth;

    [SerializeField]private float _maxHealth;

    [SerializeField]private string _name;

    [SerializeField]private List<ResourcePriceModel> _buildingCost;

    [SerializeField]private GameObject _buildingPrefab;

    [SerializeField]private TierNumber _tierNumber;

    [SerializeField]private BuildingTypes _buildingType;

    [SerializeField] private HouseType _houseType; //Это Идея
    
    [SerializeField] private string _description; 

    private float _buildingTime;
    /* [SerializeField] private BuildingTypes _TypeForBuildingsUI;
    [SerializeField] private Sprite _imageForBuildingsUI;
    [SerializeField] private string _costForBuildingsUI;
    [SerializeField] private string _infoForBuildingsUI;

    public Sprite SpriteForBuildingsUI => _imageForBuildingsUI;
    public BuildingTypes TypeForBuildingsUI => _TypeForBuildingsUI;
    public string CostForBuildingsUI => _costForBuildingsUI;
    public string InfoForBuildingsUI => _infoForBuildingsUI;*/


    public Sprite Icon => _icon;

    public float CurrentHealth => _currentHealth;

    public float MaxHealth => _maxHealth;

    public string Name => _name;

    public List<ResourcePriceModel> BuildingCost => _buildingCost;

    public GameObject BuildingPrefab => _buildingPrefab;

    public TierNumber TierNumber => _tierNumber;

    public BuildingTypes BuildingType => _buildingType;

    public HouseType HouseType => _houseType;

    public float BuildingTime => _buildingTime;

    public string Description => _description;
}
