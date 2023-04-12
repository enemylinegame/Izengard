using ResurseSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{ 
    [Serializable]    
    public abstract class BuildingModel : IBuildingModel
        {
        public int BuildingTypeID => (int)_buildingType;
        public int CurrentTirTypeID => (int)_currentTirType;
        public ResurseCost ThisBuildingCost => _thisBuildinCost;
        public GoldCost ThisBuildingGoldCost => _thisBuildingGoldCost;
        public GameObject BasePrefab => _thisBuildingPrefab;        
        public string Name => _nameBuilding;
        public float Health => _currentHealth;
        public float MaxHealth => _maxHealth;
        public Sprite Icon => _icon;
        public GameObject GotBuildPrefab => _gotBuildPrefab;
        public float BuildingTime => _buildingTime;
        public float CurrentBuildTime => _currentBuildTime;
        

        public Action<BuildingModel> AStartBuilding;
        public Action<BuildingModel> ABuildingComplete;

        [SerializeField] protected TypeOfBuildings _buildingType;
        [SerializeField] protected TirNumber _currentTirType;
        [SerializeField] protected ResurseCost _thisBuildinCost;
        [SerializeField] protected GoldCost _thisBuildingGoldCost;
        private GameObject _thisBuildingPrefab;        
        [SerializeField] protected float _currentHealth;
        [SerializeField] protected float _maxHealth;
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected string _nameBuilding;
        [SerializeField] protected GameObject _gotBuildPrefab;
        [SerializeField] protected float _buildingTime;
        [SerializeField] protected float _currentBuildTime;
        [SerializeField] protected GameObject _notGotBuildPrefab;
              
               

        public BuildingModel(BuildingModel baseBuilding)
        {
            _thisBuildinCost = new ResurseCost(baseBuilding.ThisBuildingCost);            
            _thisBuildingGoldCost = new GoldCost(baseBuilding.ThisBuildingGoldCost);
            _currentHealth = baseBuilding.Health;
            _maxHealth = baseBuilding.MaxHealth;
            _icon = baseBuilding.Icon;
            _thisBuildingPrefab = baseBuilding._thisBuildingPrefab;
            _gotBuildPrefab = baseBuilding._gotBuildPrefab;
            _buildingTime = baseBuilding.BuildingTime;
            _notGotBuildPrefab = baseBuilding._notGotBuildPrefab;
            _nameBuilding = baseBuilding.Name;           
            _currentBuildTime = 0;
            AwakeModel();
        }
        public void SetName(string name)
        {
            _nameBuilding=name;
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
            _currentBuildTime += value;
            if (CurrentBuildTime >= BuildingTime)
            {
                _thisBuildingPrefab = _gotBuildPrefab;                
                _currentBuildTime = 0;                
                ABuildingComplete?.Invoke(this);
            }
        } 
        public void GetResurseFromGlobalStock (GlobalResurseStock globalStock)
        {
            _thisBuildinCost.GetNeededResurse(globalStock.GlobalResStock);
            
        }
        public void GetGoldCostForBuilding(GlobalResurseStock globalStock)
        {            
            if (globalStock.PriceGoldFromGlobalStock(_thisBuildingGoldCost))
            {
                _thisBuildingGoldCost = null;
                AStartBuilding?.Invoke(this);
            }
        }
    }
}

