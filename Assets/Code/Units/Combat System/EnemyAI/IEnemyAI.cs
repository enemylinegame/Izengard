using System;


namespace CombatSystem
{
    public interface IEnemyAI : IOnUpdate, IDisposable
    {
        bool IsActionComplete { get; }
        void StartAction();
        void StopAction();
    }
}