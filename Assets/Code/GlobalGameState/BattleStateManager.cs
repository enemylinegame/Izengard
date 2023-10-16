using BattleSystem;

namespace Code.GlobalGameState
{
    public class BattleStateManager
    {

        private DefendersSpawnLogicMock _defendersSpawnLogic;
        private EnemySpawnLogicMock _enemySpawnLogic;

        public BattleStateManager(DefendersSpawnLogicMock defendersSpawnLogic, EnemySpawnLogicMock enemySpawnLogic)
        {
            _defendersSpawnLogic = defendersSpawnLogic;
            _enemySpawnLogic = enemySpawnLogic;
        }
        
        
        
        public void StartPhase()
        {
            _enemySpawnLogic.StartSpawn();
        }

        public void EndPhase()
        {
            _enemySpawnLogic.StopSpawn();
        }
        
        
    }
}