using Interfaces;
using System;
using UnityEngine;
using Views.BaseUnit;


namespace Models.BaseUnit
{
    public class BaseUnitFactory: IUnitFactory, IDisposable
    {
        private readonly IPoolController<GameObject> _pool;


        public BaseUnitFactory(IPoolController<GameObject> pool)
        {
            _pool = pool;
        }

        public GameObject CreateUnit(GameObject whichPrefab , Vector3 whereToPlace)
        {
            //return GameObject.Instantiate(whichPrefab,whereToPlace,new Quaternion());

            var poolObject = _pool.GetObjectFromPool();
            if (poolObject.TryGetComponent<UnitMovement>(out var unitMovement))
            {
                unitMovement.navMeshAgent.Warp(whereToPlace);
            }
            return poolObject;
        }

        public void Dispose()
        {
            _pool?.Dispose();
        }
    }
}