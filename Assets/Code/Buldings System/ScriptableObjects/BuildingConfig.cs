using System.Collections.Generic;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;

namespace BuildingSystem
{
    [CreateAssetMenu(fileName = nameof(BuildingConfig), menuName = "Tile System/" + nameof(BuildingConfig))]
    public class BuildingConfig : ScriptableObject
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
        public float BuildingTime => _buildingTime;
        public string Description => _description;
        public int MaxWorkers => _maxWorkers;
        public ResourceType Resource => _resource;
    }
}