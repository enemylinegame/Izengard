using System.Collections.Generic;
using UnityEngine;

public static class GetTileList
{
    static List<VoxelTile> _tiles = new List<VoxelTile>();

    public static List<VoxelTile> GetTiles(GameConfig gameConfig)
    {
        var resultTiles = new List<VoxelTile>();
        var pos = Vector3.zero;

        foreach (var tile in gameConfig.TilePrefabs)
        {
            pos +=Vector3.back*5f;
            resultTiles.Add(GameObject.Instantiate(tile, pos, Quaternion.identity));
        }

        return resultTiles;
    }
}
