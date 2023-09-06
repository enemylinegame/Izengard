
using System;

namespace EnemyUnit.Interfaces
{
    public interface IEnemyController : IOnController, IOnUpdate, IOnFixedUpdate, IDisposable
    {
        int Index { get; }

        event Action<int> OnDeath;

        void SetIndex(int index);
    }
}
