using System.Collections.Generic;


namespace CombatSystem
{
    public class EnemyAIController : IEnemyAIController
    {
        private const byte MAX_OPERATIONS_IN_FRAME = 10;
        private readonly List<IEnemyAI> _enemyAIs = new List<IEnemyAI>();
        private byte _counter;
        private int _currenIndex;


        public void AddEnemyAI(IEnemyAI enemyAI)
        {
            _enemyAIs.Add(enemyAI);
        }

        public void RemoveEnemyAI(IEnemyAI enemyAI)
        {
            _enemyAIs.Remove(enemyAI);
        }

        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _enemyAIs.Count; i++)
                _enemyAIs[i].OnUpdate(deltaTime);
            StartAILogic();
        }

        private void StartAILogic()
        {
            while (_counter < MAX_OPERATIONS_IN_FRAME)
            {
                if (_currenIndex < _enemyAIs.Count)
                {
                    if (_enemyAIs[_currenIndex].IsActionComplete)
                        _enemyAIs[_currenIndex].StartAction();
                }
                _currenIndex++;
                _counter++;
                if (_currenIndex >= _enemyAIs.Count)
                {
                    _currenIndex = 0;
                    _counter = 0;
                    break;
                }
            }
            _counter = 0;
        }
    }
}