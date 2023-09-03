using System;
using System.Collections.Generic;
using Code.TileSystem;
using UnityEngine;


namespace CombatSystem
{
    public class TestDummyTargetController : IDisposable, IOnController, IOnUpdate
    {
        private readonly TileGenerator _levelTileGenerator;
        private readonly GameObject _dummyPrefab;
        private readonly HashSet<DummyController> _instantiatedDummys = new HashSet<DummyController>();

        public HashSet<DummyController> InstatiatedDummys => _instantiatedDummys;
        private DummyController _dummyController;
        private TileView _tileView;


        public TestDummyTargetController(TileGenerator levelTileGenerator, GameObject testBuilding)
        {
            _levelTileGenerator = levelTileGenerator;

            _dummyPrefab = testBuilding;//Resources.Load<GameObject>("DummyTarget");

            // _levelGenerator.SpawnResources += OnNewTile;
            _levelTileGenerator.OnCombatPhaseStart += RespawnDummies;
        }

        private void RespawnDummies()
        {
            //foreach (var dummy in _instantiatedDummys) dummy.Spawn();
        }

        private void OnNewTile(VoxelTile tile)
        {
            if (tile.NumZone == 1) return;
            _tileView = tile.TileView;
            var instaniatedDummy = UnityEngine.Object.Instantiate(_dummyPrefab, tile.transform.position, Quaternion.identity);
            _dummyController = new DummyController(instaniatedDummy);
            
            _instantiatedDummys.Add(_dummyController);
            //foreach (var dummy in _instantiatedDummys) dummy.Spawn();
        }

        public void Dispose()
        {
            // _levelGenerator.SpawnResources -= OnNewTile;
            foreach (var dummyController in _instantiatedDummys) dummyController.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            
        }
    }
}