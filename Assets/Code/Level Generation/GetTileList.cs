using System.Collections.Generic;
using UnityEngine;

public static class GetTileList
{
    static Vector3 pos = Vector3.zero;
    static List<VoxelTile> _tiles = new List<VoxelTile>();

    public static List<VoxelTile> GetTiles(GameConfig gameConfig)
    {
        foreach (var tile in gameConfig.TilePrefabs)
        {
            pos +=Vector3.back*5f;
            _tiles.Add(GameObject.Instantiate(tile, pos, Quaternion.identity));
        }

        return _tiles;
    }

    public static void ClearTiles()
    {
        pos = Vector3.zero;
        _tiles.Clear();
    }
}
