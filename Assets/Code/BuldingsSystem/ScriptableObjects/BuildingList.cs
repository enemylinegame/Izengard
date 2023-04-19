using System.Collections.Generic;
using Code.BuldingsSystem.ScriptableObjects;
using UnityEngine;

namespace Code.BuildingSystem
{
    [CreateAssetMenu(fileName = nameof(BuildingList), menuName = "Tile System/" + nameof(BuildingList))]
    public class BuildingList : ScriptableObject
    {
        [SerializeField] private List<BuildingConfig> _buildingsConfig;
        public List<BuildingConfig> BuildingsConfig => _buildingsConfig;
    }
}

