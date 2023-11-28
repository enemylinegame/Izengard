using SpawnSystem;

namespace Code.GlobalGameState
{
    public class BattleStateManager
    {

        private DefenderSpawnHandler _defendersSpawnHandler;
        private EnemySpawnHandler _enemySpawnHandler;

        public BattleStateManager(DefenderSpawnHandler defendersSpawnHandler, EnemySpawnHandler enemySpawnHandler)
        {
            _defendersSpawnHandler = defendersSpawnHandler;
            _enemySpawnHandler = enemySpawnHandler;
        }

        public void StartPhase()
        {
            _enemySpawnHandler.StartSpawn();
        }

        public void EndPhase()
        {
            _enemySpawnHandler.StopSpawn();
        }
    }
}