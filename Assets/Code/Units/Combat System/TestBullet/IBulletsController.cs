using Interfaces;
using System;

namespace CombatSystem
{
    public interface IBulletsController : IOnFixedUpdate, IDisposable, IOnController
    {
        IPoolController<IBulletController> BulletsPool { get; }
        void AddBullet(IBulletController bullet);
        void RemoveBullet(IBulletController bullet);
    }
}