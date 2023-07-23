using System;


namespace Wave
{
    public interface IEnemyController : IOnFixedUpdate, IDisposable
    {
        Enemy Enemy { get; }
        event Action<IEnemyController> OnEnemyDead;
        void KillEnemy();
        void SpawnEnemy();
    }
}