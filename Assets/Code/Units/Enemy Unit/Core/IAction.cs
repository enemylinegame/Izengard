using System;

namespace EnemyUnit.Core
{
    public interface IAction<T>
    {
        event Action<T> OnComplete;
        void StartAction(T target);
        void ClearTarget();
    }
}