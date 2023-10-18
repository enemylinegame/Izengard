using System.Collections.Generic;
using UnitSystem;
using UnitSystem.Data;
using UnitSystem.Enum;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyPool
    {
        private readonly Dictionary<UnitRoleType, Queue<IUnit>> _pool = new();
        private readonly Dictionary<UnitRoleType, IUnitData> _spawnData = new();
        private readonly Dictionary<UnitRoleType, Transform> _poolHolders = new();
        private readonly EnemyUnitFactory _enemyFactory;

        private Transform _root;

        public EnemyPool(
            Transform root, 
            EnemyUnitFactory enemyFactory, 
            List<UnitCreationData> enemyDataList)
        {
            _root = root;
            _enemyFactory = enemyFactory;

            FillPool(enemyDataList);
        }

        private void FillPool(List<UnitCreationData> enemyDataList)
        {
            foreach (var enemydata in enemyDataList)
            {

                if (_pool.ContainsKey(enemydata.Type) == false)
                {
                    _pool[enemydata.Type] = new Queue<IUnit>();
                    _spawnData[enemydata.Type] = enemydata.UnitSettings;

                    var poolHolder = new GameObject("Pool_" + enemydata.Type.ToString()).transform;
                    poolHolder.parent = _root;
                    _poolHolders[enemydata.Type] = poolHolder;
                }
              
                for (int i = 0; i < enemydata.PoolCopacity; i++)
                {
                    var enemy = _enemyFactory.CreateUnit(enemydata.UnitSettings);

                    enemy.View.SelfTransform.parent = _poolHolders[enemydata.Type];
                    enemy.SetPosition(_poolHolders[enemydata.Type].localPosition);
                    
                    enemy.Disable();

                    _pool[enemydata.Type].Enqueue(enemy);
                }
            }
        }

        public IUnit GetFromPool(UnitRoleType type)
        {
            if (_pool[type].Count > 0)
            {
                var enemy = _pool[type].Dequeue();
                return enemy;
            }
            else
            {
                var enemy = _enemyFactory.CreateUnit(_spawnData[type]);
                return enemy;
            }
        }

        public void ReturnToPool(IUnit enemy)
        {
            var type = enemy.Stats.Role;

            enemy.Disable();
            enemy.SetPosition(_poolHolders[type].localPosition);
            
            _pool[type].Enqueue(enemy);
        }

        public void Dispose()
        {
            _pool.Clear();
        }

    }
}
