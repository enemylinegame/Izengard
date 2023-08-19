using System.Collections.Generic;
using Code.BuildingSystem;
using Code.TileSystem;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.BuildingSystem
{
    [CreateAssetMenu(fileName = nameof(BuildingConfig), menuName = "Tile System/" + nameof(BuildingConfig))]
    public class BuildingConfig : ScriptableObject, IBuildingModel
    {
        [Header("Building settings")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _maxHealth;
        [SerializeField] private string _name;
        [SerializeField] private List<ResourcePriceModel> _buildingCost;
        [SerializeField] private GameObject _buildingPrefab;
        [SerializeField] private TierNumber _tierNumber;
        [SerializeField] private BuildingTypes _buildingType;
        [SerializeField] private string _description;
        
        [Header("Load Settings")]
        [SerializeField] private List<TileType> _tileType;

        [SerializeField] private TileLvl _levelTile;
        
        [Header("Production building")]
        [SerializeField] private int _maxWorkers;
        [SerializeField] private ResourceType _resource;

        private float _buildingTime;
        private float _currentHealth;
        public Sprite Icon => _icon;
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public string Name => _name;
        public List<ResourcePriceModel> BuildingCost => _buildingCost;
        public GameObject BuildingPrefab => _buildingPrefab;
        public TierNumber TierNumber => _tierNumber;
        public BuildingTypes BuildingType => _buildingType;
        public List<TileType> TileType => _tileType;
        public float BuildingTime => _buildingTime;
        public string Description => _description;
        public int MaxWorkers => _maxWorkers;
        public ResourceType Resource => _resource;
        public TileLvl LevelTile => _levelTile;
    }
}