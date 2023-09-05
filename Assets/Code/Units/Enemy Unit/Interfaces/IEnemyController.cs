
using System;

namespace EnemyUnit.Interfaces
{
    public interface IEnemyController : IOnController, IOnUpdate, IOnFixedUpdate, IDisposable
    {
        int Index { get; }
        void SetIndex(int index);
    }
}
