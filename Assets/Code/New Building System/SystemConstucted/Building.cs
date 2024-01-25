using Abstraction;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace NewBuildingSystem
{
    public class Building : BaseGameObject, ITarget
    {
        [Header("Colliders")]
        public BoxCollider Collider;
        public BoxCollider ColliderArea;

        [Space, Header("Components")] 
        public Renderer BuildingRenderer;
        public Transform PositionBuild;
        public GameObject ObjectBuild;
        
        [Space, Header("Info")] 
        public string _id;
        public string _name;
        public Image Image;
        public EnumBuildings BuildingsType;
        public int CurrentCountWorkers;
        public int MaxCountWorkers;
        public Vector2Int Size;

        public string Id => _id;
        public string Name => _name;
        public Vector3 Position => transform.position;

        public event Action<bool, Building> OnTriggered; //TODO : 1 option
        public event Action<Building> OnResources;
        
        public void Init(string buildingId)
        {
            _id = buildingId;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Building"))
            {
                OnTriggered?.Invoke(false, this);
                if (other.GetComponent<Building>().BuildingsType == EnumBuildings.Mining)
                {
                    OnResources?.Invoke(other.GetComponent<Building>());
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            OnTriggered?.Invoke(true, this);
            OnResources?.Invoke(null);
        }

        public void ChangePreviewColor(Color color)
        {
            BuildingRenderer.material.color = color;
        }
    }
}