using CombatSystem;
using System;
using System.Threading.Tasks;
using UnityEngine;


namespace Wave
{
    public class EnemyController : IEnemyController
    {
        public Enemy Enemy { get; private set; }
        public event Action<IEnemyController> OnEnemyDead;

        private readonly IEnemyAnimationController _enemyAnimation;
        private IEnemyAI _enemyAI;
        private readonly GeneratorLevelController _generatorLevelController;
        private readonly IEnemyAIController _enemyAIController;
        private readonly IBulletsController _bulletsController;

        private readonly Damageable _damageable;


        public EnemyController(EnemySettings enemySettings, GameObject enemyRootGo, GeneratorLevelController generatorLevelController,
            IEnemyAIController enemyAIController, IBulletsController bulletsController)
        {
            _damageable = enemyRootGo.GetComponent<Damageable>();
            _damageable.DeathAction += KillEnemy;
            
            Enemy = new Enemy(enemySettings, enemyRootGo, _damageable);
            _enemyAnimation = new EnemyAnimationController(Enemy);
            _generatorLevelController = generatorLevelController;
            _enemyAIController = enemyAIController;
            _bulletsController = bulletsController;
        }

        public void KillEnemy()
        {
            _enemyAIController.RemoveEnemyAI(_enemyAI);
            _enemyAI.StopAction();
            _enemyAnimation.StopAnimation();
            OnEnemyDead?.Invoke(this);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            _enemyAnimation?.OnFixedUpdate(fixedDeltaTime);
        }

        public void SpawnEnemy()
        {
            _enemyAI ??= new EnemyAI(Enemy, _generatorLevelController.MainBuilding, _enemyAnimation, _bulletsController);
            _enemyAIController.AddEnemyAI(_enemyAI);
            _damageable.Init(Enemy.Stats.Health);
            // SlowlyKilling();
        }

        public void Dispose()
        {
            _enemyAI.Dispose();
            _damageable.DeathAction -= KillEnemy;
            UnityEngine.Object.Destroy(Enemy.RootGameObject);
        }

        private async void SlowlyKilling()
        {
            await Task.Delay(10000);
            // if (!UnityEditor.EditorApplication.isPlaying) return;
            if (!Enemy.RootGameObject.activeSelf) return;
            KillEnemy();
        }
    }
}