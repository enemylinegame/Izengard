using System;
using UnityEngine;


namespace CombatSystem
{
    public interface IBulletController : IOnFixedUpdate, IDisposable
    {
        event Action<IBulletController> BulletFlightIsOver;
        void StartFlight(Vector3 destination, Vector3 startPosition);
        GameObject Bullet { get; }
        Vector3 CurrentTarget { get; }
    }
}