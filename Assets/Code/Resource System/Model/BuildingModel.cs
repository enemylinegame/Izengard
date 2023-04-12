using ResourceSystem;
using System;
using System.Collections.Generic;
using ResourceSystem.SupportClases;
using UnityEngine;

namespace BuildingSystem
{
    [Serializable]
    public abstract class BuildingModel : IBuildingModel
    {
        public List<ResourcePriceModel> BuildingCost => _buildingCost;
        //public GoldCost ThisBuildingGoldCost => _thisBuildingGoldCost;
        public GameObject BuildingPrefab => _buildingPrefab;
        public TierNumber TierNumber { get; }
        public BuildingTypes BuildingType { get; }
        public string Name => _nameBuilding;
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;


        public Sprite Icon => _icon;

        //public GameObject GotBuildPrefab => _gotBuildPrefab;
        public float BuildingTime => _buildingTime;
        //public float CurrentBuildTime => _currentBuildTime;


        public Action<BuildingModel> AStartBuilding;
        public Action<BuildingModel> ABuildingComplete;

        [SerializeField] protected BuildingTypes _buildingType;
        [SerializeField] protected TierNumber currentTierType;
        [SerializeField] protected List<ResourcePriceModel> _buildingCost;
        //[SerializeField] protected GoldCost _thisBuildingGoldCost;
        private GameObject _buildingPrefab;
        [SerializeField] protected float _currentHealth;
        [SerializeField] protected float _maxHealth;
        [SerializeField] protected Sprite _icon;

        [SerializeField] protected string _nameBuilding;

        //[SerializeField] protected GameObject _gotBuildPrefab;
        [SerializeField] protected float _buildingTime;
        //[SerializeField] protected float _currentBuildTime;
        //[SerializeField] protected GameObject _notGotBuildPrefab;


        public BuildingModel(BuildingModel baseBuilding)
        {
            _buildingCost = new List<ResourcePriceModel>(baseBuilding.BuildingCost);        
            //_thisBuildingGoldCost = new GoldCost(baseBuilding.ThisBuildingGoldCost);
            _currentHealth = baseBuilding.CurrentHealth;
            _maxHealth = baseBuilding.MaxHealth;
            _icon = baseBuilding.Icon;
            _buildingPrefab = baseBuilding._buildingPrefab;
            //_gotBuildPrefab = baseBuilding._gotBuildPrefab;
            _buildingTime = baseBuilding.BuildingTime;
           //_notGotBuildPrefab = baseBuilding._notGotBuildPrefab;
            _nameBuilding = baseBuilding.Name;
           // _currentBuildTime = 0;
            AwakeModel();
        }

        public void SetName(string name)
        {
            _nameBuilding = name;
        }

        public void SetIconBuilding(Sprite icon)
        {
            _icon = icon;
        }

        public void SetCurrentHealth(float value)
        {
            _currentHealth = value;
        }

        public void SetMaxHealth(float value)
        {
            _maxHealth = value;
        }

        public abstract void AwakeModel();


        public void StartBuilding(float value)
        {
            Debug.LogError("Commented by V.Chirkov");
            /*_currentBuildTime += value;
            if (CurrentBuildTime >= BuildingTime)
            {
                _buildingPrefab = _gotBuildPrefab;
                _currentBuildTime = 0;
                ABuildingComplete?.Invoke(this);
            }*/
        }

        public void GetResourceFromGlobalStock(GlobalResorceStock globalStock)
        {
         /*   foreach (ResourceCost resource in _buildingCost)
            {
                resource.GetNeededResource(globalStock.GlobalResStock);
            }*/

            //_buildingCost.GetNeededResource(globalStock.GlobalResStock);
        }

        public void GetGoldCostForBuilding(GlobalResorceStock globalStock)
        {
            Debug.LogError("Commented by V.Chirkov");
            
         /*   if (globalStock.PriceGoldFromGlobalStock(_thisBuildingGoldCost))
            {
                _thisBuildingGoldCost = null;
                AStartBuilding?.Invoke(this);
            }*/
        }
    }
}