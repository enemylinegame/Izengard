using LevelGenerator.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.TileSystem;
using UnityEngine;
using UnityEngine.AI;


namespace LevelGenerator
{
    public class TileSetter : ITileSetter
    {
        public Vector2Int FirstTileGridPosition { get; private set; } = Vector2Int.zero;
        private readonly Vector3 _firstTilePosition = new Vector3(200, 0, 200);

        private readonly Transform _tilesParent;
        private readonly List<VoxelTile> _availableTiles;
        private readonly Dictionary<Vector2Int, VoxelTile> _spawnedTiles;
        private int _count;

        private NavMeshSurface _navMeshSurface;


        public TileSetter(List<VoxelTile> availableTiles, Dictionary<Vector2Int, VoxelTile> spawnedTiles, VoxelTile firstTile, GlobalTileSettings tileSettings)
        {
            _availableTiles = availableTiles;
            _spawnedTiles = spawnedTiles;
            _tilesParent = new GameObject("Tiles").transform;
            _navMeshSurface = _tilesParent.gameObject.AddComponent<NavMeshSurface>();//Object.FindObjectOfType<NavMeshSurface>();
            _navMeshSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
            _navMeshSurface.overrideTileSize = true;
            _navMeshSurface.tileSize = 128;
            _navMeshSurface.overrideVoxelSize = true;
            _navMeshSurface.voxelSize = 0.03f;


            PlaceFirstTile(firstTile, tileSettings);
        }

        private void PlaceFirstTile(VoxelTile firstTile, GlobalTileSettings tileSettings)
        {
            _spawnedTiles.Add(FirstTileGridPosition, InstantiateTile(firstTile, _firstTilePosition, tileSettings));
            _navMeshSurface.BuildNavMesh();//_navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
        }

        public async void SetTile(TileSpawnInfo spawnInfo, GlobalTileSettings tileSettings)
        {
            var side = (spawnInfo.GridSpawnPosition - spawnInfo.GridBasePosition);
            _spawnedTiles.Add(spawnInfo.GridSpawnPosition, GetTile(side, spawnInfo.GridBasePosition, tileSettings));
            _spawnedTiles[spawnInfo.GridSpawnPosition].IsDefendTile = spawnInfo.IsDefendTile;
            await Task.Delay(200);
            _navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
        }

        private VoxelTile GetTile(Vector2Int side, Vector2Int baseTileGridPosition, GlobalTileSettings tileSettings)
        {
            var tilesCanBeSet = Extensions.TilesCanBeSet(side.GetSide(), _availableTiles);
            byte[] test = { 0, 1, 0, 1 };
            byte[] testSecond = { 1, 0, 1, 0 };
            VoxelTile voxelTile = _availableTiles[0];
            if (_count < 2)
            {
                foreach (var tilePrefab in tilesCanBeSet)
                {
                    if (Enumerable.SequenceEqual(tilePrefab.TablePassAccess, test) | 
                        Enumerable.SequenceEqual(tilePrefab.TablePassAccess, testSecond))
                    {
                        voxelTile = tilePrefab;
                        break;
                    }
                    voxelTile = tilesCanBeSet[Random.Range(0, tilesCanBeSet.Count)];
                }
                _count++;
            }
            else
            {
                voxelTile = tilesCanBeSet[Random.Range(0, tilesCanBeSet.Count)];
                _count = 0;
            }
            var pos = _spawnedTiles[baseTileGridPosition].transform.position + 
                new Vector3(side.x * voxelTile.SizeTile, 0, side.y * voxelTile.SizeTile);

            return InstantiateTile(voxelTile, pos, tileSettings);
        }

        private VoxelTile InstantiateTile(VoxelTile voxelTile, Vector3 pos, GlobalTileSettings tileSettings)
        {
            var tile = Object.Instantiate(voxelTile, pos, Quaternion.identity, _tilesParent);
            tile.TileView.TileModel.TileConfig = tileSettings.Levels[0];
            tile.TileView.TileModel.Init();
            
            //расчет веса тайла для генерации ресурсов по Николаю
            tile.NumZone = voxelTile.NumZone + 1;
            if (tile.NumZone == 2)
            {
                tile.WeightTile = 5;
            }
            else
            {
                tile.WeightTile = 5 * (tile.NumZone - 1);
            }

            //установка склада при создании тайла
            //CreateWarehouse(tile);

            return tile;
        }
    }
}