using System.Collections.Generic;
using UnityEngine;


namespace Wave.Interfaces
{
    public interface IPosibleSpawnPointsFinder
    {
        List<Vector3> GetPosibleSpawnPoints();
        void OnNewTileInstantiated(VoxelTile tile);
    }
}