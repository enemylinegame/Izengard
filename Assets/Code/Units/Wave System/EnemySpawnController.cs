using System.Collections.Generic;
using EnemyUnit;
using WaveSystem.View;

namespace WaveSystem
{
    public class EnemySpawnController
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemyPool _enemyPool;

        public EnemySpawnController(
            Damageable primaryTarget,
            EnemySpawnView view, 
            List<EnemyData> enemyDataList)
        {
            _enemyFactory = new EnemyFactory(primaryTarget);
            _enemyPool = new EnemyPool(view.PoolHolder, _enemyFactory, enemyDataList);
        }

        public void SpawnEnemy()
        {

        }
    }
}
