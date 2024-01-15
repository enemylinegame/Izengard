using Abstraction;
using NewBuildingSystem;
using UnityEngine;

namespace SpawnSystem
{
    public class Spawner : Building
    {
        [SerializeField]
        private Transform _spawnlocation;

        public Transform SpawnLocation => _spawnlocation;

        public FactionType FactionType { get; private set; }

        public void SetFaction(FactionType factionType)
        {
            FactionType = factionType;
        }
    }
}
