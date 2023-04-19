using System;
using System.Collections.Generic;
using UnityEngine;


namespace CombatSystem
{
    public class TestDummyTargetController : IDisposable, IOnController
    {
        private readonly GeneratorLevelController _levelGenerator;
        private readonly GameObject _dummyPrefab;
        private readonly HashSet<DummyController> _instantiatedDummys = new HashSet<DummyController>();

        public HashSet<DummyController> InstatiatedDummys => _instantiatedDummys;


        public TestDummyTargetController(GeneratorLevelController levelGenerator, GameObject testBuilding)
        {
            _levelGenerator = levelGenerator;

            _dummyPrefab = testBuilding;//Resources.Load<GameObject>("DummyTarget");

            _levelGenerator.SpawnResources += OnNewTile;
            _levelGenerator.OnCombatPhaseStart += RespawnDummies;
        }

        private void RespawnDummies()
        {
            foreach (var dummy in _instantiatedDummys) dummy.Spawn();
        }

        private void OnNewTile(VoxelTile tile)
        {
            if (tile.NumZone == 1) return;
            var instaniatedDummy = UnityEngine.Object.Instantiate(_dummyPrefab, tile.transform.position, Quaternion.identity);
            var dummyController = new DummyController(instaniatedDummy);
            _instantiatedDummys.Add(dummyController);
            foreach (var dummy in _instantiatedDummys) dummy.Spawn();
        }

        public void Dispose()
        {
            _levelGenerator.SpawnResources -= OnNewTile;
            foreach (var dummyController in _instantiatedDummys) dummyController.Dispose();
        }
    }
}