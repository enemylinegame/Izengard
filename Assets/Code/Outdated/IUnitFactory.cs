using UnityEngine;

namespace Interfaces
{
    public interface IUnitFactory
    {
        public GameObject CreateUnit(GameObject whichPrefab, Vector3 whereToPlace);
    }
}