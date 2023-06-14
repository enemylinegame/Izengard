using System;
using Data;
using UnityEngine;
using Views.BaseUnit;

namespace Views.Outpost
{
    public class OutpostUnitView : MonoBehaviour
    {
        public OutpostParametersData OutpostParametersData;
        public Action<UnitMovement> UnitInZone;
        public int IndexInArray;

        private void OnTriggerEnter(Collider other)
        {
            var unitMovement = other.gameObject.GetComponent<UnitMovement>();
            if (unitMovement)
            {
                UnitInZone.Invoke(unitMovement);
            }
        }

        public void GetColliderParameters(out Vector3 center,out Vector3 size)
        {
            var collider = gameObject.GetComponent<BoxCollider>();
            center = collider.center;
            size = collider.size;
        }
    }
}