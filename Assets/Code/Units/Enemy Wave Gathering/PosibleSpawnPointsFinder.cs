using System.Collections.Generic;
using Code.TileSystem;
using UnityEngine;
using Wave.Interfaces;


namespace Wave
{
    public class PosibleSpawnPointsFinder : IPosibleSpawnPointsFinder
    {
        private const float SPAWN_OFFSET = 1.2f;

        private List<Vector3> _posibleSpawnPoints;
        public IReadOnlyDictionary<Vector2Int, VoxelTile> _spawnedTiles;


        public PosibleSpawnPointsFinder(IReadOnlyDictionary<Vector2Int, VoxelTile> spawnedTiles)
        {
            _spawnedTiles = spawnedTiles;
        }

        public List<Vector3> GetPosibleSpawnPoints()
        {
            return _posibleSpawnPoints;
        }

        public void OnNewTileInstantiated(VoxelTile tile, TileModel model)
        {
            SetPosibleSpawnPoints();
        }

        private void SetPosibleSpawnPoints()
        {
            _posibleSpawnPoints = new List<Vector3>();
            foreach (var tile in _spawnedTiles)
            {
                if (tile.Value.NumZone == 1) continue;
                for (byte way = 0; way < tile.Value.TablePassAccess.Length; ++way)
                {
                    if (tile.Value.TablePassAccess[way] == 1)
                    {
                        var posibleNextInstantiatedTileGridPosition = tile.Key + way.GetVector2IntWay();

                        if (!_spawnedTiles.ContainsKey(posibleNextInstantiatedTileGridPosition))
                        {
                            AddNewSpawnPoint(tile, way);
                        }
                    }
                }
            }
        }

        private void AddNewSpawnPoint(KeyValuePair<Vector2Int, VoxelTile> instantiatedTile, byte way)
        {
            var Vector2IntWay = way.GetVector2IntWay();
            var spawnPoint = instantiatedTile.Value.transform.position +
                new Vector3(Vector2IntWay.x * SPAWN_OFFSET, instantiatedTile.Value.SizeTileY, Vector2IntWay.y * SPAWN_OFFSET);
            _posibleSpawnPoints.Add(spawnPoint);
        }
    }
}