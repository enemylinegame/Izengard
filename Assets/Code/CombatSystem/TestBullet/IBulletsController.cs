using Interfaces;
using System;
using System.Collections;
namespace CombatSystem
{
    public interface IBulletsController : IOnFixedUpdate, IDisposable
    {
        IPoolController<IBulletController> BulletsPool { get; }
        void AddBullet(IBulletController bullet);
        void RemoveBullet(IBulletController bullet);
    }
}