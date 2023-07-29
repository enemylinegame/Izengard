using System.Collections.Generic;
using ResourceSystem.SupportClases;
using UnityEngine;

namespace Code.TileSystem
{
    [CreateAssetMenu(fileName = nameof(RepairAndRecoberCostCenterBuilding), menuName = "Tile System/" + nameof(RepairAndRecoberCostCenterBuilding))]
    public class RepairAndRecoberCostCenterBuilding : ScriptableObject
    {
        [SerializeField] private List<ResourcePriceModel> _repairCost;
        [SerializeField] private List<ResourcePriceModel> _recoveryCost;
        
        public List<ResourcePriceModel> RecoveryCost => _recoveryCost;
        public List<ResourcePriceModel> RepairCost => _repairCost;
    }
}