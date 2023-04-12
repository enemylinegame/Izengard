using System;


namespace Interfaces
{
    public interface IPoolController<T> : IDisposable
    {
        public abstract T GetObjectFromPool();
        public abstract void ReturnObjectInPool(T obj);
    }
}