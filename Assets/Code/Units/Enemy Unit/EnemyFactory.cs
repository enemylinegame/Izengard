using CombatSystem;
using EnemyUnit.Core;
using EnemyUnit.Interfaces;
using UnityEngine;

namespace EnemyUnit
{
    public class EnemyFactory
    {
        private readonly Damageable _target;
        private readonly IEnemyAnimationController _animationController;

        private int _enemyIndex = 0;

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
                _ => new StubEnemyController(),
            };

            enemy.SetIndex(_enemyIndex++);
            return enemy;
        }

        public IEnemyController CreateTank(EnemyData enemyData)
        {
            var enemyObj = Object.Instantiate(enemyData.Prefab);

            var view = enemyObj.GetComponent<EnemyView>();
            var model = new EnemyModel(enemyData);
            var core = new EnemyCore(model, view, _target);

            var statesholder = new EnemyStatesHolder(model, _animationController, core);

            return new EnemyController(model, view, core, statesholder);
        }

        public IEnemyController CreateArcher(EnemyData enemyData)
        {
            var enemyObj = Object.Instantiate(enemyData.Prefab);

            var view = enemyObj.GetComponent<EnemyView>();
            var model = new EnemyModel(enemyData);
            //Доработать отдельно для Archer Enemy
            var core = new EnemyCore(model, view, _target);
            var statesholder = new EnemyStatesHolder(model, _animationController, core);

            return new EnemyController(model, view, core, statesholder);
        }

        public IEnemyController CreateHound(EnemyData enemyData)
        {
            var enemyObj = Object.Instantiate(enemyData.Prefab);

            var view = enemyObj.GetComponent<EnemyView>();
            var model = new EnemyModel(enemyData);
            //Доработать отдельно для Hound Enemy
            var core = new EnemyCore(model, view, _target);
            var statesholder = new EnemyStatesHolder(model, _animationController, core);

            return new EnemyController(model, view, core, statesholder);
        }

        public IEnemyController CreateBoss(EnemyData enemyData)
        {
            var enemyObj = Object.Instantiate(enemyData.Prefab);

            var view = enemyObj.GetComponent<EnemyView>();
            var model = new EnemyModel(enemyData);
            //Доработать отдельно для Boss Enemy
            var core = new EnemyCore(model, view, _target);
            var statesholder = new EnemyStatesHolder(model, _animationController, core);

            return new EnemyController(model, view, core, statesholder);
        }
    }
}
