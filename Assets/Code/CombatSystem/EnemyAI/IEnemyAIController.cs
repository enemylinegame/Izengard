namespace CombatSystem
{
    public interface IEnemyAIController : IOnUpdate
    {
        void AddEnemyAI(IEnemyAI enemyAI);
        void RemoveEnemyAI(IEnemyAI enemyAI);
    }
}