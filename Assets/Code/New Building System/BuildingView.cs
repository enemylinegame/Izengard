using System.Collections.Generic;
using UnityEngine;

namespace NewBuildingSystem
{
    public class BuildingView : MonoBehaviour
    {
        public Renderer BuildingRenderer;
        [SerializeField] private Transform _positionBuild;
        private readonly List<GameObject> _cellIndicator = new ();

        public void SearchCenterTile(Vector3 size)
        {
            _positionBuild.position = new Vector3(size.x / 2, 0, size.y / 2);
        }

        public void CreateIndicators(Vector2Int value, GameObject cellIndicator)
        {
            for (float x = 0; x < value.x; x++)
            {
                for (float y = 0; y < value.y; y++)
                {
                    if (_cellIndicator.Find(ind => ind.transform.position != new Vector3(x, 0, y)) || _cellIndicator.Count == 0)
                    {
                        var indicator = Instantiate(cellIndicator, transform);
                        indicator.transform.position = new Vector3(x, 0, y);
                        _cellIndicator.Add(indicator);
                    }
                    
                }
            }
        }
    }
}