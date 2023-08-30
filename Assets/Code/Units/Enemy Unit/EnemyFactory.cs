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
            var enemy = enemyData.Type switch
            {
                EnemyType.Tank => CreateTank(enemyData),
                EnemyType.Archer => CreateArcher(enemyData),
                EnemyType.Hound => CreateHound(enemyData),
                EnemyType.Boss => CreateBoss(enemyData),
                _ => null,
            };
            return enemy;
        }

        public IEnemyController CreateTank(EnemyData enemyData)
        {
            var enemyObj = Object.Instantiate(enemyData.Prefab);

            var view = enemyObj.GetComponent<EnemyView>();
            var model = new EnemyModel(enemyData, _target);

            return new EnemyController(model, view, _target, _animationController);
        }

        public IEnemyController CreateArcher(EnemyData enemyData)
        {
            throw new System.NotImplementedException();
        }

        public IEnemyController CreateHound(EnemyData enemyData)
        {
            throw new System.NotImplementedException();
        }

        public IEnemyController CreateBoss(EnemyData enemyData)
        {
            throw new System.NotImplementedException();
        }
    }
}
