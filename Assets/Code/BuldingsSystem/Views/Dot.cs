using System;
using UnityEngine;

namespace Code.TileSystem
{
    public class Dot : MonoBehaviour
    {
        public bool IsActive = true;
        public BuildingTypes Types;

        private void Start()
        {
            Types = BuildingTypes.None;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Mineral>())
            {
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