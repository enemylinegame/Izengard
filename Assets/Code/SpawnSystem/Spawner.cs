using NewBuildingSystem;
using UnitSystem.Enum;
using UnityEngine;

namespace SpawnSystem
{
    public class Spawner : Building
    {
        [SerializeField]
        private Transform _spawnlocation;

        public Transform SpawnLocation => _spawnlocation;

        public UnitFactionType FactionType { get; private set; }

        public void SetFaction(UnitFactionType factionType)
        {
            FactionType = factionType;
        }
    }
}
