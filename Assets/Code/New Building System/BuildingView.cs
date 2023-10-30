using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewBuildingSystem
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        private List<GameObject> _cellIndicators = new ();
        
        public string ID;
        public Renderer BuildingRenderer;
        public Transform PositionBuild;
        [HideInInspector] public Vector2Int Vector2IntPosition;
        [HideInInspector] public Vector2Int Size;
        public event Action<bool, BuildingView> OnTriggered;

        public void InstallInCenterTile(Vector2Int size)
        {
            Size = size;
            var sizeX = size.x / 2f;
            var sizeY = size.y / 2f;
            
            PositionBuild.position = new Vector3(sizeX, 0, sizeY);
            _collider.center = new Vector3(sizeX, .36f, sizeY);
            _collider.size = new Vector3(size.x -.5f , .5f, size.y -.5f);
        }

        public void CreateIndicators(Vector2Int size, GameObject cellIndicator)
        {
            for (float x = 0; x < size.x; x++)
            {
                for (float y = 0; y < size.y; y++)
                {
                    if (_cellIndicators.Find(ind => ind.transform.position 
                                                    != new Vector3(x, 0, y)) || _cellIndicators.Count == 0)
                    {
                        var indicator = Instantiate(cellIndicator, transform);
                        indicator.transform.position = new Vector3(x, 0, y);
                        _cellIndicators.Add(indicator);
                    }
                    
                }
            }
        }

        public void ChangePrewievColor(Color color)
        {
            BuildingRenderer.material.color = color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Building"))
            {
                OnTriggered?.Invoke(false, this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggered?.Invoke(true, this);
        }
    }
}