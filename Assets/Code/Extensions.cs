using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private const byte FORWARD_ROAD = 0;
    private const byte RIGHT_ROAD = 1;
    private const byte BACK_ROAD = 2;
    private const byte LEFT_ROAD = 3;

    private static readonly Vector2Int _forward = new Vector2Int(0, -1);
    private static readonly Vector2Int _right = new Vector2Int(-1, 0);
    private static readonly Vector2Int _back = new Vector2Int(0, 1);
    private static readonly Vector2Int _left = new Vector2Int(1, 0);


    public static bool  CheckEmptyPosition(VoxelTile standTile, int xOfset, int yOfset, VoxelTile[,] _spawnedTiles)
    {
        
        var tilePosition = Vector3.zero;
        if (standTile != null)
            tilePosition = standTile.transform.position;
        var xPos = (int)tilePosition.x;
        var yPos = (int)tilePosition.z;
        return _spawnedTiles[xPos + xOfset, yPos + yOfset] == null;
    }
    
    public static List<VoxelTile> TilesCanBeSet(int side, List<VoxelTile> tilePrefabs)
    {
        List<VoxelTile> availableTiles = new List<VoxelTile>();
        foreach (var tile in tilePrefabs)
        {
            if (side == 0 && tile.TablePassAccess[2] == 1 && !availableTiles.Contains(tile))
                availableTiles.Add(tile);
            if (side == 1 && tile.TablePassAccess[3] == 1 && !availableTiles.Contains(tile))
                availableTiles.Add(tile);    
            if (side == 2 && tile.TablePassAccess[0] == 1 && !availableTiles.Contains(tile))
                availableTiles.Add(tile);
            if (side == 3 && tile.TablePassAccess[1] == 1 && !availableTiles.Contains(tile))
                availableTiles.Add(tile);
        }
        return availableTiles;
    }

    public static Vector2Int GetVector2IntWay(this byte way)
    {
        return way switch
        {
            FORWARD_ROAD => new Vector2Int(0, -1),
            RIGHT_ROAD => new Vector2Int(-1, 0),
            BACK_ROAD => new Vector2Int(0, 1),
            LEFT_ROAD => new Vector2Int(1, 0),
            _ => Vector2Int.zero
        };
    }

    public static int GetSide(this Vector2Int Vector2Dirrection)
    {
        if (Vector2Dirrection == _forward) return 0;
        if (Vector2Dirrection == _right) return 1;
        if (Vector2Dirrection == _back) return 2;
        if (Vector2Dirrection == _left) return 3;
        return -1;
    }
}