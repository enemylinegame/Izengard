using Interfaces;
using System.Collections.Generic;
using UnityEngine;


namespace Wave.Interfaces
{
    public interface ISendingEnemys : IOnUpdate, IOnFixedUpdate
    {
        bool IsSending { get; }
        HashSet<IEnemyController> LifeEnemys { get; }
        void SendEnemys(List<IPoolController<IEnemyController>> enemys, List<Vector3> spawnPositions);
    }
}