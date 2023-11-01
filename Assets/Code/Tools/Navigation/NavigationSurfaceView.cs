using UnityEngine;
using UnityEngine.AI;

namespace Tools.Navigation
{
    public class NavigationSurfaceView : MonoBehaviour
    {
        [SerializeField] private NavMeshCollectGeometry _meshCollectGeometry;
        [SerializeField] private bool _overrideTileSize = true;
        [SerializeField] private int _tileSize = 64; 
        [SerializeField] private bool _overrideVoxelSize = true;
        [SerializeField] private float voxelSize = 0.03f;

        private GameObject _rootGameobject;

        public NavMeshCollectGeometry MeshCollectGeometry => _meshCollectGeometry;
        public bool OverrideTileSize => _overrideTileSize;
        public int TileSize  => _tileSize;
        public bool OverrideVoxelSize => _overrideVoxelSize;
        public float VoxelSize => voxelSize;

        public GameObject RootGameobject => _rootGameobject;

        private void OnEnable()
        {
            _rootGameobject ??= gameObject;
        }
    }
}
