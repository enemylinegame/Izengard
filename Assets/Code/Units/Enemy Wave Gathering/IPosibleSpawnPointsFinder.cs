using System.Collections.Generic;
using Code.TileSystem;
using UnityEngine;


namespace Wave.Interfaces
{
    public interface IPosibleSpawnPointsFinder
    {
        List<Vector3> GetPosibleSpawnPoints();
        void OnNewTileInstantiated(VoxelTile tile, TileModel model);
    }
}