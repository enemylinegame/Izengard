using System;


namespace CombatSystem
{
    public interface IAction<T>
    {
        event Action<T> OnComplete;
        void StartAction(T target);
        void ClearTarget();
    }
}