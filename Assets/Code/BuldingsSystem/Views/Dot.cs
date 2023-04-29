using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.TileSystem
{
    [RequireComponent(typeof(SphereCollider))]
    public class Dot : MonoBehaviour
    {
        public bool IsActive = true;
        public Building Building;
        public Mineral Mineral;

        // private void Start()
        // {
        //     Building = BuildingTypes.None;
        // }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.GetComponent<Mineral>())
        //     {
        //         var mineral = other.GetComponent<Mineral>();//TODO Коостыль!!!
        //         Mineral = mineral;
        //         IsActive = false;
        //     }
        //
        //     if (other.GetComponent<Building>())
        //     {
        //         var building = other.GetComponent<Building>();
        //         Building = building.Type;
        //         IsActive = false;
        //     }
        // }
    }
}