using System;
using UnityEngine;


namespace CombatSystem
{
    public interface IBulletController : IOnFixedUpdate, IDisposable
    {
        event Action<IBulletController> BulletFlightIsOver;
        void StartFlight(Damageable target, Vector3 startPosition);
        GameObject Bullet { get; }
        Damageable CurrentTarget { get; }
    }
}