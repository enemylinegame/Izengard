using Interfaces;
using System.Collections.Generic;


namespace CombatSystem
{
    public class BulletsController : IBulletsController
    {
        private readonly HashSet<IBulletController> _bullets = new HashSet<IBulletController>();
        public IPoolController<IBulletController> BulletsPool { get; private set; }
        private HashSet<IBulletController> _bulletsToAdd = new HashSet<IBulletController>();
        private HashSet<IBulletController> _bulletsToRemove = new HashSet<IBulletController>();


        public BulletsController()
        {
            BulletsPool = new BulletControllerPool(10);
        }

        public void AddBullet(IBulletController bullet)
        {
            _bulletsToAdd.Add(bullet);
        }

        public void RemoveBullet(IBulletController bullet)
        {
            _bulletsToRemove.Remove(bullet);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            foreach (IBulletController bullet in _bullets) bullet.OnFixedUpdate(fixedDeltaTime);
            foreach (IBulletController bullet in _bulletsToRemove) _bullets.Remove(bullet);
            foreach (IBulletController bullet in _bulletsToAdd) _bullets.Add(bullet);
            _bulletsToRemove.Clear();
            _bulletsToAdd.Clear();
        }

        public void Dispose()
        {
            _bullets.Clear();
        }
    }
}