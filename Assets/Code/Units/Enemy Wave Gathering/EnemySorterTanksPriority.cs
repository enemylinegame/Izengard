using Interfaces;
using System.Collections.Generic;
using Wave.Interfaces;


namespace Wave
{
    public class EnemySorterTanksPriority : IEnemySorter
    {
        public List<IPoolController<IEnemyController>> SortEnemyList(List<IPoolController<IEnemyController>> list)
        {
            list.Sort((IPoolController<IEnemyController> pool1, IPoolController<IEnemyController> pool2) =>
            {
                var enemyPool1 = pool1 as EnemyControllerPool;
                var enemyPool2 = pool2 as EnemyControllerPool;
                if (enemyPool1 == null || enemyPool2 == null) return 0;
                if (enemyPool1.Enemy.Type == EnemyType.Tank && enemyPool2.Enemy.Type != EnemyType.Tank) return -1;
                else if (enemyPool1.Enemy.Type != EnemyType.Tank && enemyPool2.Enemy.Type == EnemyType.Tank) return 1;
                else return 0;
            });

            return list;
        }
    }
}