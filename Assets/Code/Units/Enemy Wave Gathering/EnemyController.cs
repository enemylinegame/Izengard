using CombatSystem;
using System;
using System.Threading.Tasks;
using Code.BuildingSystem;
using UnityEngine;


namespace Wave
{
    public class EnemyController : IEnemyController
    {
        public Enemy Enemy { get; private set; }
        public event Action<IEnemyController> OnEnemyDead;

       
        private IEnemyAI _enemyAI;
        private readonly BuildingFactory _buildingFactory;
        private readonly IEnemyAIController _enemyAIController;
        private readonly IBulletsController _bulletsController;

        private readonly Damageable _damageable;
        private readonly EnemyTileDislocation _tileDislocation;

        public EnemyController(EnemySettings enemySettings, GameObject enemyRootGo, BuildingFactory buildingFactory,
            IEnemyAIController enemyAIController, IBulletsController bulletsController)
        {
            _damageable = enemyRootGo.GetComponent<Damageable>();
            _damageable.OnDeath += KillEnemy;
            
            Enemy = new Enemy(enemySettings, enemyRootGo, _damageable);
            
            _buildingFactory = buildingFactory;
            _enemyAIController = enemyAIController;
            _bulletsController = bulletsController;

            _tileDislocation = new EnemyTileDislocation(_damageable);
        }

        public void KillEnemy()
        {
            _enemyAIController.RemoveEnemyAI(_enemyAI);
            _enemyAI.StopAction();
           
            _tileDislocation.Off();
            OnEnemyDead?.Invoke(this);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
          
        }

        public void SpawnEnemy()
        {
            _enemyAI ??= new EnemyAI(Enemy, _buildingFactory.MainBuilding, _bulletsController);
            _enemyAIController.AddEnemyAI(_enemyAI);
            _damageable.Init(Enemy.Stats.Health);
            _tileDislocation.On();
            // SlowlyKilling();
        }

        public void Dispose()
        {
            _enemyAI?.Dispose();
            _damageable.OnDeath -= KillEnemy;
            _tileDislocation.Off();
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