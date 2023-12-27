using System;

namespace Abstraction
{
    public interface IKillable<T>
    {
        event Action<T> OnReachedZeroHealth;
    }
}
