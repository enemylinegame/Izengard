using CombatSystem;
using EnemyUnit.Interfaces;
using UnityEngine;

namespace EnemyUnit
{
    public class EnemyFactory
    {
        private readonly Damageable _target;
        private readonly IEnemyAnimationController _animationController;

        public EnemyFactory(Damageable target)
        {
            _target = target;
        }


        public IEnemyController CreateEnemy(EnemyData enemyData)
        {
            switch (enemyData.Type)
            {
                default:
                    return null;
                case EnemyType.Tank:
                    {
                        var enemy = CreateTank(enemyData);
                        return enemy;
                    }
            }
        }

        public IEnemyController CreateTank(EnemyData enemyData) 
        {
            var enemyObj = Object.Instantiate(enemyData.Prefab);

            var view = enemyObj.GetComponent<EnemyView>();
            var model = new EnemyModel(enemyData, _target);

            return new EnemyController(model, view, _target, _animationController);
        }
    }
}
