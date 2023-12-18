namespace Abstraction
{
    public interface IDefence<TData, TValue>
    {
        TData Data { get; }

        float GetAfterDefDamage(TValue value);
    }
}
