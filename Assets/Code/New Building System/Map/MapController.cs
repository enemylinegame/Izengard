using Izengard;
using UnityEngine;

namespace NewBuildingSystem
{
    public class MapController
    {
        private readonly Map _map;
        private readonly GameConfig _config;

        public MapController(Map map, GameConfig config)
        {
            _map = map;
            _config = config;
            SizeMap();
        }

        private void SizeMap()
        {
            float x = _config.MapSize.x / 10f;
            float y = _config.MapSize.y / 10f;
            _map.MapObj.transform.localScale = new Vector3(x, 0, y);
        }
    }
}