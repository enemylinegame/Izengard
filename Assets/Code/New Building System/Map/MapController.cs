using UnityEngine;

namespace NewBuildingSystem
{
    public class MapController
    {
        private readonly Map _map;
        private readonly Vector2Int _mapSize;

        public MapController(Map map, Vector2Int mapSize)
        {
            _map = map;
            _mapSize = mapSize;
            SizeMap();
        }

        private void SizeMap()
        {
            float x = _mapSize.x / 10f;
            float y = _mapSize.y / 10f;
            _map.MapObj.transform.localScale = new Vector3(x, 0, y);
        }
    }
}