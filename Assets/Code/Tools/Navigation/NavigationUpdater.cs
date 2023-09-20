using System;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Izengard.Tools.Navigation
{
    public class NavigationUpdater : IDisposable
    {
        private Dictionary<int, NavMeshSurface> _surfacesCollection 
            = new Dictionary<int, NavMeshSurface>();

        private int _surfaceIndex;

        public NavigationUpdater() 
        {
            _surfaceIndex = 0;
        }

        public int AddNavigationSurface(NavigationSurfaceView surfaceView)
        {
            _surfacesCollection[_surfaceIndex] = SetupSurface(surfaceView);

            return _surfaceIndex++;
        }

        private NavMeshSurface SetupSurface(NavigationSurfaceView surfaceView) 
        {
            var navMeshSurf = surfaceView.RootGameobject.AddComponent<NavMeshSurface>();
            navMeshSurf.useGeometry = surfaceView.MeshCollectGeometry;
            navMeshSurf.overrideTileSize = surfaceView.OverrideTileSize;
            navMeshSurf.tileSize = surfaceView.TileSize;
            navMeshSurf.overrideVoxelSize = surfaceView.OverrideVoxelSize;
            navMeshSurf.voxelSize = surfaceView.VoxelSize;

            navMeshSurf.BuildNavMesh();

            return navMeshSurf;
        }

        public void UpdateSurface(int index)
        {
            _surfacesCollection[index].BuildNavMesh();
        }

        public void RemoveSurface(int index) 
        {
            _surfacesCollection.Remove(index);
        }

        public void Dispose()
        {
            _surfacesCollection.Clear();
        }
    }
}
