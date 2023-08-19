using Code.TileSystem;
using UnityEngine;

namespace LevelGenerator.Interfaces
{
    public interface ITileSetter
    {
        Vector2Int FirstTileGridPosition { get; }
        void SetTile(TileSpawnInfo spawnInfo, GlobalTileSettings tileSettings);
    }
}