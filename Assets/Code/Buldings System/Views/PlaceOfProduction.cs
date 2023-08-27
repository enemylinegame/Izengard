using Code.BuildingSystem;
using UnityEngine;


namespace Code.TileSystem
{
    public class PlaceOfProduction : MonoBehaviour
    {
        [SerializeField]
        private Transform _workersHousePlace;

        public Vector3 WorkerHousePlace => _workersHousePlace.position;

        public bool IsActive = true;
        public ICollectable Building;
    }
}