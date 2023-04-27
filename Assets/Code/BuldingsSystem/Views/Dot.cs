using System;
using UnityEngine;

namespace Code.TileSystem
{
    [RequireComponent(typeof(SphereCollider))]
    public class Dot : MonoBehaviour
    {
        public bool IsActive = true;
        public BuildingTypes Types;
        public Mineral Mineral;

        private void Start()
        {
            Types = BuildingTypes.None;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Mineral>())
            {
                var mineral = other.GetComponent<Mineral>();//TODO Коостыль!!!
                Mineral = mineral;
                IsActive = false;
            }

            if (other.GetComponent<Building>())
            {
                var building = other.GetComponent<Building>();
                Types = building.Type;
                IsActive = false;
            }
        }
    }
}