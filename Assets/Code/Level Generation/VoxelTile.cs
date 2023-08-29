using Code.TileSystem;
using UnityEngine;


public class VoxelTile : MonoBehaviour
{
    [SerializeField] private Sprite _iconTile;
    
    [field: SerializeField] public TileView TileView { get; set; }
    
    private float offset = 0.1f;
    private byte [] _tablePassAccess = new byte[4];
    private int _sizeTile;
    private float _sizeTileY;

    public int NumZone { get; set; }

    public bool IsDefendTile { get; set; }
    //public int WeightTile { get; set; }

    public float SizeTileY => _sizeTileY;

    public int SizeTile => _sizeTile;

    public Sprite IconTile => _iconTile;

    public byte[] TablePassAccess => _tablePassAccess;
    
    void Awake()
    {
        PassAccess();
    }

    private void PassAccess()
    {
        var meshCollider = GetComponentInChildren<MeshCollider>();
        var bounds = meshCollider.bounds;
        _sizeTileY = bounds.max.y;
        _sizeTile = (int) bounds.size.x;

        if (!CheckRoad(new Vector3(bounds.center.x, 
            bounds.center.y + bounds.center.y / 2, bounds.min.z - offset), 
            Vector3.forward))
        {
            _tablePassAccess[0] = 1;
        }

        if (!CheckRoad(new Vector3(bounds.min.x - offset, 
            bounds.center.y + bounds.center.y / 2, bounds.center.z), 
            Vector3.right))
        {
            _tablePassAccess[1] = 1;
        }

        if (!CheckRoad(new Vector3(bounds.center.x, 
            bounds.center.y + bounds.center.y / 2, bounds.max.z + offset), 
            Vector3.back))
        {
            _tablePassAccess[2] = 1;
        }

        if (!CheckRoad(new Vector3(bounds.max.x + offset, 
            bounds.center.y + bounds.center.y / 2, bounds.center.z), 
            Vector3.left))
        {
            _tablePassAccess[3] = 1;
        }
    }

    private bool CheckRoad(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(new Ray(origin, direction), 0.2f))
        {
            return true;
        }
        return false;
    }
}