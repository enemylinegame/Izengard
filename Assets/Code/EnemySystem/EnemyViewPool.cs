using System.Collections.Generic;
using UnitSystem;
using UnitSystem.Data;
using UnitSystem.Enum;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyViewPool
    {
        private readonly Dictionary<UnitType, Queue<IUnitView>> _pool = new();
        private readonly Dictionary<UnitType, Transform> _poolHolders = new();
        private readonly List<UnitCreationData> creationDataList = new();
        private Transform _root;

        public EnemyViewPool(Transform root, List<UnitCreationData> enemyDataList)
        {
            _root = root;
            foreach (var enemydata in enemyDataList)
            {
                creationDataList.Add(enemydata);
            }

            FillPool(enemyDataList);
        }

        private void FillPool(List<UnitCreationData> enemyDataList)
        {
            foreach (var unitCreationData in enemyDataList)
            {
                if (_pool.ContainsKey(unitCreationData.Type) == false)
                {
                    _pool[unitCreationData.Type] = new Queue<IUnitView>();

                    var poolHolder = new GameObject("Pool_" + unitCreationData.Type.ToString()).transform;
                    poolHolder.parent = _root;
                    _poolHolders[unitCreationData.Type] = poolHolder;
                }

                for (int i = 0; i < unitCreationData.PoolCopacity; i++)
                {
                    var unitView = CreateUnit(unitCreationData);

                    unitView.Hide();

                    _pool[unitCreationData.Type].Enqueue(unitView);
                }
            }
        }

        private IUnitView CreateUnit(UnitCreationData creationData)
        {
            var obj = Object.Instantiate(creationData.UnitPrefab);
            var unitView = obj.GetComponent<IUnitView>();

            unitView.SelfTransform.parent
                = _poolHolders[creationData.Type];

            unitView.SelfTransform.position
                = _poolHolders[creationData.Type].localPosition;

            return unitView;
        }

        public IUnitView GetFromPool(UnitType type)
        {
            if (_pool[type].Count > 0)
            {
                var unitView = _pool[type].Dequeue();
                return unitView;
            }
            else
            {
                var unitCreationData = creationDataList.Find(u => u.Type == type);

                var unitView = CreateUnit(unitCreationData);

                return unitView;
            }
        }

        public void ReturnToPool(IUnitView enemy, UnitType type)
        {
            enemy.SelfTransform.localPosition
                = _poolHolders[type].localPosition;

            enemy.Hide();

            _pool[type].Enqueue(enemy);
        }

        public void Dispose()
        {
            _pool.Clear();
        }
    }
}
